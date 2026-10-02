using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGameLibrary.Graphics;
using System;

using sprint2.Bosses.States;

namespace sprint2.Bosses;

public class EyeOfCthulhu: IBoss
{
    public Vector2 Position { get; set; }
    public Vector2 Velocity {get; set;}
    public Vector2 Acceleration {get; set;}
    public Vector2 JerkDamping {get; set;}//drag force on acceleration
    public Vector2 Drag {get; set;}

    public Vector2 TargetPos {get;set;}

    private AnimatedSprite _sprite;

    private TextureAtlas _atlas;

    private IBossState _state;

    private uint movementSpeed = 12;

    private bool _facingRight = true;

    public void ChangeState(IBossState newState)
    {
        _state = newState;
        _state.Enter();
    }

    public EyeOfCthulhu(AnimatedSprite sprite, TextureAtlas atlas)
    {
        _sprite = sprite;
        _atlas = atlas;
        Position = new Vector2(1100, 600);
        Velocity = new Vector2(0, 0); 
        Drag = new Vector2(0.2f,0.2f);
        JerkDamping = new Vector2(0.002f,0.002f);

        ChangeState(new CthulhuPhase1State(this));
    }

     public void SetAnimation(string animationName)
    {
        _sprite.Animation = _atlas.GetAnimation(animationName);
    }


    public void Update(GameTime gameTime, Vector2 PlayerPos)
    {
        TargetPos = PlayerPos;
        _state.Update(gameTime);
        _sprite.Update(gameTime);
        
        
        HandleMovement();
    }

    public void HandleAttack(GameTime gameTime)
    {
            RunToPosition(TargetPos);
            DashToPosition(TargetPos);
    }

    public void HandleMovement()
    {
        
        
        Velocity += Acceleration;
        Position += Velocity; 
        
        

        Velocity = CalcDrag(Velocity, Drag);
        Acceleration = CalcDrag(Acceleration, JerkDamping);

        if (Velocity.X > 0) _facingRight = true;
        else if (Velocity.X < 0) _facingRight = false;

    }

    public Vector2 CalcDrag(Vector2 vel, Vector2 drag)
    {
        return new Vector2(HandleDragComponent(vel.X, drag.X), HandleDragComponent(vel.Y, drag.Y));
    }

    public float HandleDragComponent(float velComp, float dragComp)
    {
        if (Math.Abs(velComp) <= dragComp)
        {
            return 0;
        }

        if (velComp > 0)
        {
            velComp -= dragComp;
        }
        else
        {
            velComp += dragComp;
        }
        return velComp;
    }

    public void RunToPosition(Vector2 TargetPosition)
    {
        Vector2 direction = TargetPosition - Position;
        float epsilon = 0.03f;
        if (direction.Length() > epsilon)
        {
            direction.Normalize();

            float accelerationForce = 0.1f; 
            Acceleration = direction * accelerationForce;
        }
        else
        {
            Acceleration = Vector2.Zero;
        }
    }

    public void DashToPosition(Vector2 TargetPosition)
    {
        Vector2 direction = TargetPosition - Position;
        
        direction.Normalize(); 
        Velocity = direction * movementSpeed;
        
    }

    public void Draw(SpriteBatch spriteBatch)
    {
        SpriteEffects effect = _facingRight ? SpriteEffects.FlipHorizontally : SpriteEffects.None;
        _sprite.Draw(spriteBatch, Position, effect);
    }
}