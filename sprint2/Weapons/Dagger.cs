using System;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using sprint2.Projectiles;
using sprint2.World;

namespace sprint2.Weapons;

public class Dagger : Weapon
{
    private const float SwingStart = -MathHelper.PiOver2;
    private const float SwingEnd = MathHelper.PiOver2;

    private readonly Vector2 _rightHandOffset = new Vector2(20f, 20f);
    private readonly Vector2 _leftHandOffset = new Vector2(10f, 20f);

    private readonly ProjectileManager _projectileManager;

    public Dagger(
        Texture2D texture,
        ProjectileManager projectileManager)
        : base(texture, 0.25f)
    {
        _projectileManager = projectileManager;
    }

    public override void Attack(
        Vector2 playerLocation,
        bool facingRight, Point cursorPos)
    {
        if (IsAttacking)
        {
            return;
        }

        base.Attack(playerLocation, facingRight);

        float direction = facingRight ? 1f : -1f;

        Vector2 spawnLocation =
            playerLocation + new Vector2(20f * direction, 20f);

        
        
        Vector2 MousePointer = new Vector2(cursorPos.X, cursorPos.Y);
        Vector2 PointerWorldPos = WorldHandler.PixelToWorldSpace(MousePointer);
        Vector2 velVect = PointerWorldPos - playerLocation;
        velVect.Normalize();

        Vector2 velocity = velVect * 500f;

        DaggerProjectile projectile =
            ProjectileFactory.Instance.CreateDagger(
                spawnLocation,
                velocity
            );

        _projectileManager.AddProjectile(projectile);
    }

    public override void Update(
        GameTime gameTime,
        bool facingRight)
    {
        ArgumentNullException.ThrowIfNull(gameTime);
        if (IsAttacking)
        {
            float deltaTime =
                (float)gameTime.ElapsedGameTime.TotalSeconds;

            AttackTimer += deltaTime;

            float progress =
                AttackTimer / AttackDuration;

            progress =
                MathHelper.Clamp(progress, 0f, 1f);

            if (facingRight)
            {
                Rotation =
                    MathHelper.Lerp(
                        SwingStart,
                        SwingEnd,
                        progress
                    );
            }
            else
            {
                Rotation =
                    MathHelper.Lerp(
                        -SwingStart,
                        -SwingEnd,
                        progress
                    );
            }

            if (AttackTimer >= AttackDuration)
            {
                FinishAttack();
            }
        }
    }


    public override void Draw(
        SpriteBatch spriteBatch,
        Vector2 playerLocation,
        bool facingRight)
    {
        ArgumentNullException.ThrowIfNull(spriteBatch);
        // Keep the equipped dagger visible even when it is not attacking.
        Vector2 handOffset =
            facingRight
                ? _rightHandOffset
                : _leftHandOffset;

        Vector2 weaponPosition =
            playerLocation + handOffset;

        Vector2 originOfWeapon =
            facingRight
                ? new Vector2(2f, Texture.Height - 2f)
                : new Vector2(Texture.Width - 2f, Texture.Height - 2f);

        SpriteEffects effect =
            facingRight
                ? SpriteEffects.None
                : SpriteEffects.FlipHorizontally;

        float weaponScale = 1.0f;

        spriteBatch.Draw(
            Texture,
            weaponPosition,
            null,
            Color.White,
            Rotation,
            originOfWeapon,
            weaponScale,
            effect,
            0f
        );
    }
}
