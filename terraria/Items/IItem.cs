using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using sprint2.Hotbar;
using sprint2.Players;

namespace sprint2.Items;

public interface IItem : IHotbarEntry
{
    bool IsActive { get; }
}
