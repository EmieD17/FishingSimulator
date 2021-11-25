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

    public enum States{
        MOVE, 
        FISH,
        FISHING,
        RIM
    }

    public States state = States.MOVE;
    Vector2 velocity = Vector2.Zero;

    AnimationPlayer animationPlayer;
    AnimationTree animationTree;
    AnimationNodeStateMachinePlayback animationState;
    Sprite spriteWalk;
    Sprite spriteFishing;
    Sprite spriteRode;
    Node2D circleMouse;
    PackedScene floaterScene;
    public Floater floater;
    public FishShadow theFish;

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

        circleMouse = GetNode<Node2D>("CanvasLayer/CircleMouse");
        floaterScene = ResourceLoader.Load<PackedScene>("res://actors/Floater.tscn");
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
            case States.FISH:
            {
                Fish_State(delta);
                break;
            }
            case States.FISHING:
            {
                Fishing_State(delta);
                break;
            }
            case States.RIM:
            {
                Rim_State(delta);
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
            animationTree.Set("parameters/StartFishing/blend_position", input_vector);
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
            state = States.FISH;

            velocity = Vector2.Zero;
            spriteWalk.Visible = false;
            spriteFishing.Visible = true;
            spriteRode.Visible = true;
            circleMouse.Visible = true;
            
            animationState.Travel("StartFishing");
            GD.Print("let's fish");
        }

    }
    public void Fish_State(float delta)
    {
        
        if(Input.IsActionJustPressed("left_click")){
            animationState.Travel("Fishing");
            GD.Print("I'm fishing!");
            state = States.FISHING;
            circleMouse.Visible = false;
            /*floater.Position = circleMouse.Position;
            floater.Visible = true;*/

            floater = (Floater)floaterScene.Instance();
            var position = circleMouse.Position - (GetViewport().GetVisibleRect().Size / 2) + GetNode<Camera2D>("Camera2D").GlobalPosition;;
            floater.Position = position;
            GetNode<Node>("Rode").AddChild(floater);  
        }
        
        if(Input.IsActionJustPressed("fish")|Input.IsActionJustPressed("ui_right")|Input.IsActionJustPressed("ui_down")|Input.IsActionJustPressed("ui_up")|Input.IsActionJustPressed("ui_left")){
        
            state = States.MOVE;
            GD.Print("Stop fishing :(");
            animationState.Travel("Run");

            velocity = Vector2.Zero;
            spriteWalk.Visible = true;
            spriteFishing.Visible = false;
            spriteRode.Visible = false;
            circleMouse.Visible = false;
        }
    }
    public void Fishing_State(float delta)
    {
        if(theFish != null && theFish.state == FishShadow.States.HOOCKED){
            floater.animationPlayer.Play("Hoocked");
        }
        else{
            floater.animationPlayer.Play("Float");
        }
        
        if(Input.IsActionJustPressed("fish")){
            if(theFish != null){
                //if(theFish.state == FishShadow.States.HOOCKED){
                    //start to rim
                    //state = States.RIM;
                    //GD.Print("ready to rim!");
                //}else{       
                    //release the fish & floater
                    animationState.Travel("StartFishing");
                    GD.Print("Stop fishing!");
                    state = States.FISH;
                    if(GetNode<Floater>("Rode/Floater") != null){
                        GetNode<Floater>("Rode/Floater").QueueFree();
                    }
                    circleMouse.Visible = true;
                    theFish.SetFree();
                //}
            }
            else{
                //release the floater
                animationState.Travel("StartFishing");
                    GD.Print("Stop fishing!");
                    state = States.FISH;
                    if(GetNode<Floater>("Rode/Floater") != null){
                        GetNode<Floater>("Rode/Floater").QueueFree();
                    }
                    circleMouse.Visible = true;
            }
        } 
    }
    public void Rim_State(float delta){


    }

    public void ShootRode_Animation_Finished()
    {
       // GD.Print("shot the floater!");
        //floater.Visible = true;
        //GetNode<Node2D>("Rode").AddChild(floater); 
    }

    public override void _Input(InputEvent @event)
    {
        if(@event is InputEventMouseMotion mouse)
        {            
            circleMouse.Position = mouse.GlobalPosition;
        }
        
    }
}
