using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using terraria.Collision;

namespace terraria.Weapons;

public abstract class Weapon : IWeapon
{
    public ICollider Hitbox { get; protected set; }

    public virtual int Damage => 20;
    public int AttackId { get; private set; }

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

    public virtual void Attack(Vector2 playerLocation, bool facingRight)
    {
        // Don't restart an attack while one is already happening.
        if (IsAttacking)
        {
            return;
        }

        AttackId++;
        IsAttacking = true;
        AttackTimer = 0f;
    }

    public virtual void Attack(Vector2 playerLocation, bool facingRight, Point cursorPos)
    {
        // Don't restart an attack while one is already happening.
        if (IsAttacking)
        {
            return;
        }

        AttackId++;
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

    public virtual void UpdateHitbox(Vector2 playerLocation, bool facingRight)
    {
    }
}
