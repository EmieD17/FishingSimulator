using Godot;
using System;

public class PauseScreen : Control
{
    VBoxContainer menu;
    VBoxContainer controls;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        GetNode<TextureButton>("MarginContainer/CenterContainer/VBoxContainer/Resume").GrabFocus();
        menu = GetNode<VBoxContainer>("MarginContainer/CenterContainer/VBoxContainer");
        controls = GetNode<VBoxContainer>("MarginContainer/CenterContainer/Controls");
    }

    public override void _Input(InputEvent @event)
    {
        if(@event.IsActionPressed("ui_cancel")){
            GetTree().Paused = !GetTree().Paused;
            Visible = !Visible;
        }
    }
    public void _on_Resume_pressed(){
        GetTree().Paused = !GetTree().Paused;
        Visible = !Visible;
    }

    public void _on_Quit_pressed(){
        PackedScene TitleScreen = GD.Load<PackedScene>("res://menu/TitleScreen.tscn");
        GetTree().Paused = false;
        GetTree().ChangeSceneTo(TitleScreen);        
    }
    public void _on_Controls_pressed(){
        menu.Visible = false;
        controls.Visible = true;
    }
    public void _on_Quit2_pressed(){
        menu.Visible = true;
        controls.Visible = false;
    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
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
        if(GetNode<TextureButton>("MarginContainer/CenterContainer/VBoxContainer/Controls").IsHovered())
        {
            GetNode<TextureButton>("MarginContainer/CenterContainer/VBoxContainer/Controls").GrabFocus();
        }
    }
}
