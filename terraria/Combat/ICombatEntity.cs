using System.Reflection;
using Microsoft.Xna.Framework;

namespace terraria.Combat;

public interface ICombatEntity
{
    CombatTeam Team {get;}
    Rectangle Hitbox {get;}
    bool IsActive {get;}
    void TakeDamage(int damage);
}