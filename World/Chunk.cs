using Godot;

namespace Redwood.World;

[GlobalClass]
public partial class Chunk : Resource
{
	[Export]
	public int SourceId = 0;

	[Export]
	public int ChunkLoadThreshold = 1024;

	[Export]
	public int TilesPerChunk = 64; // X and Y

	[Export]
	public int TileSize = 16; // Size of individual tiles

	[Export]
	public Vector2I PlainGrassAtlas = new(2, 0);

	[Export]
	public Vector2I SandAtlas = new(0, 0);

	[Export]
	public Vector2I ShallowWaterAtlas1 = new(1, 0);

	[Export]
	public Vector2I ShallowWaterAtlas2 = new(2, 0);

	[Export]
	public Vector2I DeepWaterAtlas1 = new(3, 0);

	[Export]
	public Vector2I DeepWaterAtlas2 = new(4, 0);

	public int ChunkSize => TilesPerChunk * TileSize;

}