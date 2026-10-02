using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGameLibrary.Graphics;
using sprint2.Bosses.States;
using static sprint2.Constants;
//temp
using MonoGameLibrary.Input;


namespace sprint2.Bosses;

public class QueenBee: IBoss
{
    public Vector2 Position { get; set; }
    public Vector2 Velocity {get; set;}
    public Vector2 Acceleration {get; set;}
    public Vector2 Drag {get; set;}
    public Vector2 TargetPos {get;set;}

    private AnimatedSprite _sprite;

    private TextureAtlas _atlas;
    private uint movementSpeed = 6;

    private IBossState _state;

    private bool _facingRight = true;

    public void ChangeState(IBossState newState)
    {
        _state = newState;
        _state.Enter();
    }

    public QueenBee(AnimatedSprite sprite, TextureAtlas atlas)
    {
        _sprite = sprite;
        _atlas = atlas;
        Position = new Vector2(1100, 400);
        Velocity = new Vector2(0, 0); 
        Drag = new Vector2(0.02f,0.02f);//constantly subtract or add from velocity towards 0 (acceleration vector acting against direction of movement)
        ChangeState(new QueenBeeIdleState(this));
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

    public void HandleMovement()
    {
        
        
        Velocity += Acceleration;
        Position += Velocity; 
        

        Velocity = CalcDrag(Velocity);

        if (Velocity.X > 0) _facingRight = true;
        else if (Velocity.X < 0) _facingRight = false;

    }

    public Vector2 CalcDrag(Vector2 vel)
    {
        return new Vector2(HandleDragComponent(vel.X, Drag.X), HandleDragComponent(vel.Y, Drag.Y));
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


    private float CalcDist(float target, float currPos)
    {
        float epsilon = 0.03f;
        float dist = target - currPos;

        if (Math.Abs(dist) >= epsilon)
        {
            return dist;
        }
        return 0;
    }
    public void RunToPosition(Vector2 TargetPosition)
    {
        Vector2 direction = TargetPosition - Position;
        float epsilon = 0.03f;
        if (direction.Length() > epsilon)
        {
            direction.Normalize();

            float accelerationForce = 0.5f; 
            Acceleration = direction * accelerationForce;
        }
        else
        {
            Acceleration = Vector2.Zero;
        }
    }

    public void Draw(SpriteBatch spriteBatch)
    {
        SpriteEffects effect = _facingRight ? SpriteEffects.FlipHorizontally : SpriteEffects.None;
        _sprite.Draw(spriteBatch, Position, effect);
    }
}