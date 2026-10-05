using System;
using System.Diagnostics.Contracts;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace sprint2.Projectiles;

public abstract class Projectile : IProjectile
{
    public Texture2D Texture {get;}
    public Vector2 Position {get; protected set;}
    public Vector2 Velocity {get; protected set;}
    public bool IsActive{get; protected set;}

    protected Projectile(Texture2D texture, Vector2 position, Vector2 velocity)
    {
        Texture = texture;
        Position = position;
        Velocity = velocity;
        IsActive = true;
    }

    public virtual void Update(GameTime gameTime)
    {
        ArgumentNullException.ThrowIfNull(gameTime);
        float deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;
        Position += Velocity * deltaTime;
    }

    public abstract void Draw(SpriteBatch spriteBatch);

    public virtual void Destroy()
    {
        IsActive = false;
    }
}