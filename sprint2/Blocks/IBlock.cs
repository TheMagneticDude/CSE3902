using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace sprint2.Blocks;
public interface IBlock
{
    void Update(GameTime gameTime);

    void Draw(SpriteBatch spriteBatch);
}