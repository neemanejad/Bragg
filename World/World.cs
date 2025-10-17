using System;

using Godot;

using Redwood.Scripts.World;
using Redwood.Singletons.RandomUtils;
using Redwood.Singletons.WorldManager;

namespace Redwood.World;

public partial class World : Node
{
	[Export]
	public ProcWorld ProcWorld;

	[Export]
	public WorldCamera WorldCamera;

	[Export]
	public WorldMap WorldMap;

	private WorldData _worldData;

	[Signal]
	public delegate void WorldReadyEventHandler(int terrainSeed);

	public override void _Ready()
	{
		GameManager.Instance.ResetSurvivalTimer();
		GameManager.Instance.ResumeSurvivalTimer();
		PlayerManager.Instance.ShowSurvivalTimer();
		PlayerManager.Instance.StartShooting();

		_worldData = WorldManager.Instance.SelectedWorld;

		var terrainSeed = (int)Math.Abs(RandomUtils.RNG.Randi());

		Connect(SignalName.WorldReady, new Callable(ProcWorld.ChunkGenerator, ChunkGenerator.MethodName.SetSeeds));
		Connect(SignalName.WorldReady, new Callable(WorldMap.Map, Map.MethodName.GenerateMap));

		EmitSignal(SignalName.WorldReady, terrainSeed);
	}
}