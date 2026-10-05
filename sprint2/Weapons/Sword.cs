using System.Numerics;
using System.Security.Cryptography.X509Certificates;
using System.Threading;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Vector2 = Microsoft.Xna.Framework.Vector2;
using sprint2.Weapons;

public class Sword : Weapon
{
    private const float SwingStart = -MathHelper.PiOver2;
    private const float SwingEnd = MathHelper.PiOver2;
    private readonly Vector2 _rightHandOffset = new Vector2(20f, 20f);
    private readonly Vector2 _leftHandOffset = new Vector2(10f, 20f);
    public Sword(Texture2D texture) : base(texture, 0.25f)
    {
        
    }

    public override void Update(GameTime gameTime, bool facingRight)
    {
        if(IsAttacking)
        {
            float deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;

            AttackTimer += deltaTime;

            float progress = AttackTimer / AttackDuration;

            progress = MathHelper.Clamp (progress, 0f, 1f);

            if(facingRight)
            {
                Rotation = MathHelper.Lerp(SwingStart, SwingEnd, progress);
            }
            else
            {
                Rotation = MathHelper.Lerp(-SwingStart, -SwingEnd, progress);
            }
            if(AttackTimer >= AttackDuration)
            {
                FinishAttack();
            }
        }
    }

    public override void Draw(SpriteBatch spriteBatch, Vector2 playerLocation, bool facingRight)
    {
        //Don't draw if not attacking
        if(IsAttacking)
        {

            //All for setting up the draw
            Vector2 handOffset = facingRight ? _rightHandOffset : _leftHandOffset;
            Vector2 weaponPosition = playerLocation + handOffset;
            
            // SET ORIGIN TO THE HANDLE

            //Vector2 originOfWeapon = new Vector2(0, Texture.Height);

            Vector2 originOfWeapon = facingRight ? new Vector2(2f, Texture.Height - 2f) : new Vector2(Texture.Width - 2f, Texture.Height - 2f);

            SpriteEffects effect = facingRight ? SpriteEffects.None : SpriteEffects.FlipHorizontally;
            float weaponScale = 2.0f;

            spriteBatch.Draw(Texture, weaponPosition, null, Color.White, Rotation, originOfWeapon, weaponScale, effect, 0f);
        }
    }
    
}