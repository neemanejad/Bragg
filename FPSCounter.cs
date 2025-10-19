using Godot;

namespace Redwood;
public partial class FPSCounter : Label
{
	public override void _Process(double delta)
	{
		Text = Engine.GetFramesPerSecond().ToString();
	}
}