using System.Reflection;
using Microsoft.Xna.Framework;

namespace sprint2.Combat;

public interface ICombatEntity
{
    CombatTeam Team {get;}
    Rectangle Hitbox {get;}
    bool IsActive {get;}
    void TakeDamage(int damage);
}