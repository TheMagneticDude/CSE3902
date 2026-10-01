using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGameLibrary.Graphics;
using sprint2.Bosses.States;
using static sprint2.Constants;


namespace sprint2.Bosses;

public class QueenBee: IBoss
{
    public Vector2 Location { get; set; }
    public Vector2 Velocity {get; set;}
    public Vector2 Acceleration {get; set;}
    public Vector2 Drag {get; set;}

    private AnimatedSprite _sprite;

    private TextureAtlas _atlas;
    private uint movementSpeed = 6;

    private IBossState _state;

    public void ChangeState(IBossState newState)
    {
        _state = newState;
        _state.Enter();
    }

    public QueenBee(AnimatedSprite sprite, TextureAtlas atlas)
    {
        _sprite = sprite;
        _atlas = atlas;
        Location = new Vector2(1100, 400);
        Velocity = new Vector2(movementSpeed, 0);  //move right on init
        Drag = new Vector2(2f,2f);//constantly subtract or add from velocity towards 0 
        ChangeState(new QueenBeeIdleState(this));
    }

    public void SetAnimation(string animationName)
    {
        _sprite.Animation = _atlas.GetAnimation(animationName);
    }

    public void Update(GameTime gameTime)
    {
        _state.Update(gameTime);
        _sprite.Update(gameTime);

        
    }

    public void HandleMovement()
    {
        Location += Velocity;

        Velocity = CalcDrag(Velocity);



        if (Location.X >= 1280)
        {
            Velocity = new Vector2(-movementSpeed,0);
            _sprite.Effects = SpriteEffects.None;
        }
        if (Location.X <= 0)
        {
            Velocity = new Vector2(movementSpeed, 0);
            _sprite.Effects = SpriteEffects.FlipHorizontally;
        }

    }

    public Vector2 CalcDrag(Vector2 vel)
    {
        return new Vector2(HandleDragComponent(vel.X, Drag.X), HandleDragComponent(vel.Y, Drag.Y));
    }

    public float HandleDragComponent(float velComp, float dragComp)
    {
        if (Math.Abs(velComp) <= dragComp)
        {
            velComp = 0;
        }

        if (velComp > 0)
        {//+
            velComp -= dragComp;
        }
        else
        {//-
            velComp += dragComp;
        }
        return velComp;
    }

    public void Draw(SpriteBatch spriteBatch)
    {
        _sprite.Draw(spriteBatch, Location);
    }
}