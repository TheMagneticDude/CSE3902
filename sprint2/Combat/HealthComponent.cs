using System;
using Microsoft.Xna.Framework;

namespace sprint2.Combat;
public class HealthComponent
{
    private readonly float _slowDuration;
    private readonly float _invulnerabilityDuration;
    private float _slowTimer;
    private float _invulnerabilityTimer;
    public int CurrentHealth { get; private set; }
    public int MaxHealth { get; private set; }
    public bool IsDead { get; private set; }
    public bool Slow { get; set; }
    public bool IsInvulnerable { get; set; } = false;

    public HealthComponent(int maxHealth, float slowDuration, float invulnerabilityDuration = 0.75f)
    {
        if (maxHealth <= 0)
        {
            throw new ArgumentException("Max health must be greater than zero.");
        }
        if (slowDuration < 0)
        {
            throw new ArgumentException("Slow duration must be a non-negative value.");
        }
        if (invulnerabilityDuration < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(invulnerabilityDuration));
        }
        _invulnerabilityDuration = invulnerabilityDuration;
        MaxHealth = maxHealth;
        CurrentHealth = maxHealth;
        IsDead = false;
        Slow = false;
        _slowDuration = slowDuration;
        _slowTimer = 0f;
    }

    public bool TakeDamage(int damage)
    {
        if (damage<=0 || IsDead || IsInvulnerable)
        {
            return false;
        }
        CurrentHealth -= damage;
        if (CurrentHealth <= 0)
        {
            CurrentHealth = 0;
            IsDead = true;
        }
        IsInvulnerable = _invulnerabilityDuration > 0f;
        _invulnerabilityTimer = _invulnerabilityDuration;
        return true;
    }

    public void Heal(int amount)
    {
        if (amount <= 0 || IsDead)
        {
            return;
        }
        CurrentHealth += Math.Min(amount, MaxHealth - CurrentHealth);
    }

    public void Update(GameTime gameTime)
    {
        ArgumentNullException.ThrowIfNull(gameTime);

        if (IsInvulnerable)
        {
            _invulnerabilityTimer -= (float)gameTime.ElapsedGameTime.TotalSeconds;

            if (_invulnerabilityTimer <= 0f)
            {
                IsInvulnerable = false;
            }
        }

        if (Slow)
        {
            _slowTimer += (float)gameTime.ElapsedGameTime.TotalSeconds;
            if (_slowTimer >= _slowDuration)
            {
                Slow = false;
                _slowTimer = 0f;
            }
        }
    }

    public void Reset()
    {
        CurrentHealth = MaxHealth;
        IsDead = false;
        Slow = false;
        _slowTimer = 0f;
        _invulnerabilityTimer = 0f;
        IsInvulnerable = false;
    }
}