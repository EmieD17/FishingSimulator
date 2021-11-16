using Godot;
using System;

public class World : Node
{
    // Declare member variables here. Examples:
    // private int a = 2;
    // private string b = "text";
    [Export]
    public PackedScene Mob;

    [Export]
    public Godot.Collections.Array fishArray;

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
        Node2D fishNode = GetNode<Node2D>("YSort/Fish");
        fishArray = fishNode.GetChildren();
        GD.Print(fishNode.GetChildCount());
        GD.Print(fishArray.Count);
        GD.Print(fishArray[0]);

        if(fishArray.Count < 6)
        {
            createFish();
        }
        else{
            
            GetNode<Node2D>("YSort/Fish").GetChild(0).QueueFree();
        }

    }
    public void createFish(){
        // Choose a random location on Path2D.
            var fishSpawnLocation = GetNode<PathFollow2D>("FishPath/FishSpawnLocation");
            fishSpawnLocation.Offset = _random.Next();

            // Create a Mob instance and add it to the scene.
            FishShadow fishInstance = (FishShadow)fishScene.Instance();
            GetNode<Node2D>("YSort/Fish").AddChild(fishInstance);

            // Set the mob's position to a random location.
            fishInstance.Position = fishSpawnLocation.Position;

            // Add some randomness to the direction.
            /*Vector2 direction = new Vector2(_random.Next(), _random.Next());
            if(direction == new Vector2())
                direction = new Vector2(0, 1);
            fishInstance.set_direction(direction);
            */

            // Choose the velocity.
            //fishInstance.MoveAndCollide(new Vector2(RandRange(150f, 250f), 0).Rotated(direction));
    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(float delta)
    {
        if(Input.IsActionJustPressed("ui_cancel")) {
            GetTree().Quit();
        }
    }
}
