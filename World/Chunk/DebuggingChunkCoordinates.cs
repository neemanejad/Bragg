using Godot;

namespace Redwood.World;

public partial class DebuggingChunkCoordinates : Control
{
	[Export]
	public Label CoordinatesLabel { get; set; }

	public void SetDebugCoords(Vector2I coords)
	{
		CoordinatesLabel.Text = coords.ToString();
	}
}