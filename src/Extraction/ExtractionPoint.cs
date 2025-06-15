using Godot;

public partial class ExtractionPoint : Node2D
{
	public void OnBodyEnteredExtractionPoint(Node2D body)
	{
		SceneSwitcher.Instance.GotoScene("res://src/Levels/Hub/Hub.tscn");
	}
}
