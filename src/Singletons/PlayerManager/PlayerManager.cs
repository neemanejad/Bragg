using Godot;



public partial class PlayerManager : Node
{
	public static PlayerManager Instance { get; private set; }

	[Export]
	public PackedScene PlayerScene;

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
}
