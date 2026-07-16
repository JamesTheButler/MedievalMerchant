namespace Features.Tutorial.Onboarding.Data
{
    // Values are pinned explicitly and must never be reused or reordered:
    // they are serialized as-is in OnboardingResources.asset (explainerTexts keys).
    // Add new explainers with the next free value; never renumber existing ones.
    public enum OnboardingExplainer
    {
        Welcome = 0,
        IntroGoal = 1,
        HayMissionInstructions = 2,
        HayMissionComplete = 3,
        TownAUpgradeReady = 4,
        BerryPickerInstructions = 5,
        BerryPickerComplete = 6,
        GameSpeedInstructions = 7,
        BerryDeliveryInstructions = 8,
        CartUpgradeInstructions = 9,
        TownAUpgraded = 10,
        FindYourOwnFortune = 11,
        ClosingRemarks = 12,
        IntroCampsite = 13,
    }
}
