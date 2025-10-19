using Godot;

namespace Redwood.World;

public class TileGenerator
{
	private static Vector2I s_lightPlainGrassAtlas = new(1, 0);

	private static Vector2I s_darkPlainGrassAtlas = new(2, 0);

	private static Vector2I s_sandAtlas = new(0, 0);

	private static Vector2I s_shallowWaterAtlas1 = new(1, 0);

	private static Vector2I s_shallowWaterAtlas2 = new(2, 0);

	private static Vector2I s_deepWaterAtlas1 = new(3, 0);

	private static Vector2I s_deepWaterAtlas2 = new(4, 0);

	public static (int sourceId, Vector2I tileAtlas) DetermineTileAtlas(float elevation, float moisture)
	{
		int sourceId;

		Vector2I tileAtlas;

		if (elevation > -0.3)
		{
			sourceId = 0;
			tileAtlas = moisture > -0.3 ? s_darkPlainGrassAtlas : s_lightPlainGrassAtlas;
		}
		else if (elevation > -0.35)
		{
			sourceId = 1;
			tileAtlas = s_sandAtlas;
		}
		else if (elevation > -0.4)
		{
			sourceId = 1;
			tileAtlas = s_shallowWaterAtlas1;
		}
		else if (elevation > -0.5)
		{
			sourceId = 1;
			tileAtlas = s_shallowWaterAtlas2;
		}
		else if (elevation > -0.6)
		{
			sourceId = 1;
			tileAtlas = s_deepWaterAtlas1;
		}
		else
		{
			sourceId = 1;
			tileAtlas = s_deepWaterAtlas2;
		}

		return (sourceId, tileAtlas);
	}
}