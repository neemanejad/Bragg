using Godot;

using Redwood.Scripts.World;

using Redwood.Singletons.WorldManager;

namespace Redwood.World;

public partial class World : Node
{
	[Export]
	public PackedScene EnemyScene { get; set; }

	private WorldData _worldData;

	public override void _Ready()
	{
		GameManager.Instance.ResetSurvivalTimer();
		GameManager.Instance.ResumeSurvivalTimer();
		PlayerManager.Instance.ShowSurvivalTimer();
		PlayerManager.Instance.StartShooting();

		_worldData = WorldManager.Instance.SelectedWorld;
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{

	}
}