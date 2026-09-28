using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace sprint2.Weapons;

public class Weapon
{
    public Texture2D Texture { get; }

    public bool IsAttacking { get; private set; }

    public float Rotation { get; private set; }

    public float AttackDuration { get; }

    private float _attackTimer;

    public Weapon(
        Texture2D texture,
        float attackDuration = 0.25f)
    {
        Texture = texture;
        AttackDuration = attackDuration;

        IsAttacking = false;
        Rotation = 0f;
        _attackTimer = 0f;
    }

    public virtual void Attack()
    {
        // Don't restart an attack while one is already happening.
        if (IsAttacking)
            return;

        IsAttacking = true;
        _attackTimer = 0f;
    }

    public virtual void Update(
        GameTime gameTime,
        bool facingRight)
    {
        if (!IsAttacking)
            return;

        float deltaTime =
            (float)gameTime.ElapsedGameTime.TotalSeconds;

        _attackTimer += deltaTime;

        float progress =
            _attackTimer / AttackDuration;

        progress = MathHelper.Clamp(
            progress,
            0f,
            1f
        );

        /*
         * Rotate the weapon through a 180-degree arc.
         *
         * These values can be changed later depending
         * on how the sword image is oriented.
         */
        if (facingRight)
        {
            Rotation = MathHelper.Lerp(
                -MathHelper.PiOver2,
                MathHelper.PiOver2,
                progress
            );
        }
        else
        {
            Rotation = MathHelper.Lerp(
                MathHelper.PiOver2,
                -MathHelper.PiOver2,
                progress
            );
        }

        if (_attackTimer >= AttackDuration)
        {
            FinishAttack();
        }
    }

    protected virtual void FinishAttack()
    {
        IsAttacking = false;
        _attackTimer = 0f;
        Rotation = 0f;
    }

    public virtual void Draw(
        SpriteBatch spriteBatch,
        Vector2 playerLocation,
        bool facingRight)
    {
        // For now, only display the weapon during an attack.
        if (!IsAttacking)
            return;

        Vector2 handOffset;

        if (facingRight)
        {
            handOffset = new Vector2(
                20f,
                20f
            );
        }
        else
        {
            handOffset = new Vector2(
                -20f,
                20f
            );
        }

        Vector2 weaponPosition =
            playerLocation + handOffset;

        /*
         * The origin determines where the weapon rotates from.
         *
         * This assumes the sword handle is near the bottom
         * center of the image.
         */
        Vector2 origin = new Vector2(
            Texture.Width / 2f,
            Texture.Height
        );

        SpriteEffects effect =
            facingRight
                ? SpriteEffects.None
                : SpriteEffects.FlipHorizontally;

        spriteBatch.Draw(
            Texture,
            weaponPosition,
            null,
            Color.White,
            Rotation,
            origin,
            1f,
            effect,
            0f
        );
    }
}