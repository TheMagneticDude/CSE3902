using System.Reflection.Metadata;
using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGameLibrary.Graphics;
using sprint2.Collision;
using static sprint2.Constants;


namespace sprint2.Bosses;

public class Slime: IBoss
{
    public Vector2 Position { get; set; }
    public Vector2 Velocity { get; set; }
    public Vector2 Drag {get; set;}
    public bool IsGrounded { get;  set; }
    public Vector2 TargetPos {get;set;}
    public Color Color {get;set;}

    public ICollider Hitbox {get; set;}

    private float _jumpStrength = -10f - (float)random.Next(10);
    private bool jumped = false;
    private float _timer;

    private static Random random = new Random();
    private AnimatedSprite _sprite;

    private TextureAtlas _atlas;
    private uint _movementSpeed = 5 + (uint) random.Next(5);

    

    
    public Slime(AnimatedSprite sprite, TextureAtlas atlas)
    {
        _sprite = sprite;
        _atlas = atlas;
        Position = new Vector2(200 - random.Next(10)*10, 100); //randomize where they spawn
        Drag = new Vector2(2f,0f);
        Velocity = new Vector2(0, 0);
        Color = Color.White;

        Hitbox = new CenteredRectangularCollider(Position, _sprite.Width, _sprite.Height);
    }

    public Slime(AnimatedSprite sprite, TextureAtlas atlas, Color c)
    {
        _sprite = sprite;
        _atlas = atlas;
        Position = new Vector2(200 - random.Next(10)*10, 80);
        Drag = new Vector2(2f,0f);
        Velocity = new Vector2(0, 0);
        Color = c;

        Hitbox = new CenteredRectangularCollider(Position, _sprite.Width, _sprite.Height);
    }

    public void SetColor(Color c)
    {
        Color = c;
    }

    public void SetAnimation(string animationName)
    {
        _sprite.Animation = _atlas.GetAnimation(animationName);
    }

    public void Update(GameTime gameTime, Vector2 PlayerPos)
    {
        ArgumentNullException.ThrowIfNull(gameTime);

        TargetPos = PlayerPos;
        _sprite.Update(gameTime);
        ApplyPhysics();
        HandleMovement();
        HandleJump(gameTime);
    }

    public void HandleMovement()
    {
        // Position += Velocity; //velocity handled within room collision logic now
        if (IsGrounded){Velocity = CalcDrag(Velocity, Drag);}
        
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
        Hitbox.Update(Position);
        Velocity = new Vector2(Velocity.X, Velocity.Y + Gravity);
        //void floor
        // if (Position.Y >= GroundLevel)
        // {
        //     Position = new Vector2(Position.X, GroundLevel);
        //     Velocity = new Vector2(Velocity.X, 0f);
        //     IsGrounded = true;
        // }
        // else
        // {
        //     IsGrounded = false;
        // }

        if (IsGrounded)
        {
            Velocity = new Vector2(Velocity.X, 0f);
        }
    }
    private void HandleHorizontalMovement()
    {
        float componentX = (TargetPos.X - Position.X) >=  0 ? 1 : -1;

        Velocity = new Vector2(componentX * _movementSpeed, Velocity.Y);
    }

    private void HandleJump(GameTime gameTime)
    {
        _timer += (float)gameTime.ElapsedGameTime.TotalSeconds;
        if (IsGrounded)
        {
            if (_timer >= 2f)
            {
                HandleHorizontalMovement();
                Velocity = new Vector2(Velocity.X, _jumpStrength);
                IsGrounded = false;
                _timer = 0f;
            }
        }
        
    }

    public void Draw(SpriteBatch spriteBatch)
    {
        _sprite.Color = Color;
        _sprite.Draw(spriteBatch, Position);
        Hitbox.DrawDebug(spriteBatch);
    }
}