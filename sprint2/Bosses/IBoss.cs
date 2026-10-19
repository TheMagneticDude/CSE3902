using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace sprint2.Bosses;

public interface IBoss
{
    Vector2 Position { get; set; }
    
    void Update(GameTime gameTime, Vector2 PlayerPos);

    void Draw(SpriteBatch spriteBatch);
}