using Godot;

public partial class ExtractionPoint : Node2D
{

	bool _isExtractable = false;

	public ExtractionPoint()
	{
	}

	public override void _Ready()
	{
		base._Ready();

		GameManager.Instance.LevelCleared += IsExtractable;

		setExtractableColor();
	}

	public override void _Process(double delta)
	{
		base._Process(delta);

		setExtractableColor();
	}

	public override void _ExitTree()
	{
		base._ExitTree();

		GameManager.Instance.LevelCleared -= IsExtractable;
	}

	public void OnBodyEnteredExtractionPoint(Node2D body)
	{
		if (_isExtractable && body.IsInGroup("Player"))
		{
			SceneSwitcher.Instance.GotoScene("res://Levels/Hub/Hub.tscn");
		}
	}

	public void IsExtractable()
	{
		_isExtractable = true;
	}

	private void setExtractableColor()
	{
		var extractionColorRect = GetNode<ColorRect>("ColorRect");

		extractionColorRect.Color = !_isExtractable ? new Color("red") : new Color("green");
	}
}