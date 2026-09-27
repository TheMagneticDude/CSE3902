using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGameLibrary.Graphics;

namespace sprint2.Bosses;

public class QueenBee
{
    public Vector2 Location { get; set; }
    public Vector2 Velocity {get; set;}

    private AnimatedSprite _sprite;

    private TextureAtlas _atlas;
    private uint movementSpeed = 10;

    public QueenBee(AnimatedSprite sprite, TextureAtlas atlas)
    {
        _sprite = sprite;
        _atlas = atlas;
        Location = new Vector2(400, 300);
        Velocity = new Vector2(movementSpeed, 0);  //move right on init
    }

    public void Update(GameTime gameTime)
    {
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