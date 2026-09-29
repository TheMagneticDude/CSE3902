using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace sprint2.Bosses;

public interface IBoss
{
    void Update(GameTime gameTime);

    void Draw(SpriteBatch spriteBatch);
}