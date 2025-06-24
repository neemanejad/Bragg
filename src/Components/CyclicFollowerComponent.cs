using Godot;
using System;

public partial class CyclicFollowerComponent : Node2D
{
	[Export]
	public PackedScene EntityScene { get; set; }
	[Export]
	public Vector2 Anchor {  get; set; }
	[Export]
	public float Radius { get; set; }
	private Node2D Entity {  get; set; }
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		if (EntityScene == null)
		{
			throw new NullReferenceException($"EntityScene is null in CyclicFollowerComponent in {this.GetPath().ToString()}");
		}

		Entity = EntityScene.Instantiate<Node2D>();
		AddChild(Entity);

		Position = Anchor;
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		ProcessMouseMovement();
	}

	private void ProcessMouseMovement()
	{
        var directionVector = GetLocalMousePosition().Normalized() * Radius;
        Entity.Position = directionVector;
    }
}
