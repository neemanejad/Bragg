using System;
using System.Collections.Generic;

using Godot;

using Redwood.Enemies;
using Redwood.Singletons.RandomUtils;

namespace Redwood.World;

public partial class ChunkGenerator : Node
{
	[Export]
	public FastNoiseLite TerrainElevationMap;

	[Export]
	public FastNoiseLite TerrainMoistureMap;

	[Export]
	public FastNoiseLite EnemyHeightMap;

	[Export]
	public TileGenerator TileGenerator;

	[Export]
	public ChunkData ChunkData;

	[Export]
	public bool UseAreaBasedChunkRendering { get; set; } = false;

	[Export]
	public PackedScene DebuggingChunkCoordinates { get; set; }

	[Export]
	public bool ChunkDebugging { get; set; } = true;

	[Export]
	public EnemyColony EnemyColony { get; set; }

	[Signal]
	public delegate void TileGeneratedEventHandler(Vector2I coords, int sourceId, Vector2I tileAtlas);

	private Player _player = PlayerManager.PlayerNode;


	private readonly HashSet<Vector2I> _generatedChunks = new();
	private readonly Queue<Vector2I> _chunkQueue = new();

	public void SetSeeds(int terrainSeed)
	{
		TerrainElevationMap.Seed = terrainSeed;
		TerrainMoistureMap.Seed = (int)RandomUtils.RNG.Randi();
		EnemyHeightMap.Seed = (int)RandomUtils.RNG.Randi();
	}

	public override void _Process(double delta)
	{
		if (UseAreaBasedChunkRendering)
		{
			CheckChunkLoading_AreaAroundPlayer();
		}
		else
		{
			CheckChunkLoading_PlayerNeighbor();
		}

		ProcessChunkQueue();
	}

	public void GenerateChunk(Vector2I coords)
	{
		if (_generatedChunks.Contains(coords))
		{
			return;
		}

		_generatedChunks.Add(coords);
		_chunkQueue.Enqueue(coords);
	}

	private void ProcessChunkQueue()
	{
		if (_chunkQueue.Count == 0)
		{
			return;
		}

		float terrainElevationSum = 0;
		float enemyElevationSum = 0;

		var chunkCoords = _chunkQueue.Dequeue();
		var chunkWorldCoords = chunkCoords * ChunkData.TilesPerChunk;

		ChunkDebugInfo(chunkCoords);

		for (var x = 0; x < ChunkData.TilesPerChunk; x++)
		{
			for (var y = 0; y < ChunkData.TilesPerChunk; y++)
			{

				var worldX = x + chunkWorldCoords.X;
				var worldY = y + chunkWorldCoords.Y;
				var approxFalloff = new Vector2I(worldX, worldY).DistanceTo(Vector2I.Zero) / (6 * ChunkData.TilesPerChunk);

				var terrainElevationValue = TerrainElevationMap.GetNoise2D(worldX, worldY) - approxFalloff;
				var terrainMoistureValue = TerrainMoistureMap.GetNoise2D(worldX, worldY) - approxFalloff;

				terrainElevationSum += terrainElevationValue;
				enemyElevationSum += EnemyHeightMap.GetNoise2D(worldX, worldY);

				var (sourceId, tileAtlas) = TileGenerator.DetermineTileAtlas(terrainElevationValue, terrainMoistureValue);
				EmitSignal(SignalName.TileGenerated, new Vector2I(worldX, worldY), sourceId, tileAtlas);
			}
		}

		var shouldSpawnColony = Math.Abs((int)enemyElevationSum / ChunkData.TilesPerChunk) > 19 && ((int)terrainElevationSum / ChunkData.TilesPerChunk) > 0;

		if (shouldSpawnColony)
		{
			EnemyColony.Generate(chunkCoords * ChunkData.ChunkSize);
		}
	}

	private void CheckChunkLoading_AreaAroundPlayer()
	{
		var playerGlobalX = _player.GlobalPosition.X;
		var playerGlobalY = _player.GlobalPosition.Y;

		var renderDistanceChunkTopLeftChunk = new Vector2I((int)Math.Floor((playerGlobalX - ChunkData.ChunkLoadThreshold) / ChunkData.ChunkSize), (int)Math.Floor((playerGlobalY + ChunkData.ChunkLoadThreshold) / ChunkData.ChunkSize));
		var renderDistanceChunkBottomRight = new Vector2I((int)Math.Floor((playerGlobalX + ChunkData.ChunkLoadThreshold) / ChunkData.ChunkSize), (int)Math.Floor((playerGlobalY - ChunkData.ChunkLoadThreshold) / ChunkData.ChunkSize));

		for (var i = renderDistanceChunkTopLeftChunk.X; i < renderDistanceChunkBottomRight.X; i++)
		{
			for (var j = renderDistanceChunkBottomRight.Y; j < renderDistanceChunkTopLeftChunk.Y; j++)
			{
				GenerateChunk(new Vector2I(i, j));
			}
		}
	}

	private void CheckChunkLoading_PlayerNeighbor()
	{
		var globalX = _player.GlobalPosition.X;
		var globalY = _player.GlobalPosition.Y;

		var playerChunk = new Vector2I((int)Math.Floor(globalX / ChunkData.ChunkSize), (int)Math.Floor(globalY / ChunkData.ChunkSize));

		var localX = Mathf.PosMod(globalX, ChunkData.ChunkSize);
		var localY = Mathf.PosMod(globalY, ChunkData.ChunkSize);

		if (localX < ChunkData.ChunkLoadThreshold)
		{
			GenerateChunk(playerChunk + Vector2I.Left);
		}
		if (localX > ChunkData.ChunkSize - ChunkData.ChunkLoadThreshold)
		{
			GenerateChunk(playerChunk + Vector2I.Right);
		}
		if (localY < ChunkData.ChunkLoadThreshold)
		{
			GenerateChunk(playerChunk + Vector2I.Up);
		}
		if (localY > ChunkData.ChunkSize - ChunkData.ChunkLoadThreshold)
		{
			GenerateChunk(playerChunk + Vector2I.Down);
		}

		// Top left
		if (localY < ChunkData.ChunkLoadThreshold && localX < ChunkData.ChunkLoadThreshold)
		{
			GenerateChunk(playerChunk + Vector2I.Up + Vector2I.Left);
		}
		// Bottom left
		if (localY > ChunkData.ChunkSize - ChunkData.ChunkLoadThreshold && localX < ChunkData.ChunkLoadThreshold)
		{
			GenerateChunk(playerChunk + Vector2I.Down + Vector2I.Left);
		}
		// Bottom right
		if (localY > ChunkData.ChunkSize - ChunkData.ChunkLoadThreshold && localX > ChunkData.ChunkSize - ChunkData.ChunkLoadThreshold)
		{
			GenerateChunk(playerChunk + Vector2I.Down + Vector2I.Right);
		}
		// Top right
		if (localY < ChunkData.ChunkLoadThreshold && localX > ChunkData.ChunkSize - ChunkData.ChunkLoadThreshold)
		{
			GenerateChunk(playerChunk + Vector2I.Up + Vector2I.Right);
		}
	}

	private void ChunkDebugInfo(Vector2I chunkCoords)
	{
		if (!ChunkDebugging && DebuggingChunkCoordinates != null) return;

		var debugCoordsLabel = DebuggingChunkCoordinates.Instantiate<DebuggingChunkCoordinates>();
		debugCoordsLabel.Coordinates = chunkCoords;
		debugCoordsLabel.GlobalPosition = chunkCoords * ChunkData.ChunkSize;
		AddChild(debugCoordsLabel);
	}

}