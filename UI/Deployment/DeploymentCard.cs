namespace Redwood.UI.Deployment;

using Godot;

using Redwood.Scripts.World;

public partial class DeploymentCard : PanelContainer
{
	public string WorldName { get; set; }
	public Biome Biome { get; set; }
	public TimeOfDay TimeOfDay { get; set; }
	public Weather Weather { get; set; }
	public float GravityScale { get; set; }
	public float Temperature { get; set; }
	public float VisibilityKm { get; set; }
	public bool AtmosphereBreathable { get; set; }

	public override void _Ready()
	{
		var nameValue = GetNode<Label>("MarginContainer/VBoxContainer/NameLabel");
		nameValue.Text = WorldName;

		var biomeValue = GetNode<Label>("MarginContainer/VBoxContainer/PillContainer/TempPill/TempPillValue");
		biomeValue.Text = Biome.ToString();

		var timeValue = GetNode<Label>("MarginContainer/VBoxContainer/GridContainer/TimeValue");
		timeValue.Text = TimeOfDay.ToString();

		var weatherValue = GetNode<Label>("MarginContainer/VBoxContainer/GridContainer/WeatherValue");
		weatherValue.Text = Weather.ToString();

		var gravityValue = GetNode<Label>("MarginContainer/VBoxContainer/GridContainer/GravityValue");
		gravityValue.Text = GravityScale.ToString();

		var tempValue = GetNode<Label>("MarginContainer/VBoxContainer/GridContainer/TempValue");
		tempValue.Text = Temperature.ToString();

		var atmoValue = GetNode<Label>("MarginContainer/VBoxContainer/GridContainer/AtmoValue");
		atmoValue.Text = AtmosphereBreathable.ToString();

		var deployButton = GetNode<Button>("MarginContainer/VBoxContainer/DeployButton");
		deployButton.Pressed += Deploy;
	}

	private void Deploy()
	{
		GameManager.Instance.Start();
		SceneSwitcher.Instance.GotoScene("res://Levels/Main/Main.tscn");
	}
}