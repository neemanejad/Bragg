using Godot;

public partial class SceneSwitcher : Node
{
	public static SceneSwitcher Instance { get; private set; }

	public Node CurrentScene { get; set; }

	public override void _Ready()
	{
		Instance = this;

		Viewport root = GetTree().Root;
		// last child node of `root`.
		CurrentScene = root.GetChild(-1);

		GD.Print(Instance);
	}

	public void GotoScene(string path)
	{
		// This function will usually be called from a signal callback,
		// or some other function from the current scene.
		// Deleting the current scene at this point is
		// a bad idea, because it may still be executing code.
		// This will result in a crash or unexpected behavior.

		// The solution is to defer the load to a later time, when
		// we can be sure that no code from the current scene is running:

		CallDeferred(MethodName.DeferredGotoScene, path);
	}

	public void DeferredGotoScene(string path)
	{
		CurrentScene.Free();

		var nextScene = ResourceLoader.Load<PackedScene>(path);
		GD.Print(nextScene);

		CurrentScene = nextScene.Instantiate();
		GetTree().Root.AddChild(CurrentScene);
		GetTree().CurrentScene = CurrentScene;
	}
}
