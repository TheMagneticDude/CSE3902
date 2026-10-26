using sprint2.Collision;
using sprint2.Combat;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using sprint2.Collision;

namespace sprint2.Bosses;

public interface IBoss
{
    HealthComponent Health { get; }
    int ContactDamage { get; }
    Vector2 Position { get; set; }
    Vector2 Velocity { get; set; }
    public bool IsGrounded { get; set; }
    ICollider Hitbox {get;}
    
    void Update(GameTime gameTime, Vector2 PlayerPos);

    void Draw(SpriteBatch spriteBatch);
}
