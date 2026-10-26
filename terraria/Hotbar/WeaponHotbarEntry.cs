using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using sprint2.Players;
using sprint2.Weapons;

namespace sprint2.Hotbar;

public class WeaponHotbarEntry : IHotbarEntry
{
    private readonly IWeapon _weapon;
    private readonly bool _attackWhenSelected;

    public string Name { get; }

    public Texture2D Icon
    {
        get { return _weapon.Texture; }
    }

    public WeaponHotbarEntry(string name, IWeapon weapon, bool attackWhenSelected)
    {
        Name = name;
        _weapon = weapon;
        _attackWhenSelected = attackWhenSelected;
    }

    public void Use(IPlayer player)
    {
        ArgumentNullException.ThrowIfNull(player);
        player.EquipWeapon(_weapon);

        if (_attackWhenSelected)
        {
            _weapon.Attack(player.Location, player.FacingRight);
        }
    }

    public void Select(IPlayer player)
    {
        ArgumentNullException.ThrowIfNull(player);
        player.EquipWeapon(_weapon);
    }

    public void Update(GameTime gameTime, IPlayer player)
    {
        // Player updates the currently equipped weapon.
    }

    public void Draw(SpriteBatch spriteBatch, IPlayer player)
    {
        // Player draws the currently equipped weapon.
    }

    public void DrawIcon(SpriteBatch spriteBatch, Rectangle slot)
    {
        ArgumentNullException.ThrowIfNull(spriteBatch);
        Texture2D texture = _weapon.Texture;

        float scale = Math.Min((float)(slot.Width - 12) / texture.Width, (float)(slot.Height - 12) / texture.Height);

        spriteBatch.Draw(
            texture,
            new Vector2(slot.Center.X, slot.Center.Y),
            null,
            Color.White,
            0f,
            new Vector2(texture.Width / 2f, texture.Height / 2f),
            scale,
            SpriteEffects.None,
            0f);
    }
}
