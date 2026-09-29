using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace sprint2.Weapons;

public sealed class Dagger : Weapon
{
    private const float HorizontalSpeed = 5f;
    private const float DaggerAttackDuration = 2f;

    private readonly Vector2 _rightHandOffset = new(20f, 20f);
    private readonly Vector2 _leftHandOffset = new(-20f, 20f);

    public Vector2 Location { get; private set; }
    public Vector2 Velocity { get; private set; }

    public Dagger(Texture2D texture) : base(texture, DaggerAttackDuration)
    {
        Location = Vector2.Zero;
        Velocity = Vector2.Zero;
    }

    public override void Attack(Vector2 playerLocation, bool facingRight)
    {
        if (IsAttacking)
        {
            return;
        }

        Vector2 handOffset;

        if (facingRight)
        {
            handOffset = _rightHandOffset;
        }
        else
        {
            handOffset = _leftHandOffset;
        }

        Location = playerLocation + handOffset;

        if (facingRight)
        {
            Velocity = new Vector2(HorizontalSpeed, 0f);
        }
        else
        {
            Velocity = new Vector2(-HorizontalSpeed, 0f);
        }

        base.Attack(playerLocation, facingRight);
    }

    public override void Update(GameTime gameTime, bool facingRight)
    {
        if (!IsAttacking)
        {
            return;
        }

        Location += Velocity;
        AttackTimer += (float)gameTime.ElapsedGameTime.TotalSeconds;

        if (AttackTimer >= AttackDuration)
        {
            Velocity = Vector2.Zero;
            FinishAttack();
        }
    }

    public override void Draw(SpriteBatch spriteBatch, Vector2 playerLocation, bool facingRight)
    {
        if (!IsAttacking)
        {
            return;
        }

        Vector2 origin = new(Texture.Width / 2f, Texture.Height / 2f);
        SpriteEffects effect;

        float rot = 0;

        if (Velocity.X >= 0f)
        {
            effect = SpriteEffects.None;
            rot = (float) Math.PI/2f;
        }
        else
        {
            effect = SpriteEffects.FlipHorizontally;
            rot = - (float) Math.PI/2f;
        }

        spriteBatch.Draw(
            Texture,
            Location,
            null,
            Color.White,
            rot,
            origin,
            1f,
            effect,
            0f);
    }
}
