using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGameLibrary;
using MonoGameLibrary.Graphics;
using MonoGameLibrary.Input;

namespace sprint0;

public class Player : IPlayer
{
    private AnimatedSprite _sprite;
    private Vector2 _position;
    private const float MOVEMENT_SPEED = 5.0f;

    public Vector2 Position => _position;


    //calculates bounding box based on position
    public Circle Bounds => new Circle(
        (int)(_position.X + (_sprite.Width * 0.5f)),
        (int)(_position.Y + (_sprite.Height * 0.5f)),
        (int)(_sprite.Width * 0.5f)
    );


    /// <summary>
    /// Creates a new Player.
    /// </summary>
    public Player(AnimatedSprite sprite, Vector2 startPosition)
    {
        _sprite = sprite;
        _position = startPosition;
    }

    public void Draw(SpriteBatch spriteBatch)
    {
        _sprite.Draw(spriteBatch, _position);
    }
    
}