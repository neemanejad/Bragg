namespace Redwood.Deployment;

using Godot;

using Redwood.UI;

public partial class Deployment : Area2D
{
	[Export]
	UISignalEmitter _uiSignalEmitter { get; set; }
	private PackedScene _deploymentSelectionScene;

	public override void _Ready()
	{
		_deploymentSelectionScene = ResourceLoader.Load<PackedScene>("res://UI/Deployment/DeploymentSelection.tscn");
	}

	public override void _Process(double delta)
	{
		var playerInArea = GetOverlappingBodies().Contains(PlayerManager.PlayerNode);

		var isInteracting = Input.IsActionJustPressed("interact");

		if (playerInArea && isInteracting)
		{
			_uiSignalEmitter.SignalAddUI(UIType.DeploymentMenu);
		}
	}
}