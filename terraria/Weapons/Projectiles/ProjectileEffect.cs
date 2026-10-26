namespace sprint2.Combat;
public readonly struct ProjectileEffect
{
    public StatusEffectType Type {get;}

    public float Duration {get;}

    public float Strength {get;}

    public ProjectileEffect(StatusEffectType type, float duration, float strength)
    {
        Type = type;
        Duration = duration;
        Strength = strength;
    }
}