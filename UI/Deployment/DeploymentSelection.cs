using Godot;

using Redwood.Singletons.SceneSwitcher;
using Redwood.Singletons.WorldManager;

namespace Redwood.UI.Deployment;

public partial class DeploymentSelection : UI
{
	private Button _button;

	private GridContainer _deploymentCardGrid;

	public override void _Ready()
	{
		ProcessMode = ProcessModeEnum.Always;

		_button = GetNode<Button>("CanvasLayer/Control/MarginContainer/PanelContainer/MarginContainer/VBoxContainer/HeaderContainer/HBoxContainer/Button");
		_button.Pressed += () => QueueFree();

		_deploymentCardGrid = GetNode<GridContainer>("CanvasLayer/Control/MarginContainer/PanelContainer/MarginContainer/VBoxContainer/DeploymentCardGrid");

		GenerateDeploymentCards();

		GetTree().Paused = true;
	}

	public override void _Process(double delta)
	{
		if (Input.IsActionJustPressed("escape"))
		{
			QueueFree();
		}
	}

	private void GenerateDeploymentCards()
	{
		var worlds = WorldManager.Instance.Worlds;

		foreach (var world in worlds)
		{
			var deploymentCard = ResourceLoader.Load<PackedScene>("res://UI/Deployment/DeploymentCard.tscn").Instantiate<DeploymentCard>();
			deploymentCard.World = world;
			deploymentCard.Connect(DeploymentCard.SignalName.DeploymentCardSelected, new Callable(this, MethodName.DeploymentCardSelected));

			_deploymentCardGrid.AddChild(deploymentCard);
		}
	}

	public void DeploymentCardSelected(DeploymentCard deploymentCard)
	{
		GameManager.Instance.Start();
		WorldManager.Instance.Select(deploymentCard.World);
		SceneSwitcher.Instance.GotoScene("res://World/World.tscn");
		QueueFree();
	}
}