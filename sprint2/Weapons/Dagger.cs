using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using sprint2.Projectiles;

namespace sprint2.Weapons;

public class Dagger : Weapon
{
    private const float SwingStart =
        -MathHelper.PiOver2;

    private const float SwingEnd =
        MathHelper.PiOver2;


    private readonly Vector2 _rightHandOffset =
        new Vector2(20f, 20f);

    private readonly Vector2 _leftHandOffset =
        new Vector2(10f, 20f);


    private readonly ProjectileManager _projectileManager;


    public Dagger(
        Texture2D texture,
        ProjectileManager projectileManager)
        : base(texture, 0.25f)
    {
        _projectileManager =
            projectileManager;
    }


    public override void Attack(
        Vector2 playerLocation,
        bool facingRight)
    {
        // Don't start another attack
        // while one is already happening.
        if (IsAttacking)
        {
            return;
        }


        // Start the normal Weapon attack timer.
        base.Attack(
            playerLocation,
            facingRight
        );


        float direction =
            facingRight ? 1f : -1f;


        // Start the projectile near the player's hand.
        Vector2 spawnLocation =
            playerLocation +
            new Vector2(
                20f * direction,
                20f
            );


        // Projectile travels in the direction
        // the player is facing.
        Vector2 velocity =
            new Vector2(
                500f * direction,
                0f
            );


        DaggerProjectile projectile =
            ProjectileFactory.Instance.CreateDagger(
                spawnLocation,
                velocity
            );


        _projectileManager.AddProjectile(
            projectile
        );
    }


    public override void Update(
        GameTime gameTime,
        bool facingRight)
    {
        if (IsAttacking)
        {
            float deltaTime =
                (float)gameTime
                    .ElapsedGameTime
                    .TotalSeconds;


            AttackTimer += deltaTime;


            float progress =
                AttackTimer /
                AttackDuration;


            progress =
                MathHelper.Clamp(
                    progress,
                    0f,
                    1f
                );


            // Same swing behavior as Sword.
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
        if(!IsAttacking)
        {
            return;
        }
        Vector2 handOffset =
            facingRight
                ? _rightHandOffset
                : _leftHandOffset;


        Vector2 weaponPosition =
            playerLocation +
            handOffset;

        Vector2 originOfWeapon =
            facingRight
                ? new Vector2(
                    2f,
                    Texture.Height - 2f
                )
                : new Vector2(
                    Texture.Width - 2f,
                    Texture.Height - 2f
                );


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