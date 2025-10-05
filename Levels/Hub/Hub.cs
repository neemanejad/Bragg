namespace Redwood.Levels.Hub;

using Godot;

using Singletons.WorldManager;

public partial class Hub : Node2D
{
	public override void _Ready()
	{
		WorldManager.Instance.Generate(4);
	}
}