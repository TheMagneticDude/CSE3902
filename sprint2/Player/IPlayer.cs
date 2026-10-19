using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using sprint2.Hotbar;
using sprint2.Weapons;
using sprint2.Collision;

namespace sprint2.Players;

public interface IPlayer
{
    public Vector2 Location { get; set; }
    public Vector2 Velocity { get; set; }
    public CapsuleCollider Hitbox { get; set; }
    public bool IsGrounded { get; set; }
    bool IsDead { get; }
    void TakeDamage(int damage);
    bool FacingRight { get; }
    int SelectedHotbarSlot { get; }
    IWeapon EquippedWeapon { get; }

    PlayerHotbar Hotbar { get; }
    IHotbarEntry SelectedHotbarItem { get; }

    void Update(GameTime gameTime, PlayerInput input);
    void Draw(SpriteBatch spriteBatch);

    void EquipWeapon(IWeapon weapon);
    void UnequipWeapon();
}
