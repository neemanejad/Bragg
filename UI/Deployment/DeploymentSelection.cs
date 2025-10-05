namespace Redwood.UI.Deployment;

using Godot;

using Singletons.WorldManager;

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
			var name = world.Name;
			var biome = world.Biome;
			var weather = world.Weather;
			var timeOfDay = world.TimeOfDay;
			var gravityScale = world.GravityScale;
			var temperatureC = world.TemperatureC;
			var atmosphereBreathable = world.AtmosphereBreathable;
			var visibilityKm = world.VisibilityKm;

			var builder = new DeploymentCardBuilder();

			var card = builder
				.SetName(name)
				.SetBiome(biome)
				.SetWeather(weather)
				.SetTimeOfDay(timeOfDay)
				.SetGravityScale(gravityScale)
				.SetTemperature(temperatureC)
				.SetAtmosphereBreathable(atmosphereBreathable)
				.SetWeather(weather)
				.SetVisibilityKm(visibilityKm)
				.GetDeploymentCard();

			_deploymentCardGrid.AddChild(card);
		}
	}

	public override void _ExitTree()
	{
		GetTree().Paused = false;
	}
}