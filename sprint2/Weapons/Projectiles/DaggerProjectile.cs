using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace sprint2.Projectiles;

public class DaggerProjectile : Projectile
{
    public DaggerProjectile(Texture2D texture, Vector2 position, Vector2 velocity) : base(texture, position, velocity)
    {
    }

    public override void Update(GameTime gameTime)
    {
        base.Update(gameTime);

        if (!IsActive)
        {
            return;
        }
    }

    public override void Draw(SpriteBatch spriteBatch)
    {
        ArgumentNullException.ThrowIfNull(spriteBatch);
        if (IsActive)
        {
            float rotation = (float)Math.Atan2(Velocity.Y,Velocity.X) + MathHelper.PiOver2;
            Vector2 origin = new Vector2(Texture.Width / 2f, Texture.Height / 2f);
            spriteBatch.Draw(Texture, Position, null, Color.White, rotation, origin, 1f, SpriteEffects.None, 0f);
        }
    }

}
