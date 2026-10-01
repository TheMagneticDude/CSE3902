using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace sprint2.Projectiles;

public interface IProjectile
{
    bool IsActive {get;}
    Vector2 Position {get;}
    void Update(GameTime gameTime);
    void Draw(SpriteBatch spriteBatch);
}
