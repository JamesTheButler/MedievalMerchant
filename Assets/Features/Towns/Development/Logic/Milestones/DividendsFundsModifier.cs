using Common.Infrastructure;
using Common.Infrastructure.Modifiable;

namespace Features.Towns.Development.Logic.Milestones
{
    public sealed class DividendsFundsModifier : FlatModifier
    {
        private readonly float _percentage;
        private readonly Town _town;

        public DividendsFundsModifier(float value, Town town)
            : base(value, GetDescription(value, town))
        {
            _percentage = value;
            _town = town;

            _town.FundsChange.Observe(OnFundsChangeChanged);
        }

        ~DividendsFundsModifier()
        {
            _town.FundsChange.StopObserving(OnFundsChangeChanged);
        }

        private void OnFundsChangeChanged(float fundsChange)
        {
            Value.Value = fundsChange * _percentage;
        }

        private static string GetDescription(float value, Town town)
        {
            var loc = ResourceManager.Instance.LocalizationResources.Town;
            return loc.DividendsFundsModifier(value, town.Name);
        }
    }
}