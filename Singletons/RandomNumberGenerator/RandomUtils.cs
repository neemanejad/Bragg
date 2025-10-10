using System;
using System.Collections.Generic;

using Godot;

namespace Redwood.Singletons.RandomUtils;

public partial class RandomUtils : Node
{
	public static RandomNumberGenerator RNG { get; private set; } = new();

	public override void _Ready()
	{
		RNG.Randomize();
	}

	[Obsolete("Prefer RandomNumberGenerator.RandWeighted")]
	public static T WeightedPick<T>(Dictionary<T, float> weights)
	{
		float sum = 0;
		foreach (var w in weights.Values)
		{
			sum += w;
		}

		var r = RNG.Randf() * sum;

		foreach (var pair in weights)
		{
			r -= pair.Value;
			if (r <= 0f) return pair.Key;
		}

		// Fallback
		foreach (var key in weights.Keys) return key;
		throw new InvalidOperationException("Empty dictionary.");
	}


	public static float RandRange(float min, float max)
	{
		return min + RNG.Randf() * (max - min);
	}
}