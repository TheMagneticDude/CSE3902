using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using terraria.Combat;
using terraria.Collision;

namespace terraria.Projectiles;

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
