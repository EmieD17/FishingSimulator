using Godot;
using System;

public class World : Node
{
    // Declare member variables here. Examples:
    // private int a = 2;
    // private string b = "text";
 
    RandomNumberGenerator random = new RandomNumberGenerator();
    private Timer fishSpawningTimer;
    PackedScene fishScene;

    public Godot.Collections.Array fishArray;

    Player player;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        fishSpawningTimer = GetNode<Timer>("FishSpawningTimer");
        fishSpawningTimer.Start();
        fishScene = ResourceLoader.Load<PackedScene>("res://actors/FishShadow.tscn");
        
        player = GetNode<Player>("YSort/Player");

        random.Randomize();
        
    }
    public void _on_FishSpawningTimer_timeout()
    {
        Node2D fishNode = GetNode<Node2D>("Fish");
        fishArray = fishNode.GetChildren();

        if(fishArray.Count < 20)
        {
            createFish();
        }
        else{
            //GD.Print("Erase!");
            FishShadow fishToErase = (FishShadow)GetNode<Node2D>("Fish").GetChild(0);
            if(fishToErase.state == FishShadow.States.SWIMMING){
                fishToErase.QueueFree();
            }
            
        }
    }

    
    public void createFish(){
        //GD.Print("create!");

        // Choose a random location on Path2D.
        var fishSpawnLocation = GetNode<PathFollow2D>("FishPath/FishSpawnLocation");
        fishSpawnLocation.Offset = random.Randi();

        FishShadow fishInstance = (FishShadow)fishScene.Instance();

        // Set the mob's position to a random location.
        fishInstance.Position = fishSpawnLocation.Position;

        GetNode<Node2D>("Fish").AddChild(fishInstance);
    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(float delta)
    { 
        
        /*
        if(Input.IsActionJustPressed("ui_cancel")) {
            GetTree().Quit();
        }*/
    }
    
}
