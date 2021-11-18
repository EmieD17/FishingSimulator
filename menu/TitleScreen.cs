using Godot;
using System;

public class TitleScreen : Node2D
{
    // Declare member variables here. Examples:
    // private int a = 2;
    // private string b = "text";

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        
    }

    public void _on_Start_pressed()
    {
        GetTree().ChangeScene("res://World.tscn");
    }
    public void _on_Quit_pressed()
    {
        GetTree().Quit();
    }

//  // Called every frame. 'delta' is the elapsed time since the previous frame.
//  public override void _Process(float delta)
//  {
//      
//  }
}
