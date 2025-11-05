using Godot;

namespace Redwood.World;

public class TileGenerator
{
	private static Vector2I s_lightPlainGrassAtlas = new(1, 0);

	private static Vector2I s_darkPlainGrassAtlas = new(2, 0);

	private static Vector2I s_sandAtlas = new(0, 0);


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
		else
		{
			sourceId = -1;
			tileAtlas = Vector2I.Zero;
		}

		return (sourceId, tileAtlas);
	}
}