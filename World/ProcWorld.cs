using Godot;
using Godot.Collections;

namespace Redwood.World;

public partial class ProcWorld : TileMapLayer
{
	[Export]
	public NoiseTexture2D NoiseTexture2D { get; set; }

	private Noise _noise;

	private int _height = 512;
	private int _width = 512;

	private int _sourceId = 0;
	private Vector2I _plainGrassAtlas = new(2, 0);
	private Vector2I _sandAtlas = new(0, 0);
	private Vector2I _shallowWaterAtlas1 = new(1, 0);
	private Vector2I _shallowWaterAtlas2 = new(2, 0);
	private Vector2I _deepWaterAtlas1 = new(3, 0);
	private Vector2I _deepWaterAtlas2 = new(4, 0);

	public override void _Ready()
	{
		_noise = NoiseTexture2D.Noise;

		GenerateWorld();
	}

	private void GenerateWorld()
	{
		var genWidth = _width / 2;
		var genHeight = _height / 2;

		for (var x = -genWidth; x < genWidth; x++)
		{
			for (var y = -genHeight; y < genHeight; y++)
			{
				DetermineCellPlacement(new(x, y), _noise.GetNoise2D(x, y));
			}
		}
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

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
}