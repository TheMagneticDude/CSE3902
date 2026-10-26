using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace sprint2.Weapons;

public abstract class Projectile : IProjectile
{
    public Texture2D Texture { get; }

    public float Rotation {get; protected set;}
    public float LifeSpan {get; set;}//how long it can stay in the game

    public float AttackTimer {get; set;}//how long its been in the game

    protected Projectile(
        Texture2D texture,
        float attackDuration = 0.25f)
    {
        Texture = texture;

        AttackTimer = 0f;
    }

    public virtual void Attack(Vector2 playerLocation, bool facingRight)
    {
        AttackTimer = 0f;
    }


    public abstract void Update(
        GameTime gameTime,
        bool facingRight);

    protected virtual void FinishAttack()
    {
        AttackTimer = 0f;
    }

    public abstract void Draw(
        SpriteBatch spriteBatch,
        Vector2 playerLocation,
        bool facingRight);
}
