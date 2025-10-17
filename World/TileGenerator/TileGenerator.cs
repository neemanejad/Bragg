using Godot;

namespace Redwood.World;

public partial class TileGenerator : Node
{
	[Export]
	public ChunkData ChunkData { get; set; }

	public (int sourceId, Vector2I tileAtlas) DetermineTileAtlas(float elevation, float moisture)
	{
		int sourceId;

		Vector2I tileAtlas;

		if (elevation > -0.3)
		{
			sourceId = 0;
			tileAtlas = moisture > -0.3 ? ChunkData.DarkPlainGrassAtlas : ChunkData.LightPlainGrassAtlas;
		}
		else if (elevation > -0.35)
		{
			sourceId = 1;
			tileAtlas = ChunkData.SandAtlas;
		}
		else if (elevation > -0.4)
		{
			sourceId = 1;
			tileAtlas = ChunkData.ShallowWaterAtlas1;
		}
		else if (elevation > -0.5)
		{
			sourceId = 1;
			tileAtlas = ChunkData.ShallowWaterAtlas2;
		}
		else if (elevation > -0.6)
		{
			sourceId = 1;
			tileAtlas = ChunkData.DeepWaterAtlas1;
		}
		else
		{
			sourceId = 1;
			tileAtlas = ChunkData.DeepWaterAtlas2;
		}

		return (sourceId, tileAtlas);
	}
}