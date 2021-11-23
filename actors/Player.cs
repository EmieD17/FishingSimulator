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

    enum States{
        MOVE, 
        FISHING
    }

    States state = States.MOVE;
    Vector2 velocity = Vector2.Zero;

    AnimationPlayer animationPlayer;
    AnimationTree animationTree;
    AnimationNodeStateMachinePlayback animationState;
    Sprite spriteWalk;
    Sprite spriteFishing;
    Sprite spriteRode;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        animationPlayer = GetNode<AnimationPlayer>("AnimationPlayer");
        animationTree = GetNode<AnimationTree>("AnimationTree");
        animationTree.Active = true;
        animationState = (AnimationNodeStateMachinePlayback)animationTree.Get("parameters/playback");

        spriteWalk = GetNode<Sprite>("SpriteWalk");
        spriteFishing = GetNode<Sprite>("SpriteFishing");
        spriteRode = GetNode<Sprite>("SpriteFishingRode");
    }

//  // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _PhysicsProcess(float delta)
    {
        switch(state)
        {
            case States.MOVE:
            {
                Move_State(delta);
                break;
            }
            case States.FISHING:
            {
                Fishing_State(delta);
                break;
            }
            default: break;
        }

       
    }

    public void Move_State(float delta)
    {
        spriteWalk.Visible = true;
        spriteFishing.Visible = false;
        spriteRode.Visible = false;

        Vector2 input_vector = Vector2.Zero;
        input_vector.x = Input.GetActionStrength("ui_right") - Input.GetActionStrength("ui_left");
        input_vector.y = Input.GetActionStrength("ui_down") - Input.GetActionStrength("ui_up");
        input_vector = input_vector.Normalized();

        if(input_vector != Vector2.Zero){
            animationTree.Set("parameters/Idle/blend_position", input_vector);
            animationTree.Set("parameters/Run/blend_position", input_vector);
            animationTree.Set("parameters/Fishing/blend_position", input_vector);
            animationState.Travel("Run");
            velocity = velocity.MoveToward(input_vector * MAX_SPEED, ACCELERATION * delta);
        }
        else{
            //animationPlayer.Play("IdleRight");
            animationState.Travel("Idle");
            velocity = velocity.MoveToward(Vector2.Zero, FRICTION * delta);
        }

        velocity = MoveAndSlide(velocity);

        if(Input.IsActionJustPressed("fish")){
            state = States.FISHING;

            velocity = Vector2.Zero;
            spriteWalk.Visible = false;
            spriteFishing.Visible = true;
            spriteRode.Visible = true;
            
            animationState.Travel("Fishing");
            animationTree.Active = false;
        }

    }
    public void Fishing_State(float delta)
    {
        

        if(Input.IsActionJustPressed("fish")){
            //animationState.Travel("Fishing");
            animationTree.Active = true;

        }
        
        if(Input.IsActionJustPressed("ui_right")|Input.IsActionJustPressed("ui_down")|Input.IsActionJustPressed("ui_up")|Input.IsActionJustPressed("ui_left")){
        
            state = States.MOVE;

            velocity = Vector2.Zero;
            spriteWalk.Visible = true;
            spriteFishing.Visible = false;
            spriteRode.Visible = false;
        }
    }

    public void ShootRode_Animation_Finished()
    {
        state = States.MOVE;
    }
}
