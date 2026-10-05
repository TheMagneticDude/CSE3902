using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGameLibrary;
using MonoGameLibrary.Graphics;
using sprint2.Hotbar;
using sprint2.Weapons;
using sprint2.Combat;


namespace sprint2.Players;


public interface IPlayer
{
    public Vector2 Location { get; set; }
    public Vector2 Velocity {get; set;}    
    bool FacingRight { get; }
    int SelectedHotbarSlot { get; }
    IHotbarEntry SelectedHotbarItem { get; }
    
    void Update(GameTime gameTime, PlayerInput input);
    void Draw(SpriteBatch spriteBatch);

    void EquipWeapon(IWeapon weapon);
    void UnequipWeapon();
}
