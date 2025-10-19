using Godot;

namespace Redwood.World;

public partial class Chunk : TileMapLayer
{
	[Export]
	public DebuggingChunkCoordinates DebuggingChunkCoordinates;

	[Export]
	public bool ChunkDebugging = true;

	[Export]
	public ChunkData ChunkData;

	public Vector2I ChunkCoordinates { get; private set; }

	public override void _Ready()
	{
		ChunkDebugInfo();
	}

	public void Prepare(Vector2I chunkCoords)
	{
		ChunkCoordinates = chunkCoords;
	}

	private void ChunkDebugInfo()
	{
		if (!ChunkDebugging) return;

		DebuggingChunkCoordinates.SetDebugCoords(ChunkCoordinates);
		DebuggingChunkCoordinates.GlobalPosition = ChunkCoordinates * ChunkData.ChunkSize;
	}
}