using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using sprint2.Combat;
using sprint2.Collision;

namespace sprint2.Projectiles;

public interface IProjectile
{
    ICollider Hitbox { get; }
    bool IsActive {get;}
    Vector2 Position {get;}
    int Damage { get; }
    void Destroy();
    //float Knockback {get;}
    //CombatTeam Team {get;}
    
    void Update(GameTime gameTime);
    void Draw(SpriteBatch spriteBatch);
}
