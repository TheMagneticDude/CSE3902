using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using terraria.Bosses;
using terraria.Collision;
using terraria.Players;
using terraria.Projectiles;
using IProjectile = terraria.Projectiles.IProjectile;
using terraria.Weapons;

namespace terraria.Combat;

public sealed class CombatManager
{
    private readonly ProjectileManager _projectiles;
    private readonly HashSet<IBoss> _swingHits = new();
    private IWeapon _weapon;
    private int _attackId;

    public CombatManager(ProjectileManager projectiles)
    {
        _projectiles = projectiles ?? throw new ArgumentNullException(nameof(projectiles));
    }

    public void Update(GameTime gameTime, IPlayer player, IReadOnlyList<IBoss> bosses,
        IReadOnlyList<CollisionSurface> worldMesh)
    {
        ArgumentNullException.ThrowIfNull(gameTime);
        ArgumentNullException.ThrowIfNull(player);
        ArgumentNullException.ThrowIfNull(bosses);
        ArgumentNullException.ThrowIfNull(worldMesh);

        _projectiles.Update(gameTime);
        IWeapon weapon = player.EquippedWeapon;
        // Each enemy can take damage once per swing.
        if (weapon != _weapon || weapon?.AttackId != _attackId)
        {
            _swingHits.Clear();
            _weapon = weapon;
            _attackId = weapon?.AttackId ?? 0;
        }

        foreach (IBoss boss in bosses)
        {
            if (boss.Health.IsDead)
            {
                continue;
            }

            if (!player.IsDead && weapon?.IsAttacking == true && weapon.Hitbox != null
                && !_swingHits.Contains(boss) && weapon.Hitbox.Intersects(boss.Hitbox))
            {
                _swingHits.Add(boss);
                boss.Health.TakeDamage(weapon.Damage);
            }
            if (!boss.Health.IsDead && !player.IsDead && boss.Hitbox.Intersects(player.Hitbox))
            {
                player.TakeDamage(boss.ContactDamage);
            }
        }

        foreach (IProjectile projectile in _projectiles.Projectiles)
        {
            if (!projectile.IsActive || projectile.Hitbox == null)
            {
                continue;
            }

            foreach (CollisionSurface surface in worldMesh)
            {
                if (!projectile.Hitbox.Intersects(surface))
                {
                    continue;
                }

                projectile.Destroy();
                break;
            }
            if (!projectile.IsActive)
            {
                continue;
            }

            foreach (IBoss boss in bosses)
            {
                if (boss.Health.IsDead || !projectile.Hitbox.Intersects(boss.Hitbox))
                {
                    continue;
                }

                boss.Health.TakeDamage(projectile.Damage);
                projectile.Destroy();
                break;
            }
        }
        _projectiles.RemoveInactive();
    }

    public void Clear()
    {
        _projectiles.Clear();
        _swingHits.Clear();
        _weapon = null;
        _attackId = 0;
    }
}
