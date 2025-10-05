using Godot;

using Redwood.Singletons.WorldManager;

namespace Redwood.UI.Deployment;

public partial class DeploymentSelection : CanvasLayer
{
	private Button _button;

	private GridContainer _deploymentCardGrid;

	public override void _Ready()
	{
		ProcessMode = ProcessModeEnum.Always;

		_button = GetNode<Button>("Control/MarginContainer/PanelContainer/MarginContainer/VBoxContainer/HeaderContainer/HBoxContainer/Button");
		_button.Pressed += () => QueueFree();

		_deploymentCardGrid = GetNode<GridContainer>("Control/MarginContainer/PanelContainer/MarginContainer/VBoxContainer/DeploymentCardGrid");

		GenerateDeploymentCards();

		GetTree().Paused = true;
	}

	private void GenerateDeploymentCards()
	{
		var worlds = WorldManager.Instance.Worlds;

		foreach (var world in worlds)
		{
			var deploymentCard = ResourceLoader.Load<PackedScene>("res://UI/Deployment/DeploymentCard.tscn").Instantiate<DeploymentCard>();
			deploymentCard.World = world;

			_deploymentCardGrid.AddChild(deploymentCard);
		}
	}

	public override void _ExitTree()
	{
		GetTree().Paused = false;
	}
}