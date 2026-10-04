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

    public WeaponHotbarEntry(string name, IWeapon weapon, bool attackWhenSelected)
    {
        Name = name;
        _weapon = weapon;
        _attackWhenSelected = attackWhenSelected;
    }

    public void Use(IPlayer player)
    {
        player.EquipWeapon(_weapon);

        if (_attackWhenSelected)
        {
            _weapon.Attack(player.Location, player.FacingRight);
        }
    }

    public void Update(GameTime gameTime, IPlayer player)
    {
        // Player updates the currently equipped weapon.
    }

    public void Draw(SpriteBatch spriteBatch, IPlayer player)
    {
        // Player draws the currently equipped weapon.
    }
}
