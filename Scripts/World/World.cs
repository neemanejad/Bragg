namespace Redwood.Scripts.World;

public sealed class World
{
	public string Name { get; }
	public Biome Biome { get; }
	public Weather Weather { get; }
	public TimeOfDay TimeOfDay { get; }

	public float GravityScale { get; }
	public float TemperatureC { get; }
	public bool AtmosphereBreathable { get; }
	public float VisibilityKm { get; }
	public World(string name, Biome biome, Weather weather, TimeOfDay timeOfDay, float gravityScale, float temperatureC, bool atmosphereBreathable, float visibilityKm)
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