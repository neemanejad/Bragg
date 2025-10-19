using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using Godot;

using Redwood.Singletons.RandomUtils;
using Redwood.World.Utils;

namespace Redwood.World;

public partial class ProceduralWorld : Node2D
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

	/// <summary>
	/// Quantity of chunks that should be worked on per frame - affects performance greatly
	/// </summary>
	private int _chunksToProcessPerFrame = 8;

	/// <summary>
	/// Number of chunks in any cardinal direction before the noise falls off (land -> ocean)
	/// </summary>
	private int _chunkFalloff = 32;

	/// <summary>
	/// The radius of the circle and search cone for queueing chunks
	/// </summary>
	private int _renderDistance = 8;

	/// <summary>
	/// Contains chunks that are fully complete
	/// </summary>
	private readonly HashSet<Vector2I> _completeChunksCoordinates = new();

	/// <summary>
	/// Tracks chunks currently in progress alongside where they were left off
	/// </summary>
	private readonly Dictionary<Vector2I, int> _inProgressChunks = new();

	/// <summary>
	/// Chunk queue that prioritizes those closer to the player node
	/// </summary>
	private readonly PriorityQueue<Chunk, float> _chunkQueue = new();

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

		var initialSize = 4;

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
	}

	public override void _Process(double delta)
	{
		LoadSurroundingChunks();

		ProcessChunkQueue();
	}

	public void QueueChunkToProcess(Vector2I coords)
	{
		// Skip over chunks already generated or are in progress
		if (_completeChunksCoordinates.Contains(coords) || _inProgressChunks.ContainsKey(coords))
		{
			return;
		}

		var chunkToQueue = Chunk.Instantiate<Chunk>();
		chunkToQueue.Prepare(coords);



		_inProgressChunks.Add(coords, 0);


		var distanceToPlayerChunk = coords.DistanceTo(_lastKnownPlayerChunkCoordinates);
		_chunkQueue.Enqueue(chunkToQueue, distanceToPlayerChunk);
	}

	private void ProcessChunkQueue()
	{
		for (var chunksProcessed = 0; chunksProcessed < _chunksToProcessPerFrame; chunksProcessed++)
		{
			if (_chunkQueue.Count == 0)
			{
				break;
			}

			float terrainElevationSum = 0;
			float enemyElevationSum = 0;

			var chunk = _chunkQueue.Dequeue();
			var chunkCoords = chunk.ChunkCoordinates;

			if (!chunk.IsInsideTree())
			{
				AddChild(chunk);
			}

			var chunkWorldCoords = chunkCoords * ChunkData.TilesPerChunk;

			_inProgressChunks.TryGetValue(chunkCoords, out var rowNeedingWork);

			Parallel.For(0, ChunkData.TilesPerChunk, (y) =>
			{
				var worldX = rowNeedingWork + chunkWorldCoords.X;
				var worldY = y + chunkWorldCoords.Y;
				var approxFalloff = new Vector2I(worldX, worldY).DistanceTo(Vector2I.Zero) / (_chunkFalloff * ChunkData.TilesPerChunk);

				var terrainElevationValue = TerrainElevationMap.GetNoise2D(worldX, worldY) - approxFalloff;
				var terrainMoistureValue = TerrainMoistureMap.GetNoise2D(worldX, worldY) - approxFalloff;

				terrainElevationSum += terrainElevationValue;
				enemyElevationSum += EnemyHeightMap.GetNoise2D(worldX, worldY);

				var (sourceId, tileAtlas) = TileGenerator.DetermineTileAtlas(terrainElevationValue, terrainMoistureValue);
				chunk.CallDeferred(TileMapLayer.MethodName.SetCell, new Vector2I(worldX, worldY), sourceId, tileAtlas);
			});

			rowNeedingWork += 1;
			_inProgressChunks[chunkCoords] = rowNeedingWork;

			if (rowNeedingWork == ChunkData.TilesPerChunk)
			{
				_inProgressChunks.Remove(chunkCoords);
				_completeChunksCoordinates.Add(chunkCoords);
			}
			else
			{
				var distanceToPlayerChunk = chunkCoords.DistanceTo(_lastKnownPlayerChunkCoordinates);
				_chunkQueue.Enqueue(chunk, distanceToPlayerChunk);
			}
		}
	}

	private void LoadSurroundingChunks()
	{
		var chunkCoordinates = GetPlayerChunkCoordinates();

		// Skip looping through chunks to generated if player hasn't moved from a chunk
		if (chunkCoordinates.Equals(_lastKnownPlayerChunkCoordinates) || _player.Velocity.Equals(Vector2.Zero))
		{
			return;
		}

		// The player's current chunk position has changed
		_lastKnownPlayerChunkCoordinates = chunkCoordinates;

		var chunkX = chunkCoordinates.X;
		var chunkY = chunkCoordinates.Y;

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
}