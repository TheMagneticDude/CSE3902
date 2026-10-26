using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace sprint2.Blocks;
public interface IBlock
{
    Vector2 Location { get; set; }
    
    void Update(GameTime gameTime);

    void Draw(SpriteBatch spriteBatch);
}