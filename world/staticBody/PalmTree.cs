using Godot;
using System;

public class PalmTree : StaticBody2D
{
    // Declare member variables here. Examples:
    // private int a = 2;
    // private string b = "text";

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        Random rand = new Random();

        if (rand.NextDouble() >= 0.5)
        {
            TileMap PalmTreeCoconutSprite = GetNode<TileMap>("PalmTreeCoconut");
            PalmTreeCoconutSprite.Visible = true;
        }   
    }

//  // Called every frame. 'delta' is the elapsed time since the previous frame.
//  public override void _Process(float delta)
//  {
//      
//  }
}
