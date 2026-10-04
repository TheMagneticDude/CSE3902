using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using sprint2.Players;

namespace sprint2.Hotbar;

public class PlayerHotbar
{
    private readonly IHotbarEntry[] _slots;

    public int Count
    {
        get { return _slots.Length; }
    }

    public int SelectedSlot { get; private set; }

    public IHotbarEntry SelectedEntry
    {
        get { return _slots[SelectedSlot]; }
    }

    public PlayerHotbar(IHotbarEntry[] slots)
    {
        if (slots == null || slots.Length == 0)
        {
            throw new ArgumentException("A hotbar needs at least one slot.", nameof(slots));
        }

        _slots = new IHotbarEntry[slots.Length];
        Array.Copy(slots, _slots, slots.Length);
        SelectedSlot = 0;
    }

    public IHotbarEntry GetEntry(int slot)
    {
        return _slots[slot];
    }

    public void UseSelected(IPlayer player)
    {
        SelectedEntry.Use(player);
    }

    public void UseSlot(int slot, IPlayer player)
    {
        if (slot < 0 || slot >= _slots.Length)
        {
            return;
        }

        SelectedSlot = slot;
        SelectedEntry.Use(player);
    }

    public void Update(GameTime gameTime, IPlayer player)
    {
        foreach (IHotbarEntry entry in _slots)
        {
            entry.Update(gameTime, player);
        }
    }

    public void Draw(SpriteBatch spriteBatch, IPlayer player)
    {
        foreach (IHotbarEntry entry in _slots)
        {
            entry.Draw(spriteBatch, player);
        }
    }
}
