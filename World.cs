using Godot;
using System;

public class World : Node
{
    // Declare member variables here. Examples:
    // private int a = 2;
    // private string b = "text";
    [Export]
    public PackedScene Mob;

    private Random _random = new Random();
    private Timer fishSpawningTimer;
    PackedScene fishScene;


    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        fishSpawningTimer = GetNode<Timer>("FishSpawningTimer");
        fishSpawningTimer.Start();
        fishScene = ResourceLoader.Load<PackedScene>("res://actors/FishShadow.tscn");
        
    }

    // We'll use this later because C# doesn't support GDScript's randi().
    private float RandRange(float min, float max)
    {
        return (float)_random.NextDouble() * (max - min) + min;
    }

    public void _on_FishSpawningTimer_timeout()
    {
        // Choose a random location on Path2D.
        var fishSpawnLocation = GetNode<PathFollow2D>("FishPath/FishSpawnLocation");
        fishSpawnLocation.Offset = _random.Next();

        // Create a Mob instance and add it to the scene.
        KinematicBody2D fishInstance = (KinematicBody2D)fishScene.Instance();
        GetNode<Node2D>("YSort/Fish").AddChild(fishInstance);

        // Set the mob's direction perpendicular to the path direction.
        float direction = fishSpawnLocation.Rotation + Mathf.Pi / 2;

        // Set the mob's position to a random location.
        fishInstance.Position = fishSpawnLocation.Position;

        // Add some randomness to the direction.
        direction += RandRange(-Mathf.Pi / 4, Mathf.Pi / 4);
        fishInstance.Rotation = direction;

        // Choose the velocity.
        //fishInstance.LinearVelocity = new Vector2(RandRange(150f, 250f), 0).Rotated(direction);

    }

//  // Called every frame. 'delta' is the elapsed time since the previous frame.
//  public override void _Process(float delta)
//  {
//      
//  }
}
