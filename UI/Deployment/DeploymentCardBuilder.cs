namespace Redwood.UI.Deployment;

using Godot;

using Redwood.Scripts.World;

public sealed class DeploymentCardBuilder
{
	private DeploymentCard _deploymentCard;

	public DeploymentCardBuilder()
	{
		Reset();
	}

	public DeploymentCardBuilder SetName(string name)
	{
		_deploymentCard.WorldName = name;

		return this;
	}

	public DeploymentCardBuilder SetBiome(Biome biome)
	{
		_deploymentCard.Biome = biome;

		return this;
	}

	public DeploymentCardBuilder SetTimeOfDay(TimeOfDay timeOfDay)
	{
		_deploymentCard.TimeOfDay = timeOfDay;

		return this;
	}

	public DeploymentCardBuilder SetWeather(Weather weather)
	{
		_deploymentCard.Weather = weather;

		return this;
	}

	public DeploymentCardBuilder SetGravityScale(float gravityScale)
	{
		_deploymentCard.GravityScale = gravityScale;

		return this;
	}

	public DeploymentCardBuilder SetTemperature(float temperature)
	{
		_deploymentCard.Temperature = temperature;

		return this;
	}

	public DeploymentCardBuilder SetAtmosphereBreathable(bool atmosphereBreathable)
	{
		_deploymentCard.AtmosphereBreathable = atmosphereBreathable;

		return this;
	}

	public DeploymentCardBuilder SetVisibilityKm(float visibilityKm)
	{
		_deploymentCard.VisibilityKm = visibilityKm;

		return this;
	}

	public DeploymentCard GetDeploymentCard()
	{
		var result = _deploymentCard;

		Reset();

		return result;
	}

	public void Reset()
	{
		_deploymentCard = ResourceLoader.Load<PackedScene>("res://UI/Deployment/DeploymentCard.tscn").Instantiate<DeploymentCard>();
	}
}