using Godot;
using System;

public class Menu : Control
{
    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        GetNode<Button>("VBoxContainer/StartButton").GrabFocus(); 
    }
    public void onStartButtonPressed() {
        GetTree().ChangeScene("res://World.tscn");
    }
    public void _on_OptionsButton_pressed(){
        var options = GD.Load<PackedScene>("res://OptionsMenu.tscn").Instance();
        GetTree().CurrentScene.GetParent().AddChild(options);
    }
    public void _on_QuitButton_pressed(){
        GetTree().Quit();
    }
}
