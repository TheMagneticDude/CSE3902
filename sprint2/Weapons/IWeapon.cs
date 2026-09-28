using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
namespace sprint2.Weapons;

public interface IWeapon
{
    bool IsAttacking {get;}
    void Attack();
    void Update(GameTime gameTime, bool facingRight);
    void Draw(SpriteBatch spriteBatch, Vector2 playerLocation, bool facingRight);
}