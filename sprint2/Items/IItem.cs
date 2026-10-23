using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using sprint2.Hotbar;
using sprint2.Players;

namespace sprint2.Items;

public interface IItem : IHotbarEntry
{
    bool IsActive { get; }

    void Update(GameTime gameTime, IPlayer player);

    void Draw(SpriteBatch spriteBatch, IPlayer player);

    void DrawIcon(SpriteBatch spriteBatch, Rectangle slot);

    void Use(IPlayer player);
}
