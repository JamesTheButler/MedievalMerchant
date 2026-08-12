namespace Features.Combat
{
    public sealed record Attack(CombatUnit Attacker, CombatUnit Defender, float HitFactor);
}