using sprint2.Collision;
using sprint2.Combat;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace sprint2.Bosses;

public interface IBoss
{
    ICollider Hitbox { get; }
    HealthComponent Health { get; }
    int ContactDamage { get; }
    Vector2 Position { get; set; }
    
    void Update(GameTime gameTime, Vector2 PlayerPos);

    void Draw(SpriteBatch spriteBatch);
}
