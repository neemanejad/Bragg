using Godot;
using System;

public partial class SceneSwitcher : Node2D
{
	[Export]
	public string packedScenePath;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		if (packedScenePath == null)
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
		var instantiatedScene = ResourceLoader.Load<PackedScene>(packedScenePath).Instantiate();

		var root = GetTree().Root;

		foreach (Node child in root.GetChildren())
		{
			child.QueueFree();
		}

		root.CallDeferred(Node.MethodName.AddChild, instantiatedScene);
	}
}
