using System;

using Godot;

using Redwood.Scripts.World;
using Redwood.Singletons.RandomUtils;
using Redwood.Singletons.WorldManager;

namespace Redwood.World;

public partial class World : Node
{
	[Export]
	public ProceduralWorld ProceduralWorld;

	[Export]
	public WorldCamera WorldCamera;

	[Export]
	public WorldMap WorldMap;

	private WorldData _worldData;

	public override void _Ready()
	{
		GameManager.Instance.ResetSurvivalTimer();
		GameManager.Instance.ResumeSurvivalTimer();
		PlayerManager.Instance.ShowSurvivalTimer();
		PlayerManager.Instance.StartShooting();

		_worldData = WorldManager.Instance.SelectedWorld;
	}
}