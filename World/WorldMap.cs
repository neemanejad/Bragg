using Godot;

namespace Redwood.World;

public partial class WorldMap : CanvasLayer
{
	[Export]
	public Map Map;

	public override void _Process(double delta)
	{
		if (Input.IsActionJustPressed("map"))
		{
			Map.Visible = !Map.Visible;
		}
	}
}