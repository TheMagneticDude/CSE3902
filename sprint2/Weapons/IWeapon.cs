using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
namespace sprint2.Weapons;

public interface IWeapon
{
    bool IsAttacking {get;}

    Texture2D Texture { get; }
    void Attack(Vector2 playerLocation, bool facingRight);
    void Attack(Vector2 playerLocation, bool facingRight, Point cursosPos);
    void Update(GameTime gameTime, bool facingRight);
    void Draw(SpriteBatch spriteBatch, Vector2 playerLocation, bool facingRight);
}
