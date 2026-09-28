using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGameLibrary.Graphics;
using sprint2.Bosses.States;

namespace sprint2.Bosses;

public class QueenBee
{
    public Vector2 Location { get; set; }
    public Vector2 Velocity {get; set;}

    private AnimatedSprite _sprite;

    private TextureAtlas _atlas;
    private uint movementSpeed = 10;

    private IBossState _state;

    public void ChangeState(IBossState newState)
    {
        _state = newState;
        _state.Enter();
    }

    public QueenBee(AnimatedSprite sprite, TextureAtlas atlas)
    {
        _sprite = sprite;
        _atlas = atlas;
        Location = new Vector2(1100, 400);
        Velocity = new Vector2(movementSpeed, 0);  //move right on init
        ChangeState(new QueenBeeIdleState(this));
    }

    public void SetAnimation(string animationName)
    {
        _sprite.Animation = _atlas.GetAnimation(animationName);
    }

    public void Update(GameTime gameTime)
    {
        _state.Update(gameTime);
        _sprite.Update(gameTime);

        Location += Velocity;

        if (Location.X >= 1280)
        {
            Velocity = new Vector2(-movementSpeed,0);
        }
        if (Location.X <= 0)
        {
            Velocity = new Vector2(movementSpeed, 0);
        }
    }

    public void Draw(SpriteBatch spriteBatch)
    {
        _sprite.Draw(spriteBatch, Location);
    }
}