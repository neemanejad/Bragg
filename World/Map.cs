using Godot;

namespace Redwood.World;

public partial class Map : TextureRect
{
	[Export]
	public FastNoiseLite TerrainElevationMap { get; set; }

	[Export]
	Sprite2D _playerIcon;

	[Export]
	public ChunkData ChunkData;

	public Image Image = Image.CreateEmpty(900, 900, false, Image.Format.Rgba8);

	public ImageTexture ImageTexture;

	private int _mapWidth = 450;
	private int _mapHeight = 450;


	public override void _Process(double delta)
	{
		_playerIcon.Position = (PlayerManager.Instance.PlayerNode.GlobalPosition / (ChunkData.TilesPerChunk / 4)) + new Vector2(_mapWidth, _mapHeight);
	}

	public void GenerateMap(int terrainSeed)
	{
		TerrainElevationMap.Seed = terrainSeed;

		for (var x = -_mapWidth; x < _mapWidth; x++)
		{
			for (var y = -_mapHeight; y < _mapHeight; y++)
			{
				var distance = new Vector2I(x, y).DistanceTo(Vector2I.Zero) / (6 * ChunkData.TilesPerChunk);
				var terrainElevationValue = TerrainElevationMap.GetNoise2D(x, y) - distance;

				string color;

				// TODO: Clean this up
				if (terrainElevationValue > -0.3)
				{
					color = "#b1d354";
				}
				else if (terrainElevationValue > -0.35)
				{
					color = "#e7d593";
				}
				else if (terrainElevationValue > -0.4)
				{
					color = "#b1c9a7";
				}
				else if (terrainElevationValue > -0.5)
				{
					color = "#77c0b4";
				}
				else if (terrainElevationValue > -0.6)
				{
					color = "#4ebcb9";
				}
				else
				{
					color = "#42acaf";
				}

				Image.SetPixel(x + 450, y + 450, new Color(color));
			}

		}

		Texture = ImageTexture.CreateFromImage(Image);
	}
}