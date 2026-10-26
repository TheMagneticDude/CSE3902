using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
namespace sprint2.Weapons;

public interface IProjectile
{
    void Attack(Vector2 playerLocation, bool facingRight);
    void Update(GameTime gameTime, bool facingRight);
    void Draw(SpriteBatch spriteBatch, Vector2 playerLocation, bool facingRight);
}
