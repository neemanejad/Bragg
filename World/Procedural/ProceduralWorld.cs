using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading;

using Godot;

using Redwood.PlayerEntity;
using Redwood.Singletons;
using Redwood.Singletons.RandomUtils;
using Redwood.World.Utils;

namespace Redwood.World;

public partial class ProceduralWorld : TileMapLayer
{
	[Export]
	public FastNoiseLite TerrainElevationMap;

	[Export]
	public FastNoiseLite TerrainMoistureMap;

	[Export]
	public FastNoiseLite EnemyHeightMap;

	[Export]
	public ChunkData ChunkData;

	[Export]
	public PackedScene Chunk;

	private Player _player = PlayerManager.Instance.PlayerNode;

	private Thread _workerThread;

	private bool _workerThreadActive = true;

	/// <summary>
	/// Quantity of chunks that should be worked on per frame - affects performance greatly
	/// </summary>
	private int _chunksToProcessPerFrame = 8;

	/// <summary>
	/// Number of chunks in any cardinal direction before the noise falls off (land -> ocean)
	/// </summary>
	private int _chunkFalloff = 8;

	/// <summary>
	/// The radius of the circle and search cone for queueing chunks
	/// </summary>
	private int _renderDistance = 2;

	/// <summary>
	/// Contains chunks that are fully complete
	/// </summary>
	private readonly ConcurrentDictionary<Vector2I, Godot.Collections.Array<Vector2I>> _completeChunksCoordinates = new();

	/// <summary>
	/// Chunk queue that prioritizes those closer to the player node
	/// </summary>
	private readonly PriorityQueue<Chunk, float> _chunkQueue = new();

	private readonly Queue<Chunk> _chunksReady = new();

	/// <summary>
	/// Last known position of the player node, attempts to update every frame
	/// </summary>
	private Vector2I _lastKnownPlayerChunkCoordinates = Vector2I.Zero;

	public void SetSeeds()
	{
		TerrainElevationMap.Seed = (int)RandomUtils.RNG.Seed; ;
		TerrainMoistureMap.Seed = (int)RandomUtils.RNG.Seed;
		EnemyHeightMap.Seed = (int)RandomUtils.RNG.Seed;
	}

	public override void _Ready()
	{
		SetSeeds();

		var initialSize = 2;

		for (var x = -initialSize; x <= initialSize; x++)
		{
			for (var y = -initialSize; y <= initialSize; y++)
			{
				QueueChunkToProcess(new(x, y));
			}
		}

		while (_chunkQueue.Count != 0)
		{
			ProcessChunkQueue();
		}

		while (_chunksReady.TryDequeue(out var chunk))
		{
			_completeChunksCoordinates.TryGetValue(chunk.ChunkCoordinates, out var cells);
			SetCellsTerrainConnect(cells, 0, 0);
			AddChild(chunk);
		}

		StartWorkerThread();
	}

	private void StartWorkerThread()
	{
		void Work()
		{
			while (_workerThreadActive)
			{
				LoadSurroundingChunks();
				ProcessChunkQueue();
			}
		}

		_workerThread = new(new ThreadStart(Work));

		_workerThread.Start();
	}

	public override void _Process(double delta)
	{
		var chunkCoordinates = GetPlayerChunkCoordinates();

		// Skip looping through chunks to generated if player hasn't moved from a chunk
		if (!chunkCoordinates.Equals(_lastKnownPlayerChunkCoordinates))
		{
			_lastKnownPlayerChunkCoordinates = chunkCoordinates;
		}

		if (_chunksReady.TryDequeue(out var chunk))
		{
			_completeChunksCoordinates.TryGetValue(chunk.ChunkCoordinates, out var cells);
			SetCellsTerrainConnect(cells, 0, 0);
			AddChild(chunk);
		}
	}

	public void QueueChunkToProcess(Vector2I coords)
	{
		// Skip over chunks already generated or are in progress
		if (_completeChunksCoordinates.ContainsKey(coords))
		{
			return;
		}

		var chunkToQueue = Chunk.Instantiate<Chunk>();
		chunkToQueue.Prepare(coords);

		var distanceToPlayerChunk = coords.DistanceTo(_lastKnownPlayerChunkCoordinates);
		_chunkQueue.Enqueue(chunkToQueue, distanceToPlayerChunk);
	}

	private void ProcessChunkQueue()
	{
		if (_chunkQueue.Count == 0)
		{
			return;
		}

		float terrainElevationSum = 0;
		float enemyElevationSum = 0;

		var chunk = _chunkQueue.Dequeue();
		var chunkCoords = chunk.ChunkCoordinates;

		var chunkWorldCoords = chunkCoords * ChunkData.TilesPerChunk;

		var landTerrain = new Godot.Collections.Array<Vector2I>();
		var waterTerrain = new Godot.Collections.Array<Vector2I>();

		for (var x = 0; x < ChunkData.TilesPerChunk; x++)
		{
			for (var y = 0; y < ChunkData.TilesPerChunk; y++)
			{
				var worldX = x + chunkWorldCoords.X;
				var worldY = y + chunkWorldCoords.Y;
				var approxFalloff = new Vector2I(worldX, worldY).DistanceTo(Vector2I.Zero) / (_chunkFalloff * ChunkData.TilesPerChunk);

				var terrainElevationValue = TerrainElevationMap.GetNoise2D(worldX, worldY) - approxFalloff;
				var terrainMoistureValue = TerrainMoistureMap.GetNoise2D(worldX, worldY) - approxFalloff;

				terrainElevationSum += terrainElevationValue;
				enemyElevationSum += EnemyHeightMap.GetNoise2D(worldX, worldY);

				// TODO: Clean this up
				var (sourceId, tileAtlas) = TileGenerator.DetermineTileAtlas(terrainElevationValue, terrainMoistureValue);
				if (sourceId != -1)
				{
					landTerrain.Add(new(worldX, worldY));
				}

				waterTerrain.Add(new(x, y));
			}
		}

		chunk.WaterTileMapLayer.SetCellsTerrainConnect(waterTerrain, 1, 0);

		chunk.AverageTerrainElevation += terrainElevationSum / ChunkData.TilesPerChunk;
		chunk.AverageEnemyNoise += enemyElevationSum / ChunkData.TilesPerChunk;

		if (_completeChunksCoordinates.TryAdd(chunkCoords, landTerrain))
		{
			_chunksReady.Enqueue(chunk);
			chunk.Complete();
		}
	}

	private void LoadSurroundingChunks()
	{
		var chunkX = _lastKnownPlayerChunkCoordinates.X;
		var chunkY = _lastKnownPlayerChunkCoordinates.Y;

		var top = chunkY - _renderDistance;
		var left = chunkX - _renderDistance;
		var bottom = chunkY + _renderDistance;
		var right = chunkX + _renderDistance;

		for (var boundingBoxX = left; boundingBoxX <= right; boundingBoxX++)
		{
			for (var boundingBoxY = top; boundingBoxY <= bottom; boundingBoxY++)
			{
				var chunkCoords = new Vector2I(boundingBoxX, boundingBoxY);

				var shouldChunkBeQueued = ChunkCircleUtils.IsChunkInsideCircle(chunkCoords, _lastKnownPlayerChunkCoordinates, _renderDistance) && ChunkCircleUtils.IsChunkInCone(chunkCoords, _lastKnownPlayerChunkCoordinates);
				if (!shouldChunkBeQueued) continue;


				QueueChunkToProcess(chunkCoords);
			}

		}
	}

	private Vector2I GetPlayerChunkCoordinates()
	{
		var playerGlobalX = _player.GlobalPosition.X;
		var playerGlobalY = _player.GlobalPosition.Y;

		// The chunk the player is currently within
		return new Vector2I((int)Math.Floor(playerGlobalX / ChunkData.ChunkSize), (int)Math.Floor(playerGlobalY / ChunkData.ChunkSize));
	}

	public override void _ExitTree()
	{
		_workerThreadActive = false;
	}
}