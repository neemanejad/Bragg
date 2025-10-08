using Godot;

using Redwood.Enemies;
using Redwood.Singletons.WorldManager;

namespace Redwood.Hub;

public partial class Hub : Node2D
{
	public override void _Ready()
	{
		GameManager.Instance.PauseSurvivalTimer();
		GameManager.Instance.ResetSurvivalTimer();
		PlayerManager.Instance.HideSurvivalTimer();
		PlayerManager.Instance.StopShooting();

		WorldManager.Instance.Generate(4);
		BountyManager.Instance.GenerateBounties(5);
	}
}