using Godot;



public partial class PlayerManager : Node
{
	public static PlayerManager Instance { get; private set; }

	private float _health = 100f;

	private int _killCount;

	public override void _Ready()
	{
		Instance = this;
	}

	public void OnKillHandler()
	{
		_killCount += 1;
		GD.Print(_killCount);
	}

	public void AddHealth(float health)
	{
		var healthSum = health + _health;

		if (healthSum > 100f)
		{
			_health = 100f;
			return;
		}
		else if (healthSum <= 0f)
		{
			_health = 0;
			return;
		}

		_health = healthSum;
	}
}
