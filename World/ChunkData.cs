using Godot;

namespace Redwood.World;

[GlobalClass]
public partial class ChunkData : Resource
{
	[Export]
	public int TilesPerChunk = 16; // X and Y

	[Export]
	public int TileSize = 16; // Size of individual tiles in px

	public int ChunkSize => TilesPerChunk * TileSize;

}