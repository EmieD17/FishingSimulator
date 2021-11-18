using Godot;
using System;
using System.Text;

public class FishShadow : KinematicBody2D
{
    const float SEPARATION_WEIGHT = 0.5f;
    const float ALIGNMENT_WEIGHT = 0.5f;
    const float COHESION_WEIGHT = 0.1f;

    Vector2 _velocity = Vector2.Zero;
    Vector2 _direction = new Vector2(0, 1);
    int _separation_distance = 20;

    Godot.Collections.Array fishArray;

    [Export]
    public float _maxSpeed = 1f;

    [Export]
    public float _speed = 0.5f;

    KinematicCollision2D collision;

    static private Random _random = new Random();
    [Export]
    public Boolean isDebug = true;

    Label debugLabel;

    private Timer fishSpawningTimer;
    [Export]
    public int nbFish;


    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        var animationPlayer = GetNode<AnimationPlayer>("AnimationPlayer");
        if (_random.NextDouble() >= 0.5)
        {
            animationPlayer.Play("Spin");
        } 

        fishSpawningTimer = GetNode<Timer>("FishSpawningTimer");
        fishSpawningTimer.WaitTime = _random.Next(2,8);
        fishSpawningTimer.Start();

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
        debugLabel.Text = this.Name;
    }

    //Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(float delta)
    {
        if(Visible)
        {
            Rotation = new Vector2(0,1).AngleToPoint(_direction);
            var temp =  _direction * _speed;

            collision = MoveAndCollide(temp);
            if(collision != null){
                if(collision.Collider is TileMap){
                    _direction = _collision_reaction_direction(collision);
                }
            }
            else{
                //_direction = _flock_direction();
            }

            debug();
        }
    }
    // Inverts the direction when hitting a collider.
    // This implementation handles colliding with Tilemaps specifically.
    public Vector2 _collision_reaction_direction(KinematicCollision2D collision){
        return (collision.Position - collision.Normal).DirectionTo(this.Position);
    }
    /*
    public Vector2 _flock_direction(){
        var separation = new Vector2();
        var heading = _direction;
        var cohesion = new Vector2();

    }*/

    public void _on_FishSpawningTimer_timeout()
    {
        fishSpawningTimer.WaitTime = _random.Next(2,7);
        // Node fishNode = this.GetParent();
        // fishArray = fishNode.GetChildren();
        // nbFish = fishArray.Count;
        // GD.Print(fishArray.Count);
        // int fishIdx = _random.Next(nbFish);

        // FishShadow theFish = (FishShadow)GetNode<Node2D>("YSort/Fish").GetChild(fishIdx);
        GD.Print(this.Name + " timed out");
        if(this.Visible == false)
        {
           
            GD.Print("the visible is false");
        }

        if(this.Visible == true)
        {
            this.Visible = false;
            GD.Print("delete fish (set to false)");
        }
        else{
            GD.Print("before visible");
            this.Visible = true;
            GD.Print("before create");
            //createFish(); 
            GD.Print("create fish (set to true)"); 
        }
    }
        public void createFish(){
        GD.Print("inside top create");

        // Choose a random location on Path2D.
        var fishSpawnLocation = GetParent().GetParent().GetParent().GetNode<PathFollow2D>("FishPath/FishSpawnLocation");
        fishSpawnLocation.Offset = _random.Next();


        // Set the mob's position to a random location.
        this.Position = fishSpawnLocation.Position;

        // Add some randomness to the direction.
        Vector2 direction = new Vector2(_random.Next(), _random.Next());


        _direction = direction;


    }
}
