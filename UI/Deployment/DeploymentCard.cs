using Godot;

using Redwood.Scripts.World;
using Redwood.Singletons.SceneSwitcher;
using Redwood.Singletons.WorldManager;

namespace Redwood.UI.Deployment;

public partial class DeploymentCard : PanelContainer
{
	[Signal]
	public delegate void DeploymentCardSelectedEventHandler(DeploymentCard deploymentCard);
	public WorldData World { get; set; }

	public override void _Ready()
	{
		var nameValue = GetNode<Label>("MarginContainer/VBoxContainer/NameLabel");
		nameValue.Text = World.Name;

		var biomeValue = GetNode<Label>("MarginContainer/VBoxContainer/PillContainer/TempPill/TempPillValue");
		biomeValue.Text = World.Biome.ToString();

		var timeValue = GetNode<Label>("MarginContainer/VBoxContainer/GridContainer/TimeValue");
		timeValue.Text = World.TimeOfDay.ToString();

		var weatherValue = GetNode<Label>("MarginContainer/VBoxContainer/GridContainer/WeatherValue");
		weatherValue.Text = World.Weather.ToString();

		var gravityValue = GetNode<Label>("MarginContainer/VBoxContainer/GridContainer/GravityValue");
		gravityValue.Text = World.GravityScale.ToString();

		var tempValue = GetNode<Label>("MarginContainer/VBoxContainer/GridContainer/TempValue");
		tempValue.Text = World.TemperatureC.ToString();

		var atmoValue = GetNode<Label>("MarginContainer/VBoxContainer/GridContainer/AtmoValue");
		atmoValue.Text = World.AtmosphereBreathable.ToString();

		var deployButton = GetNode<Button>("MarginContainer/VBoxContainer/DeployButton");
		deployButton.Pressed += Deploy;
	}

	private void Deploy()
	{
		EmitSignal(SignalName.DeploymentCardSelected, this);
	}
}