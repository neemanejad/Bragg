using Godot;

namespace Redwood.Scripts.World;

[GlobalClass]
public partial class WorldData : Resource
{
	[Export]
	public string Name { get; private set; }
	[Export]
	public Biome Biome { get; private set; }

	[Export]
	public Weather Weather { get; private set; }

	[Export]
	public TimeOfDay TimeOfDay { get; private set; }


	[Export]
	public float GravityScale { get; private set; }

	[Export]
	public float TemperatureC { get; private set; }

	[Export]
	public bool AtmosphereBreathable { get; private set; }

	[Export]
	public float VisibilityKm { get; private set; }

	public WorldData() { }

	public WorldData(string name, Biome biome, Weather weather, TimeOfDay timeOfDay, float gravityScale, float temperatureC, bool atmosphereBreathable, float visibilityKm)
	{
		Name = name;
		Biome = biome;
		Weather = weather;
		TimeOfDay = timeOfDay;

		GravityScale = gravityScale;
		TemperatureC = temperatureC;
		AtmosphereBreathable = atmosphereBreathable;
		VisibilityKm = visibilityKm;
	}

	public override string ToString()
	{
		return string.Format("Biome: {0}\nWeather {1}\nTime of day: {2}\nGravity: {3}\nTemperature: {4}\nAtmosphere: {5}\nVisibility: {6}\n", Biome, Weather, TimeOfDay, GravityScale, TemperatureC, AtmosphereBreathable, VisibilityKm);

	}
}