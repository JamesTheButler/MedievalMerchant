namespace Features.Combat.Logic
{
    public sealed record CombatantDelta(int UnitsLost, float HealthLost, float CombatStrengthLost)
    {
        public static readonly CombatantDelta None = new(0, 0f, 0f);

        public bool IsEmpty => UnitsLost == 0 && HealthLost <= 0f && CombatStrengthLost <= 0f;
    }
}