using Godot;
using System;

public partial class SceneSwitcher : Node2D
{
	[Export]
	public PackedScene packedScene;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		if (packedScene == null)
		{
			throw new NullReferenceException("Missing packed scene.");
		}
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	public void SwitchScene()
	{
		var instantiatedScene = packedScene.Instantiate();

		var root = GetTree().Root;

		foreach (Node child in root.GetChildren())
		{
			child.QueueFree();
		}

		root.AddChild(instantiatedScene);
	}
}
