namespace Redwood.Deployment;

using Godot;

public partial class Deployment : Area2D
{
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
			AddChild(_deploymentSelectionScene.Instantiate());
		}
	}
}