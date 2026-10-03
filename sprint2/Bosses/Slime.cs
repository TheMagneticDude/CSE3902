using System.Reflection.Metadata;
using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGameLibrary.Graphics;
using static sprint2.Constants;


namespace sprint2.Bosses;

public class Slime: IBoss
{
    public Vector2 Position { get; set; }
    public Vector2 Velocity { get; set; }
    public Vector2 Drag {get; set;}
    public bool IsGrounded { get; private set; }
    public Vector2 TargetPos {get;set;}

    private const float _jumpStrength = -10f;
    private float _timer;


    private AnimatedSprite _sprite;

    private TextureAtlas _atlas;
        private uint _movementSpeed = 5;


    public Slime(AnimatedSprite sprite, TextureAtlas atlas)
    {
        _sprite = sprite;
        _atlas = atlas;
        Position = new Vector2(200, 80);
        Drag = new Vector2(2f,0f);
        Velocity = new Vector2(0, 0); 
    }

    public void SetAnimation(string animationName)
    {
        _sprite.Animation = _atlas.GetAnimation(animationName);
    }

    public void Update(GameTime gameTime, Vector2 PlayerPos)
    {
        TargetPos = PlayerPos;
        _sprite.Update(gameTime);
        HandleMovement();
        ApplyPhysics();

        HandleJump(gameTime);
    }

    public void HandleMovement()
    {
        Position += Velocity; 
        Velocity = CalcDrag(Velocity, Drag);
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

    private void ApplyPhysics()
    {
        Velocity = new Vector2(Velocity.X, Velocity.Y + Gravity);
        Position += Velocity;

        if (Position.Y >= GroundLevel)
        {
            Position = new Vector2(Position.X, GroundLevel);
            Velocity = new Vector2(Velocity.X, 0f);
            IsGrounded = true;
        }
        else
        {
            IsGrounded = false;
        }
    }
    void HandleHorizontalMovement()
    {
        float componentX = (TargetPos.X - Position.X) >=  0 ? 1 : -1;

        Velocity = new Vector2(componentX * _movementSpeed, Velocity.Y);
    }

    void HandleJump(GameTime gameTime)
    {
        _timer += (float)gameTime.ElapsedGameTime.TotalSeconds;
        if (_timer >= 2)
        {
            Velocity = new Vector2(Velocity.X, _jumpStrength);
            _timer = 0;
        }

        if (!IsGrounded)
        {
            HandleHorizontalMovement();
        }
        
    }

    public void Draw(SpriteBatch spriteBatch)
    {
        _sprite.Draw(spriteBatch, Position);
    }
}