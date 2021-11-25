using Godot;
using System;
using System.Text;

public class FishShadow : KinematicBody2D
{
    const float SEPARATION_WEIGHT = 5f;
    const float ALIGNMENT_WEIGHT = 0.5f;
    const float COHESION_WEIGHT = 0.05f;

    Vector2 _direction = new Vector2(0, 1);
    int _separation_distance = 20;

    Godot.Collections.Array<FishShadow> _local_flockmates = new Godot.Collections.Array<FishShadow>();

    [Export]
    public float _maxSpeed = 1f;

    [Export]
    public float _speed = 0.5f;

    KinematicCollision2D collision;

    private Random _random = new Random();
    [Export]
    public Boolean isDebug = false;

    Label debugLabel;
    int spin_counter = 0;
    int spin_goal;


    public Boolean baited = false;
    public Vector2 baitPosition = new Vector2(0,1);
    public CollisionShape2D collisionDetectionRadius;
    public AnimationPlayer animationPlayer;
    public enum States{
        SWIMMING, 
        BAITED,
        HOOCKED
    }
    public States state = States.SWIMMING;



    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        
        animationPlayer = GetNode<AnimationPlayer>("AnimationPlayer");
        collisionDetectionRadius = GetNode<CollisionShape2D>("DetectionRadius/CollisionShape2D");
        //Random Animation
        /*if (_random.NextDouble() >= 0.5)
        {
            animationPlayer.Play("Spin");
        } */
/*
        fishSpawningTimer = GetNode<Timer>("FishSpawningTimer");
        fishSpawningTimer.WaitTime = _random.Next(2,8);
        fishSpawningTimer.Start();
*/
        float scaleNb = (float)(_random.NextDouble() * (2.5 - 1) + 1);
        Vector2 scale = new Vector2(scaleNb, scaleNb);
        this.Scale = scale;
        isDebug = false;
        
        var random = new RandomNumberGenerator();
        random.Randomize();

        _direction = new Vector2(random.Randfn(), random.Randfn());
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

    //Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(float delta)
    {
        switch(state)
        {
            case States.SWIMMING:
            {
                Swimming_State(delta);
                break;
            }
            case States.BAITED:
            {
                Baited_State(delta);
                break;
            }
            case States.HOOCKED:
            {
                Hoocked_State(delta);
                break;
            }
            default: break;
        }

        debug();
    }
    public void Swimming_State(float delta)
    {
        //Rotation = new Vector2(0,1).AngleToPoint(_direction);
        Rotation = _direction.Angle();
        var temp =  _direction * _speed;

        collision = MoveAndCollide(temp);
        if(collision != null){
            if(collision.Collider is TileMap){
                _direction = _collision_reaction_direction(collision);
            }
        }
        else{
            _direction = _flock_direction();
        }
    }
    public void Baited_State(float delta){
        _direction = Position.DirectionTo(baitPosition) * _speed * 50;
        Rotation = _direction.Angle();
        _direction = MoveAndSlide(_direction);
        if(Position.DistanceTo(baitPosition) < 25){
            animationPlayer.Play("Spin");
        } 
    }
    public void Hoocked_State(float delta){

    }
    public void Spin_Animation_Finished()
    {
        spin_counter++;
        if(spin_counter >= spin_goal){
            animationPlayer.Play("Idle");
            state = States.HOOCKED;
        }
       
       
    }

    // Inverts the direction when hitting a collider.
    // This implementation handles colliding with Tilemaps specifically.
    public Vector2 _collision_reaction_direction(KinematicCollision2D collision){
        return (collision.Position - collision.Normal).DirectionTo(this.Position);
    }
    
    public Vector2 _flock_direction(){
        var separation = new Vector2();
        var heading = _direction;
        var cohesion = new Vector2();

        foreach (FishShadow flockmate in _local_flockmates)
        {
            heading += flockmate.get_direction();
            cohesion += flockmate.Position;

            var distance = this.Position.DistanceTo(flockmate.Position);

            if(distance < _separation_distance){
              separation -= (flockmate.Position - this.Position).Normalized() * (_separation_distance / distance * _speed);
            }
        }

        if(_local_flockmates.Count > 0){
            heading /= _local_flockmates.Count;
            cohesion /= _local_flockmates.Count;
            var center_direction = Position.DirectionTo(cohesion);
            var circleShape2D = (CircleShape2D)collisionDetectionRadius.Shape;
            var center_speed = _maxSpeed * this.Position.DistanceTo(cohesion) / circleShape2D.Radius;
            cohesion = center_direction * center_speed;
        }
		
        return (
            _direction +
            separation * SEPARATION_WEIGHT +
            heading * ALIGNMENT_WEIGHT +
            cohesion * COHESION_WEIGHT
        ).Clamped(_maxSpeed);
    }

    public Vector2 get_direction(){
        return _direction;
    }
    public void set_direction(Vector2 direction){
        _direction = direction;
    }
    public void _on_DetectionRadius_body_entered(KinematicBody2D body){
        if(body == (KinematicBody2D)this)
		    return;

        if(body.IsInGroup("fish"))
            _local_flockmates.Add((FishShadow)body);
    }
    public void _on_DetectionRadius_body_exited(KinematicBody2D body){
        if(body.IsInGroup("fish"))
            _local_flockmates.Remove((FishShadow)body);
    }
}
