using Godot;

using Redwood.Singletons.WorldManager;

namespace Redwood.Hub;

public partial class Hub : Node2D
{
	public override void _Ready()
	{
		WorldManager.Instance.Generate(4);
	}
}