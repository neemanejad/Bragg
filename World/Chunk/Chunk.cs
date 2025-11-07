using Godot;

using Redwood.Enemies;

namespace Redwood.World;

public partial class Chunk : Node2D
{
	[Export]
	public DebuggingChunkCoordinates DebuggingChunkCoordinates;

	[Export]
	public bool ChunkDebugging = true;

	[Export]
	public ChunkData ChunkData;

	[Export]
	public EnemyColony EnemyColony;

	public Vector2I ChunkCoordinates { get; private set; }

	[Export]
	public TileMapLayer WaterTileMapLayer;

	public float AverageTerrainElevation { get; set; } = 0;
	public float AverageEnemyNoise { get; set; } = 0;

	public override void _Ready()
	{
		ChunkDebugInfo();
	}

	public void Complete()
	{
		// TODO: Consolidate any noise values in one place
		if (AverageTerrainElevation > -0.3 && AverageEnemyNoise > 4)
		{
			EnemyColony.Generate(ChunkCoordinates * ChunkData.ChunkSize);
		}
	}

	public void Prepare(Vector2I chunkCoords)
	{
		GlobalPosition = chunkCoords * ChunkData.ChunkSize;
		ChunkCoordinates = chunkCoords;
	}

	private void ChunkDebugInfo()
	{
		if (!ChunkDebugging) return;

		DebuggingChunkCoordinates.SetDebugCoords(ChunkCoordinates);
	}
}