using Godot;
using System;

public class Player : KinematicBody2D
{
    // Declare member variables here. Examples:
    // private int a = 2;
    // private string b = "text";
    const int ACCELERATION = 700;
    const int MAX_SPEED = 100;
    const int FRICTION = 500;
    Vector2 velocity = Vector2.Zero;

    AnimationPlayer animationPlayer;
    AnimationTree animationTree;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        animationPlayer = GetNode<AnimationPlayer>("AnimationPlayer");
    }

//  // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _PhysicsProcess(float delta)
    {
        Vector2 input_vector = Vector2.Zero;
        input_vector.x = Input.GetActionStrength("ui_right") - Input.GetActionStrength("ui_left");
        input_vector.y = Input.GetActionStrength("ui_down") - Input.GetActionStrength("ui_up");
        input_vector = input_vector.Normalized();

        if(input_vector != Vector2.Zero){
            if(input_vector.x > 0){
                animationPlayer.Play("RunRight");
            }else{
                animationPlayer.Play("RunLeft");
            }
            velocity = velocity.MoveToward(input_vector * MAX_SPEED, ACCELERATION * delta);
        }
        else{
            animationPlayer.Play("IdleRight");
            velocity = velocity.MoveToward(Vector2.Zero, FRICTION * delta);
        }

        velocity = MoveAndSlide(velocity);
    }
}
