using System.Drawing;
using sprint2.Combat;

public abstract class CombatEntity : ICombatEntity
{
    public int Health {get; protected set;}

    public CombatTeam Team {get; protected set;}

    public bool IsAlive
    {
        get
        {
            return Health > 0;
        }
    }

    //Need to add later public abstract Rectangle Hitbox {get;}

    protected CombatEntity(int health, CombatTeam team)
    {
        Health = health;
        Team = team;
    }

    public virtual void TakeDamage(int damage)
    {
        Health -= damage;
        if (Health < 0)
        {
            Health = 0;
        }
    }
}
