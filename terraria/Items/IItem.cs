using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using terraria.Hotbar;
using terraria.Players;

namespace terraria.Items;

public interface IItem : IHotbarEntry
{
    bool IsActive { get; }
}
