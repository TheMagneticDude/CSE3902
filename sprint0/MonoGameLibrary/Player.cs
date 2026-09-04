using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGameLibrary;
using MonoGameLibrary.Graphics;

namespace sprint0;

public class Player : IPlayer
{
    private AnimatedSprite _sprite;
    private Vector2 _position;
    private const float MOVEMENT_SPEED = 5.0f;
    private const float BOOST_MULTIPLIER = 5.5f;

    public Keybinds Binds { get; private set; }
    public Vector2 Position => _position;
    public Circle Bounds => new Circle(
        (int)(_position.X + (_sprite.Width * 0.5f)),
        (int)(_position.Y + (_sprite.Height * 0.5f)),
        (int)(_sprite.Width * 0.5f)
    );

    public Player(AnimatedSprite sprite, Vector2 startPosition)
    {
        _sprite = sprite;
        _position = startPosition;
    }

    public void Update(GameTime gameTime, Rectangle screenBounds, Keybinds binds)
    {
        Binds = binds;
        _sprite.Update(gameTime);
        HandleInput();
        KeepInBounds(screenBounds);
    }

    public void Draw(SpriteBatch spriteBatch)
    {
        _sprite.Draw(spriteBatch, _position);
    }

    private void HandleInput()
    {
        float speed = MOVEMENT_SPEED;
        
        //handles all keybinds
        
        if (Binds.Zoom) 
        {
            speed *= BOOST_MULTIPLIER;
        }
        if (Binds.Up) _position.Y -= speed;
        if (Binds.Down) _position.Y += speed;
        if (Binds.Left) _position.X -= speed;
        if (Binds.Right) _position.X += speed;
    }

    private void KeepInBounds(Rectangle screenBounds)
    {
        if (Bounds.Left < screenBounds.Left) _position.X = screenBounds.Left;
        else if (Bounds.Right > screenBounds.Right) _position.X = screenBounds.Right - _sprite.Width;

        if (Bounds.Top < screenBounds.Top) _position.Y = screenBounds.Top;
        else if (Bounds.Bottom > screenBounds.Bottom) _position.Y = screenBounds.Bottom - _sprite.Height;
    }
}