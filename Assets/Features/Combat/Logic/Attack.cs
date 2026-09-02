namespace Features.Combat.Logic
{
    public sealed record Attack(CombatUnit Attacker, CombatUnit Defender, float Damage);
}
