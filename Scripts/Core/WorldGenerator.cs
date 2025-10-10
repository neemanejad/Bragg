
using System;
using System.Collections.Generic;

using Godot;

using Redwood.Scripts.World;
using Redwood.Singletons.RandomUtils;

namespace Redwood.Scripts.Core;

public partial class WorldGenerator
{
	private readonly RandomNumberGenerator _rng = new();

	public WorldGenerator()
	{
		_rng.Randomize();
	}

	public WorldData GenerateWorld(int? seed = null)
	{
		if (seed.HasValue)
		{
			_rng.Seed = (ulong)seed.Value;
		}

		var name = GenerateName();
		var biome = PickBiome();
		var weather = PickWeatherFor(biome);
		var timeOfDay = PickEnum<TimeOfDay>();
		var gravityScale = _rng.Randf();
		var temperatureC = SampleTemperatureFor(biome, timeOfDay, weather);

		// Randomize generation for breathable atmposphere and visibility
		var world = new WorldData(name, biome, weather, timeOfDay, gravityScale, temperatureC, true, 100f);

		// Debugging
		// GD.Print(world);

		return world;
	}

	private string GenerateName()
	{
		var rand = new Random((int)_rng.Randi());
		var guid = new byte[16];
		rand.NextBytes(guid);

		return new Guid(guid).ToString().Substr(0, 5);
	}

	private float SampleTemperatureFor(Biome biome, TimeOfDay timeOfDay, Weather weather)
	{
		(float min, float max) range = biome switch
		{
			Biome.Barren => (10f, 30f),
			Biome.Desert => (30f, 45f),
			Biome.Jungle => (25f, 35f),
			Biome.Volcanic => (50f, 100f),
			Biome.Plains => (20f, 30f),
			_ => (15f, 30f),
		};

		var temp = RandomUtils.RandRange(range.min, range.max);

		temp += weather switch
		{
			Weather.Clear => 1f,
			Weather.Fog => -3f,
			Weather.Rain => -2f,
			Weather.Storm => -1f,
			_ => 0f,
		};

		temp += timeOfDay switch
		{
			TimeOfDay.Dawn => -1f,
			TimeOfDay.Day => 2f,
			TimeOfDay.Dusk => -1f,
			TimeOfDay.Night => -3f,
			_ => 0f,
		};

		return temp;
	}

	private Weather PickWeatherFor(Biome biome)
	{
		Dictionary<Weather, float> weights = biome switch
		{
			Biome.Barren => new() { { Weather.Clear, 1.0f }, { Weather.Fog, 0f }, { Weather.Rain, 0.3f }, { Weather.Storm, 0.4f } },
			Biome.Desert => new() { { Weather.Clear, 1.5f }, { Weather.Fog, 0.1f }, { Weather.Rain, 0.2f }, { Weather.Storm, 0.8f } },
			Biome.Jungle => new() { { Weather.Clear, 0.8f }, { Weather.Fog, 0.6f }, { Weather.Rain, 1.4f }, { Weather.Storm, 0.4f } },
			Biome.Plains => new() { { Weather.Clear, 1.0f }, { Weather.Fog, 0.3f }, { Weather.Rain, 0.4f }, { Weather.Storm, 0.2f } },
			Biome.Volcanic => new() { { Weather.Clear, 1.0f }, { Weather.Fog, 0f }, { Weather.Rain, 0f }, { Weather.Storm, 0.3f } },
			_ => new() { { Weather.Clear, 1f } }
		};

		return RandomUtils.WeightedPick(weights);
	}

	private Biome PickBiome()
	{
		var weights = new Dictionary<Biome, float>
		{
			{Biome.Barren, 0.2f},
			{Biome.Desert, 0.6f},
			{Biome.Jungle, 0.7f},
			{Biome.Plains, 1.0f},
			{Biome.Volcanic, 0.5f},
		};

		return RandomUtils.WeightedPick(weights);
	}

	private T PickEnum<T>() where T : Enum
	{
		var values = (T[])Enum.GetValues(typeof(T));
		var idx = _rng.RandiRange(0, values.Length - 1);
		return values[idx];
	}
}