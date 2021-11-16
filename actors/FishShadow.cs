using Godot;
using System;

public class FishShadow : KinematicBody2D
{
    Vector2 velocity = Vector2.Zero;

    [Export]
    public int MinSpeed = 150; // Minimum speed range.

    [Export]
    public int MaxSpeed = 250; // Maximum speed range.
    static private Random _random = new Random();
    [Export]
    public Boolean isDebug = true;

    Label debugLabel;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        var animationPlayer = GetNode<AnimationPlayer>("AnimationPlayer");
        if (_random.NextDouble() >= 0.5)
        {
            animationPlayer.Play("Spin");
        } 

        float scaleNb = (float)(_random.NextDouble() * (2.5 - 1) + 1);
        Vector2 scale = new Vector2(scaleNb, scaleNb);
        this.Scale = scale;
        isDebug = true;
        

    }
    public void debug(){
        if(!isDebug){
            return ;
        }
        if(debugLabel == null){
            debugLabel = GetNode<Label>("debugLabel");
        }
        debugLabel.Text = Position.ToString();
    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(float delta)
    {
        debug();
    }
}
