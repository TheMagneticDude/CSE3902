using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGameLibrary.Graphics;

namespace sprint2.Bosses;

public class QueenBee
{
    public Vector2 Location { get; set; }

    private AnimatedSprite _sprite;

    private TextureAtlas _atlas;

    public QueenBee(AnimatedSprite sprite, TextureAtlas atlas)
    {
        _sprite = sprite;
        _atlas = atlas;
        Location = new Vector2(400, 300);
    }

    public void Update(GameTime gameTime)
    {
        _sprite.Update(gameTime);
    }

    public void Draw(SpriteBatch spriteBatch)
    {
        _sprite.Draw(spriteBatch, Location);
    }
}