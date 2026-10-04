using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using sprint2.Players;

namespace sprint2.Hotbar;

public interface IHotbarEntry
{
    string Name { get; }

    void Use(IPlayer player);
    void Update(GameTime gameTime, IPlayer player);
    void Draw(SpriteBatch spriteBatch, IPlayer player);
}
