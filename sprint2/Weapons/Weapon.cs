using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace sprint2.Weapons;

public abstract class Weapon : IWeapon
{
    public Texture2D Texture { get; }

    public bool IsAttacking { get; protected set; }

    public float AttackDuration { get; protected set;}

    public float Rotation {get; protected set;}

    public float AttackTimer {get; set;}

    protected Weapon(
        Texture2D texture,
        float attackDuration = 0.25f)
    {
        Texture = texture;
        AttackDuration = attackDuration;

        IsAttacking = false;
        AttackTimer = 0f;
    }

    public virtual void Attack()
    {
        // Don't restart an attack while one is already happening.
        if (IsAttacking)
        {
            return;
        }

        IsAttacking = true;
        AttackTimer = 0f;
    }

    public abstract void Update(
        GameTime gameTime,
        bool facingRight);

    protected virtual void FinishAttack()
    {
        IsAttacking = false;
        AttackTimer = 0f;
    }

    public abstract void Draw(
        SpriteBatch spriteBatch,
        Vector2 playerLocation,
        bool facingRight);
}