using Godot;
using System;

public class Floater : KinematicBody2D
{
    // Declare member variables here. Examples:
    // private int a = 2;
    // private string b = "text";
    Player player;
    RandomNumberGenerator random;
    public AnimationPlayer animationPlayer;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        player = (Player)GetTree().GetNodesInGroup("player")[0];
        animationPlayer = GetNode<AnimationPlayer>("AnimationPlayer");
        random = new RandomNumberGenerator();
        random.Randomize();
    }
    public void _on_DetectionZone_body_entered(KinematicBody2D body){
        if(player.theFish == null){
            if(body.IsInGroup("fish")){
                FishShadow fish = (FishShadow)body;
                fish.spin_goal = random.RandiRange(1,2);

                GD.Print("spin_goal:" + fish.spin_goal);
                fish.state = FishShadow.States.BAITED;
                
                fish.baitPosition = GlobalPosition;
                fish.collisionDetectionRadius.Disabled = true;
                player.theFish = fish;

                GD.Print("the floater got a fish ( )< !");
            }
        }
    }

//  // Called every frame. 'delta' is the elapsed time since the previous frame.
//  public override void _Process(float delta)
//  {
//      
//  }
}
