using Godot;

public partial class StartGame : CanvasLayer
{
	public void OnBeginPressed()
	{
		SceneSwitcher.Instance.GotoScene("res://src/Levels/Main/Main.tscn");
	}
}
