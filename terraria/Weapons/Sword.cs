using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using sprint2.Weapons;
using sprint2.Collision;

public class Sword : Weapon
{
    private const float WeaponScale = 1.3f;
    private const float SwingStart = -MathHelper.PiOver2;
    private const float SwingEnd = MathHelper.PiOver2;
    private readonly Vector2 _rightHandOffset = new Vector2(22f, 30f);
    private readonly Vector2 _leftHandOffset = new Vector2(5f, 30f);

    public Sword(Texture2D texture) : base(texture, 0.25f)
    {
        ArgumentNullException.ThrowIfNull(texture);

        Hitbox = new RotatedRectangleCollider(
            Vector2.Zero, texture.Width * WeaponScale * 0.6f, texture.Height * WeaponScale * 0.75f);
    }

    public override void Update(GameTime gameTime, bool facingRight)
    {
        ArgumentNullException.ThrowIfNull(gameTime);
        if (IsAttacking)
        {
            float deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;

            AttackTimer += deltaTime;

            float progress = AttackTimer / AttackDuration;

            progress = MathHelper.Clamp(progress, 0f, 1f);

            if (facingRight)
            {
                Rotation = MathHelper.Lerp(SwingStart, SwingEnd, progress);
            }
            else
            {
                Rotation = MathHelper.Lerp(-SwingStart, -SwingEnd, progress);
            }
            if (AttackTimer >= AttackDuration)
            {
                FinishAttack();
            }
        }
    }

    public override void Draw(SpriteBatch spriteBatch, Vector2 playerLocation, bool facingRight)
    {
        ArgumentNullException.ThrowIfNull(spriteBatch);
        if (IsAttacking)
        {
            Vector2 handOffset = facingRight ? _rightHandOffset : _leftHandOffset;
            Vector2 weaponPosition = playerLocation + handOffset;

            Vector2 originOfWeapon = facingRight ? new Vector2(2f, Texture.Height - 2f) : new Vector2(Texture.Width - 2f, Texture.Height - 2f);

            SpriteEffects effect = facingRight ? SpriteEffects.None : SpriteEffects.FlipHorizontally;

            spriteBatch.Draw(Texture, weaponPosition, null, Color.White, Rotation, originOfWeapon, WeaponScale, effect, 0f);
            Hitbox.DrawDebug(spriteBatch);
        }
    }

    public override void UpdateHitbox(Vector2 playerLocation, bool facingRight)
    {
        if (Hitbox is not RotatedRectangleCollider collider)
        {
            return;
        }

        Vector2 handOffset = facingRight ? _rightHandOffset : _leftHandOffset;
        Vector2 weaponPosition = playerLocation + handOffset;

        Vector2 origin = facingRight
            ? new Vector2(2f, Texture.Height - 2f)
            : new Vector2(Texture.Width - 2f, Texture.Height - 2f);

        // The artwork is diagonal: follow the blade from the guard to its tip.
        Vector2 bladeStart = new Vector2(Texture.Width * 0.25f, Texture.Height * 0.75f);
        Vector2 bladeEnd = new Vector2(Texture.Width * 0.94f, Texture.Height * 0.06f);
        if (!facingRight)
        {
            bladeStart.X = Texture.Width - bladeStart.X;
            bladeEnd.X = Texture.Width - bladeEnd.X;
        }
        Vector2 bladeCenter = (bladeStart + bladeEnd) / 2f;
        Vector2 bladeDirection = bladeEnd - bladeStart;

        Vector2 localOffset = (bladeCenter - origin) * WeaponScale;
        float cos = MathF.Cos(Rotation);
        float sin = MathF.Sin(Rotation);

        Vector2 rotatedOffset = new Vector2(
            localOffset.X * cos - localOffset.Y * sin,
            localOffset.X * sin + localOffset.Y * cos);

        collider.Position = weaponPosition + rotatedOffset;
        collider.Width = bladeDirection.Length() * WeaponScale;
        collider.Height = Texture.Width * 0.22f * WeaponScale;
        collider.Rotation = Rotation + MathF.Atan2(bladeDirection.Y, bladeDirection.X);
    }
}
