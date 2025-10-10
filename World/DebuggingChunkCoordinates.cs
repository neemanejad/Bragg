using Godot;

namespace Redwood.World;

public partial class DebuggingChunkCoordinates : Control
{
	[Export]
	public Label CoordinatesLabel { get; set; }

	public Vector2I Coordinates { get; set; }

	public override void _Ready()
	{
		CoordinatesLabel.Text = Coordinates.ToString();
	}
}