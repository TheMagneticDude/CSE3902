using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using sprint2.Combat;

namespace sprint2.Projectiles;

public interface IProjectile
{
    bool IsActive {get;}
    Vector2 Position {get;}
    int Damage {get;}
    float Knockback {get;}
    CombatTeam Team {get;}
    
    void Update(GameTime gameTime);
    void Draw(SpriteBatch spriteBatch);
}
