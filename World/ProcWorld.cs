using System;
using System.Collections.Generic;

using Godot;

namespace Redwood.World;

public partial class ProcWorld : TileMapLayer
{
	[Export]
	public NoiseTexture2D NoiseTexture2D { get; set; }

	private Noise _noise;

	private int _tilesPerChunk = 64; // X and Y
	private int _tileSize = 16; // Size of individual tiles
	private int _chunkSize;

	private int _chunkLoadThreshold = 8096;

	private int _sourceId = 0;
	private Vector2I _plainGrassAtlas = new(2, 0);
	private Vector2I _sandAtlas = new(0, 0);
	private Vector2I _shallowWaterAtlas1 = new(1, 0);
	private Vector2I _shallowWaterAtlas2 = new(2, 0);
	private Vector2I _deepWaterAtlas1 = new(3, 0);
	private Vector2I _deepWaterAtlas2 = new(4, 0);

	private readonly HashSet<Vector2I> _generatedChunks = new();
	private readonly Queue<Vector2I> _chunkQueue = new();

	private Player _player = PlayerManager.PlayerNode;

	public override void _Ready()
	{
		_chunkSize = _tilesPerChunk * _tileSize;

		_noise = NoiseTexture2D.Noise;

		GenerateChunk(Vector2I.Zero);
	}

	private void DetermineCellPlacement(Vector2I coords, float noiseVal)
	{
		if (noiseVal > 0.0f)
		{
			SetCell(coords, 0, _plainGrassAtlas);
		}
		else if (noiseVal > -0.03)
		{
			SetCell(coords, 1, _sandAtlas);
		}
		else if (noiseVal > -0.05)
		{
			SetCell(coords, 1, _shallowWaterAtlas1);
		}
		else if (noiseVal > -0.075)
		{
			SetCell(coords, 1, _shallowWaterAtlas2);
		}
		else if (noiseVal > -0.1)
		{
			SetCell(coords, 1, _deepWaterAtlas1);
		}
		else
		{
			SetCell(coords, 1, _deepWaterAtlas2);
		}
	}

	public override void _PhysicsProcess(double delta)
	{
		CheckChunkLoading();
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

		var chunkCoords = _chunkQueue.Dequeue();
		var chunkWorldCoords = chunkCoords * _tilesPerChunk;

		for (var x = 0; x < _tilesPerChunk; x++)
		{
			for (var y = 0; y < _tilesPerChunk; y++)
			{
				var worldX = x + chunkWorldCoords.X;
				var worldY = y + chunkWorldCoords.Y;

				DetermineCellPlacement(new(worldX, worldY), _noise.GetNoise2D(worldX, worldY));
			}
		}
	}

	private void CheckChunkLoading()
	{
		var globalX = _player.GlobalPosition.X;
		var globalY = _player.GlobalPosition.Y;

		var playerChunk = new Vector2I((int)Math.Floor(globalX / _chunkSize), (int)Math.Floor(globalY / _chunkSize));

		var localX = Mathf.PosMod(globalX, _chunkSize);
		var localY = Mathf.PosMod(globalY, _chunkSize);

		if (localX < _chunkLoadThreshold)
		{
			GenerateChunk(playerChunk + Vector2I.Left);
		}
		if (localX > _chunkSize - _chunkLoadThreshold)
		{
			GenerateChunk(playerChunk + Vector2I.Right);
		}
		if (localY < _chunkLoadThreshold)
		{
			GenerateChunk(playerChunk + Vector2I.Up);
		}
		if (localY > _chunkSize - _chunkLoadThreshold)
		{
			GenerateChunk(playerChunk + Vector2I.Down);
		}

		// Top left
		if (localY < _chunkLoadThreshold && localX < _chunkLoadThreshold)
		{
			GenerateChunk(playerChunk + Vector2I.Up + Vector2I.Left);
		}
		// Bottom left
		if (localY > _chunkSize - _chunkLoadThreshold && localX < _chunkLoadThreshold)
		{
			GenerateChunk(playerChunk + Vector2I.Down + Vector2I.Left);
		}
		// Bottom right
		if (localY > _chunkSize - _chunkLoadThreshold && localX > _chunkSize - _chunkLoadThreshold)
		{
			GenerateChunk(playerChunk + Vector2I.Down + Vector2I.Right);
		}
		// Top right
		if (localY < _chunkLoadThreshold && localX > _chunkSize - _chunkLoadThreshold)
		{
			GenerateChunk(playerChunk + Vector2I.Up + Vector2I.Right);
		}
	}
}