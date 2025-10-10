using System;
using System.Collections.Generic;

using Godot;

using Redwood.Enemies;

namespace Redwood.World;

public partial class ProcWorld : TileMapLayer
{
	[Export]
	public NoiseTexture2D NoiseTexture2D { get; set; }

	[Export]
	public NoiseTexture2D TerrainTexture { get; set; }

	[Export]
	public bool UseAreaBasedChunkRendering { get; set; } = false;

	[Export]
	public NoiseTexture2D EnemySpawnTexture { get; set; }

	[Export]
	public PackedScene DebuggingChunkCoordinates { get; set; }

	[Export]
	public EnemyColony EnemyColony { get; set; }

	[Export]
	public Chunk Chunk { get; set; }

	[Export]
	public bool ChunkDebugging { get; set; } = true;

	private Noise _terrainNoise;
	private Noise _enemySpawnNoise;

	private readonly HashSet<Vector2I> _generatedChunks = new();
	private readonly Queue<Vector2I> _chunkQueue = new();

	private Player _player = PlayerManager.PlayerNode;

	public override void _Ready()
	{
		_terrainNoise = TerrainTexture.Noise;
		_enemySpawnNoise = EnemySpawnTexture.Noise;

		GenerateChunk(Vector2I.Zero);
	}

	private void DetermineCellPlacement(Vector2I coords, float noiseVal)
	{
		if (noiseVal > 0.0f)
		{
			SetCell(coords, 0, Chunk.PlainGrassAtlas);
		}
		else if (noiseVal > -0.03)
		{
			SetCell(coords, 1, Chunk.SandAtlas);
		}
		else if (noiseVal > -0.05)
		{
			SetCell(coords, 1, Chunk.ShallowWaterAtlas1);
		}
		else if (noiseVal > -0.075)
		{
			SetCell(coords, 1, Chunk.ShallowWaterAtlas2);
		}
		else if (noiseVal > -0.1)
		{
			SetCell(coords, 1, Chunk.DeepWaterAtlas1);
		}
		else
		{
			SetCell(coords, 1, Chunk.DeepWaterAtlas2);
		}
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

	private void GenerateChunk(Vector2I coords)
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

		float terrainNoiseValueSum = 0;
		float enemyNoiseValueSum = 0;

		var chunkCoords = _chunkQueue.Dequeue();
		var chunkWorldCoords = chunkCoords * Chunk.TilesPerChunk;

		ChunkDebugInfo(chunkCoords);

		for (var x = 0; x < Chunk.TilesPerChunk; x++)
		{
			for (var y = 0; y < Chunk.TilesPerChunk; y++)
			{
				var worldX = x + chunkWorldCoords.X;
				var worldY = y + chunkWorldCoords.Y;

				var terrainNoiseValue = _terrainNoise.GetNoise2D(worldX, worldY);

				terrainNoiseValueSum += terrainNoiseValue;
				enemyNoiseValueSum += _enemySpawnNoise.GetNoise2D(worldX, worldY);

				DetermineCellPlacement(new(worldX, worldY), terrainNoiseValue);
			}
		}

		var shouldSpawnColony = Math.Abs((int)enemyNoiseValueSum / Chunk.TilesPerChunk) > 19 && ((int)terrainNoiseValueSum / Chunk.TilesPerChunk) > 0;

		if (shouldSpawnColony)
		{
			EnemyColony.Generate(chunkCoords * Chunk.ChunkSize);
		}
	}

	private void CheckChunkLoading_AreaAroundPlayer()
	{
		var playerGlobalX = _player.GlobalPosition.X;
		var playerGlobalY = _player.GlobalPosition.Y;

		var playerChunk = new Vector2I((int)Math.Floor(playerGlobalX / Chunk.ChunkSize), (int)Math.Floor(playerGlobalX / Chunk.ChunkSize));
		var renderDistanceChunkTopLeftChunk = new Vector2I((int)Math.Floor((playerGlobalX - Chunk.ChunkLoadThreshold) / Chunk.ChunkSize), (int)Math.Floor((playerGlobalY + Chunk.ChunkLoadThreshold) / Chunk.ChunkSize));
		var renderDistanceChunkBottomRight = new Vector2I((int)Math.Floor((playerGlobalX + Chunk.ChunkLoadThreshold) / Chunk.ChunkSize), (int)Math.Floor((playerGlobalY - Chunk.ChunkLoadThreshold) / Chunk.ChunkSize));

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

		var playerChunk = new Vector2I((int)Math.Floor(globalX / Chunk.ChunkSize), (int)Math.Floor(globalY / Chunk.ChunkSize));

		var localX = Mathf.PosMod(globalX, Chunk.ChunkSize);
		var localY = Mathf.PosMod(globalY, Chunk.ChunkSize);

		if (localX < Chunk.ChunkLoadThreshold)
		{
			GenerateChunk(playerChunk + Vector2I.Left);
		}
		if (localX > Chunk.ChunkSize - Chunk.ChunkLoadThreshold)
		{
			GenerateChunk(playerChunk + Vector2I.Right);
		}
		if (localY < Chunk.ChunkLoadThreshold)
		{
			GenerateChunk(playerChunk + Vector2I.Up);
		}
		if (localY > Chunk.ChunkSize - Chunk.ChunkLoadThreshold)
		{
			GenerateChunk(playerChunk + Vector2I.Down);
		}

		// Top left
		if (localY < Chunk.ChunkLoadThreshold && localX < Chunk.ChunkLoadThreshold)
		{
			GenerateChunk(playerChunk + Vector2I.Up + Vector2I.Left);
		}
		// Bottom left
		if (localY > Chunk.ChunkSize - Chunk.ChunkLoadThreshold && localX < Chunk.ChunkLoadThreshold)
		{
			GenerateChunk(playerChunk + Vector2I.Down + Vector2I.Left);
		}
		// Bottom right
		if (localY > Chunk.ChunkSize - Chunk.ChunkLoadThreshold && localX > Chunk.ChunkSize - Chunk.ChunkLoadThreshold)
		{
			GenerateChunk(playerChunk + Vector2I.Down + Vector2I.Right);
		}
		// Top right
		if (localY < Chunk.ChunkLoadThreshold && localX > Chunk.ChunkSize - Chunk.ChunkLoadThreshold)
		{
			GenerateChunk(playerChunk + Vector2I.Up + Vector2I.Right);
		}
	}

	private void ChunkDebugInfo(Vector2I chunkCoords)
	{
		if (!ChunkDebugging && DebuggingChunkCoordinates != null) return;

		var debugCoordsLabel = DebuggingChunkCoordinates.Instantiate<DebuggingChunkCoordinates>();
		debugCoordsLabel.Coordinates = chunkCoords;
		debugCoordsLabel.GlobalPosition = chunkCoords * Chunk.ChunkSize;
		AddChild(debugCoordsLabel);
	}
}