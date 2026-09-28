using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGameLibrary.Graphics;
using sprint2.Bosses.States;

namespace sprint2.Bosses;

public class EyeOfCthulhu
{
    public Vector2 Location { get; set; }

    private AnimatedSprite _sprite;

    private TextureAtlas _atlas;

    private IBossState _state;

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
        ChangeState(new CthulhuPhase1State(this));
    }

     public void SetAnimation(string animationName)
    {
        _sprite.Animation = _atlas.GetAnimation(animationName);
    }


    public void Update(GameTime gameTime)
    {
         _state.Update(gameTime);
        _sprite.Update(gameTime);
    }

    public void Draw(SpriteBatch spriteBatch)
    {
        _sprite.Draw(spriteBatch, Location);
    }
}