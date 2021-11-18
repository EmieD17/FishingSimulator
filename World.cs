using Godot;
using System;

public class World : Node
{
    // Declare member variables here. Examples:
    // private int a = 2;
    // private string b = "text";



    private Random _random = new Random();
   


    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        
  
        
    }

    
//     public void createFish(FishShadow theFish){
//         GD.Print("inside top create");

//         // Choose a random location on Path2D.
//         var fishSpawnLocation = GetNode<PathFollow2D>("FishPath/FishSpawnLocation");
//         fishSpawnLocation.Offset = _random.Next();
//          GD.Print("inside top create 2");

//         // Set the mob's position to a random location.
//         theFish.Position = fishSpawnLocation.Position;

//  GD.Print("inside top create 3");
//         // Add some randomness to the direction.
//         Vector2 direction = new Vector2(_random.Next(), _random.Next());

//  GD.Print("inside top create 4");
//         GD.Print("direction world" + direction);
//         theFish.setDirection(direction);
//          GD.Print("inside top create 5");

//     }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(float delta)
    {
        /*
        if(Input.IsActionJustPressed("ui_cancel")) {
            GetTree().Quit();
        }*/
    }
}
