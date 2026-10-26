using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using sprint2.Collision;

namespace sprint2.Projectiles;

public class DaggerProjectile : Projectile
{
    public DaggerProjectile(Texture2D texture, Vector2 position, Vector2 velocity)
        : base(texture, position, velocity)
    {
        // The dagger texture points up, so offset the flight angle by 90 degrees.
        Hitbox = new RotatedRectangleCollider(position, 12f, 28f)
        {
            Rotation = MathF.Atan2(velocity.Y, velocity.X) + MathHelper.PiOver2
        };
    }

    public override void Update(GameTime gameTime)
    {
        if (!IsActive)
        {
            return;
        }

        base.Update(gameTime);
        Hitbox.Position = Position;
    }

    public override void Draw(SpriteBatch spriteBatch)
    {
        ArgumentNullException.ThrowIfNull(spriteBatch);
        if (!IsActive)
        {
            return;
        }

        Vector2 origin = new Vector2(Texture.Width / 2f, Texture.Height / 2f);
        float rotation = ((RotatedRectangleCollider)Hitbox).Rotation;
        spriteBatch.Draw(Texture, Position, null, Color.White, rotation, origin, 1f, SpriteEffects.None, 0f);
        Hitbox.DrawDebug(spriteBatch);
    }
}
