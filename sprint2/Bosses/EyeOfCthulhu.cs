using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGameLibrary.Graphics;
using sprint2.Bosses.States;

namespace sprint2.Bosses;

public class EyeOfCthulhu: IBoss
{
    public Vector2 Location { get; set; }

    public Vector2 Velocity {get; set;}

    private AnimatedSprite _sprite;

    private TextureAtlas _atlas;

    private IBossState _state;

    private uint movementSpeed = 6;

    public void ChangeState(IBossState newState)
    {
        _state = newState;
        _state.Enter();
    }

    public EyeOfCthulhu(AnimatedSprite sprite, TextureAtlas atlas)
    {
        _sprite = sprite;
        _atlas = atlas;
        Location = new Vector2(1100, 600);
        Velocity = new Vector2(movementSpeed, 0);
        ChangeState(new CthulhuPhase1State(this));
    }

     public void SetAnimation(string animationName)
    {
        _sprite.Animation = _atlas.GetAnimation(animationName);
    }


    public void Update(GameTime gameTime, Vector2 PlayerPos)
    {
        _state.Update(gameTime);
        _sprite.Update(gameTime);

         Location += Velocity;

        if (Location.X >= 1280)
        {
            Velocity = new Vector2(-movementSpeed,0);
            _sprite.Effects = SpriteEffects.None;
        }
        if (Location.X <= 0)
        {
            Velocity = new Vector2(movementSpeed, 0);
            _sprite.Effects = SpriteEffects.FlipVertically;
        }
    }

    public void Draw(SpriteBatch spriteBatch)
    {
        _sprite.Draw(spriteBatch, Location);
    }
}