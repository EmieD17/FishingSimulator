using Godot;
using System;

public class PauseScreen : Control
{
    // Declare member variables here. Examples:
    // private int a = 2;
    // private string b = "text";

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        GetNode<TextureButton>("MarginContainer/CenterContainer/VBoxContainer/Resume").GrabFocus();
    }

    public override void _Input(InputEvent @event)
    {
        if(@event.IsActionPressed("ui_cancel"))
        {
            GetTree().Paused = !GetTree().Paused;
            Visible = !Visible;
        }
        
    }

    public void _on_Resume_pressed()
    {
        GetTree().Paused = !GetTree().Paused;
        Visible = !Visible;
    }

    public void _on_Quit_pressed()
    {
        PackedScene TitleScreen = GD.Load<PackedScene>("res://menu/TitleScreen.tscn");
        GetTree().Paused = false;
        GetTree().ChangeSceneTo(TitleScreen);        
    }



//  // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(float delta)
    {
        if(GetNode<TextureButton>("MarginContainer/CenterContainer/VBoxContainer/Resume").IsHovered())
        {
            GetNode<TextureButton>("MarginContainer/CenterContainer/VBoxContainer/Resume").GrabFocus();
        }
        if(GetNode<TextureButton>("MarginContainer/CenterContainer/VBoxContainer/Quit").IsHovered())
        {
            GetNode<TextureButton>("MarginContainer/CenterContainer/VBoxContainer/Quit").GrabFocus();
        }
    }
}
