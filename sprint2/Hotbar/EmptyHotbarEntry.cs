using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using sprint2.Players;

namespace sprint2.Hotbar;

public class EmptyHotbarEntry : IHotbarEntry
{
    public string Name { get; }

    public EmptyHotbarEntry(string name)
    {
        Name = name;
    }

    public void Use(IPlayer player)
    {
        player.UnequipWeapon();
    }

    public void Update(GameTime gameTime, IPlayer player)
    {
    }

    public void Draw(SpriteBatch spriteBatch, IPlayer player)
    {
    }
}
