using Godot;

public partial class ContinueButton : Button
{
	public void Continue()
	{
		SceneSwitcher.Instance.GotoScene("res://Levels/Hub/Hub.tscn");
	}
}