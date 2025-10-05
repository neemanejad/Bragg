using Godot;

using Redwood.Singletons.SceneSwitcher;

namespace Redwood.Extraction;

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

		SetExtractableColor();
	}

	public override void _Process(double delta)
	{
		base._Process(delta);

		SetExtractableColor();
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
			SceneSwitcher.Instance.GotoScene("res://Hub/Hub.tscn");
		}
	}

	public void IsExtractable()
	{
		_isExtractable = true;
	}

	private void SetExtractableColor()
	{
		var extractionColorRect = GetNode<ColorRect>("ColorRect");

		extractionColorRect.Color = !_isExtractable ? new Color("red") : new Color("green");
	}
}