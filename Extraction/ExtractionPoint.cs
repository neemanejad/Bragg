using Godot;

public partial class ExtractionPoint : Node2D
{
	public void OnBodyEnteredExtractionPoint(Node2D body)
	{
		if (body.IsInGroup("Player"))
		{
			SceneSwitcher.Instance.GotoScene("res://Levels/Hub/Hub.tscn");
		}
	}
}