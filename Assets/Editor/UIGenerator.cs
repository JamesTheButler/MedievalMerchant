using System.Collections.Generic;
using Features.Combat.UI;
using TMPro;
using UnityEditor;
using UnityEngine;
using static Editor.UIGenerationHelper;

namespace Editor
{
    /// <summary>
    /// Accessed by claude to convert design pages from other tools into Unity uGUI.
    /// Driven by the `ui-development` skill: it writes builder methods here, the user runs
    /// the menu item below, and Unity's own API produces the prefabs.
    ///
    /// Builder methods are single-use. Old ones are deleted outright when a new UI is
    /// generated - the prefabs they produced stay on disk and are hand-tuned from there.
    ///
    /// Everything reusable lives in <see cref="UIGenerationHelper"/> and is imported above
    /// with `using static`, so the helper vocabulary reads unqualified here. A new layout
    /// need is a reason to add a helper there, not to inline the same six lines three times.
    /// </summary>
    public static class UIGenerator
    {
        // ----------------------------------------------------------------- per run
        // Point this at the UI folder of the feature currently being generated, e.g.
        // "Assets/Features/Towns/UI". It is also where NewElement<T>() looks for the
        // feature's existing element prefabs.

        private const string OutputFolder = "Assets/Features/Combat/UI";

        private const string CombatTable = "Combat";
        private const string CommonTable = "Common";

        // Combat screen sizes, from the Combat Screen blockout: a 560 side panel with 28 of
        // padding, and a unit token that is never scaled.
        private const int PanelContentWidth = 504;
        private const int TokenSize = 68;

        [MenuItem("Tools/AI/Generate UI Prefabs")]
        public static void Generate()
        {
            TargetFolder = OutputFolder;

            var built = new List<string>
            {
                BuildTeamBattleUI(),
                BuildTeamSummaryUI(),
                BuildAdvantageBar(),
                BuildBattleOutcomeUI(),
            };

            EnsureUnwiredEntries();

            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();

            Debug.Log(built.Count == 0
                ? "No builder methods registered - add them to Generate()."
                : "UI prefabs generated:\n - " + string.Join("\n - ", built));
        }

        // ----------------------------------------------------------------- combat sections
        // Layer 2 of the combat screen. The layer-1 element prefabs already sit in the output
        // folder and are instantiated by class name via NewElement<T>.

        private static string BuildTeamBattleUI()
        {
            var root = NewColumn("TeamBattleUI", PaddingLarge, TextAnchor.MiddleCenter);

            var header = NewRow("Header", PaddingLarge, TextAnchor.MiddleCenter);
            header.transform.SetParent(root.transform, false);

            var partyName = NewText("PartyName", header, 24f);
            partyName.text = "Guard";
            SetTextStyle(partyName, StyleTitle);

            var tierIcon = NewIcon("TierIcon", header, DefaultIconSize);
            var count = NewElement<UnitCountRow>("UnitCountRow", header);

            var rows = NewColumn("Rows", 0f, TextAnchor.MiddleCenter);
            rows.transform.SetParent(root.transform, false);

            var topRow = NewRow("TopRow", PaddingLarge, TextAnchor.MiddleCenter);
            topRow.transform.SetParent(rows.transform, false);

            var bottomRow = NewRow("BottomRow", PaddingLarge, TextAnchor.MiddleCenter);
            bottomRow.transform.SetParent(rows.transform, false);

            // The second rank sits half a token over, so the formation does not read as a grid.
            SetPaddingLeft(bottomRow, TokenSize / 2);

            var component = AddBehaviour<TeamBattleUI>(root);

            Assign(component, new Dictionary<string, Object>
            {
                { "partyNameText", partyName },
                { "tierIcon", tierIcon },
                { "unitCountRow", count },
                { "topRow", topRow.transform },
                { "bottomRow", bottomRow.transform },
                { "unitTokenPrefab", LoadElement<UnitToken>() },
            });

            return Save(root);
        }

        private static string BuildTeamSummaryUI()
        {
            var root = NewColumn("TeamSummaryUI", PaddingMedium);
            FixedWidth(root, PanelContentWidth);

            var card = NewPanel("PartyCard", root, PanelKind.Background);
            var cardInner = NewPanel("Inner", card, PanelKind.Foreground);
            Stretch(cardInner, PaddingSmall);

            var body = NewColumn("Body", PaddingMedium);
            body.transform.SetParent(cardInner.transform, false);
            SetPadding(body, PaddingLarge);

            var titleRow = NewRow("TitleRow", PaddingLarge);
            titleRow.transform.SetParent(body.transform, false);

            var partyName = NewText("PartyName", titleRow, 38f);
            partyName.text = "Guard";
            SetTextStyle(partyName, StyleTitle);

            var tierIcon = NewIcon("TierIcon", titleRow, DefaultIconSize);

            var count = NewElement<UnitCountRow>("UnitCountRow", body);
            var strength = NewElement<ModifiedStatRow>("CombatStrengthRow", body);
            var health = NewElement<ModifiedStatRow>("HealthRow", body);

            var divider = NewPanel("Divider", body, PanelKind.Foreground);
            SetSize(divider, PanelContentWidth - 2 * PaddingLarge, 3);

            NewLocalizedText(
                "TotalsLabel", body,
                table: CombatTable,
                key: "Combat.Summary.Totals.Label",
                english: "Totals",
                comment: "Caption above the party's combined health and combat strength, on the combat screen's side panel.",
                styleHashCode: StyleSubtitle);

            var totalsRow = NewRow("TotalsRow", 26f);
            totalsRow.transform.SetParent(body.transform, false);

            var totalHealth = NewElement<TotalStatRow>("TotalHealthRow", totalsRow);
            var totalStrength = NewElement<TotalStatRow>("TotalCombatStrengthRow", totalsRow);

            var lineBox = NewPanel("CommanderLine", root, PanelKind.Background);
            FixedWidth(lineBox, PanelContentWidth);

            var lineInner = NewPanel("Inner", lineBox, PanelKind.Foreground);
            Stretch(lineInner, PaddingSmall);

            var commanderLine = NewText("CommanderLineText", lineInner, 26f, FontStyles.Italic);
            commanderLine.text = "“Hold the line!”";
            SetTextStyle(commanderLine, StyleSubtitle);
            Stretch(commanderLine.gameObject, PaddingLarge);

            var component = AddBehaviour<TeamSummaryUI>(root);

            Assign(component, new Dictionary<string, Object>
            {
                { "partyNameText", partyName },
                { "commanderLineText", commanderLine },
                { "tierIcon", tierIcon },
                { "unitCountRow", count },
                { "combatStrengthRow", strength },
                { "healthRow", health },
                { "totalHealthRow", totalHealth },
                { "totalCombatStrengthRow", totalStrength },
            });

            return Save(root);
        }

        private static string BuildAdvantageBar()
        {
            var root = NewColumn("AdvantageBar", PaddingMedium);
            FixedWidth(root, PanelContentWidth);

            var labels = NewRow("Labels", PaddingLarge);
            labels.transform.SetParent(root.transform, false);

            var leftName = NewText("LeftName", labels, 24f);
            leftName.text = "Guards";
            SetTextStyle(leftName, StyleSubtitle);

            NewLocalizedText(
                "Title", labels,
                table: CombatTable,
                key: "Combat.Advantage.Header.Title",
                english: "Advantage",
                comment: "Header over the bar showing which side is winning, between the two party cards.",
                styleHashCode: StyleSubtitle);

            var rightName = NewText("RightName", labels, 24f);
            rightName.text = "Bandits";
            SetTextStyle(rightName, StyleSubtitle);

            var bar = NewPanel("Bar", root, PanelKind.BackgroundSquare);
            SetSize(bar, PanelContentWidth, 22);

            var fill = NewFillImage("Fill", bar);

            var caption = NewText("Caption", root, 24f);
            caption.text = "Evenly matched";
            caption.alignment = TextAlignmentOptions.Center;
            SetTextStyle(caption, StyleTitle);

            var component = AddBehaviour<AdvantageBar>(root);

            Assign(component, new Dictionary<string, Object>
            {
                { "fill", fill },
                { "leftNameText", leftName },
                { "rightNameText", rightName },
                { "captionText", caption },
            });

            AssignLocalizedString(
                component, "mutualDestructionCaption",
                table: CombatTable,
                key: "Combat.Advantage.Caption.MutualDestruction",
                english: "Mutual destruction",
                comment: "Reading under the advantage bar when both sides have been wiped out.");

            AssignLocalizedString(
                component, "evenlyMatchedCaption",
                table: CombatTable,
                key: "Combat.Advantage.Caption.EvenlyMatched",
                english: "Evenly matched",
                comment: "Reading under the advantage bar when neither side is meaningfully ahead.");

            AssignLocalizedString(
                component, "slightlyAheadCaption",
                table: CombatTable,
                key: "Combat.Advantage.Caption.SlightlyAhead",
                english: "{Side} slightly ahead",
                comment: "Reading under the advantage bar. {Side} is the winning party's name, e.g. Guards or Bandits.",
                isSmart: true);

            AssignLocalizedString(
                component, "aheadCaption",
                table: CombatTable,
                key: "Combat.Advantage.Caption.Ahead",
                english: "{Side} ahead",
                comment: "Reading under the advantage bar, one band stronger than SlightlyAhead. {Side} is the winning party's name.",
                isSmart: true);

            AssignLocalizedString(
                component, "dominatingCaption",
                table: CombatTable,
                key: "Combat.Advantage.Caption.Dominating",
                english: "{Side} dominating",
                comment: "Reading under the advantage bar, the strongest band. {Side} is the winning party's name.",
                isSmart: true);

            return Save(root);
        }

        private static string BuildBattleOutcomeUI()
        {
            var root = NewColumn("BattleOutcomeUI", 0f, TextAnchor.MiddleCenter);

            var card = NewPanel("Card", root, PanelKind.Background);
            var cardInner = NewPanel("Inner", card, PanelKind.Foreground);
            Stretch(cardInner, PaddingSmall);

            var body = NewColumn("Body", PaddingLarge, TextAnchor.UpperCenter);
            body.transform.SetParent(cardInner.transform, false);
            SetPadding(body, 40);

            var roundLabel = NewText("RoundLabel", body, 24f);
            roundLabel.text = "Round 8 · battle resolved";
            SetTextStyle(roundLabel, StyleSubtitle);

            var title = NewText("Title", body, 76f);
            title.text = "Victory";
            SetTextStyle(title, StyleTitle);

            var tiles = NewRow("LossTiles", 18f);
            tiles.transform.SetParent(body.transform, false);

            var playerTile = NewElement<OutcomeLossTile>("PlayerLossTile", tiles);
            var banditTile = NewElement<OutcomeLossTile>("BanditLossTile", tiles);

            var detail = NewText("Detail", body, 26f);
            detail.text = "The bandit group is disbanded.";
            detail.alignment = TextAlignmentOptions.Center;
            SetTextStyle(detail, StyleTitle);

            var nextButton = NewButton(
                "NextButton", body,
                table: CombatTable,
                key: "Combat.Outcome.Next.Button",
                english: "Next",
                comment: "Button on the battle outcome card that moves on to the loot screen.");

            var component = AddBehaviour<BattleOutcomeUI>(root);

            Assign(component, new Dictionary<string, Object>
            {
                { "roundLabelText", roundLabel },
                { "titleText", title },
                { "detailText", detail },
                { "playerLossTile", playerTile },
                { "banditLossTile", banditTile },
                { "nextButton", nextButton },
            });

            AssignLocalizedString(
                component, "roundLabelString",
                table: CombatTable,
                key: "Combat.Outcome.RoundLabel",
                english: "Round {_int_Round} · battle resolved",
                comment: "Small line above the outcome title. {_int_Round} is how many rounds the battle lasted.",
                isSmart: true);

            AssignLocalizedString(
                component, "playerSideHeader",
                table: CombatTable,
                key: "Combat.Outcome.LossTile.PlayerHeader",
                english: "Guards lost",
                comment: "Header on the outcome tile counting the player's own casualties.");

            AssignLocalizedString(
                component, "banditSideHeader",
                table: CombatTable,
                key: "Combat.Outcome.LossTile.BanditHeader",
                english: "Bandits lost",
                comment: "Header on the outcome tile counting the bandits' casualties.");

            AssignLocalizedString(
                component, "victoryTitle",
                table: CombatTable,
                key: "Combat.Outcome.Title.Victory",
                english: "Victory",
                comment: "Outcome card headline when the player's guards win the battle.");

            AssignLocalizedString(
                component, "defeatTitle",
                table: CombatTable,
                key: "Combat.Outcome.Title.Defeat",
                english: "Defeat",
                comment: "Outcome card headline when the player's guards are wiped out.");

            AssignLocalizedString(
                component, "drawTitle",
                table: CombatTable,
                key: "Combat.Outcome.Title.Draw",
                english: "Draw",
                comment: "Outcome card headline when both sides are wiped out in the same round.");

            AssignLocalizedString(
                component, "victoryDetail",
                table: CombatTable,
                key: "Combat.Outcome.Detail.Victory",
                english: "The bandit group is disbanded.",
                comment: "One-line consequence under the Victory headline on the outcome card.");

            AssignLocalizedString(
                component, "defeatDetail",
                table: CombatTable,
                key: "Combat.Outcome.Detail.Defeat",
                english: "The raid resumes where it left off.",
                comment: "One-line consequence under the Defeat headline on the outcome card.");

            AssignLocalizedString(
                component, "drawDetail",
                table: CombatTable,
                key: "Combat.Outcome.Detail.Draw",
                english: "The group is disbanded, with no loot to take.",
                comment: "One-line consequence under the Draw headline on the outcome card.");

            return Save(root);
        }

        // Entries whose LocalizedString field lives on a ScriptableObject rather than a prefab.
        // The entry is created here; the field is pointed at it by hand in the inspector.
        private static void EnsureUnwiredEntries()
        {
            EnsureEntry(
                CommonTable,
                "Common.Quote",
                "“{Line}”",
                "Wraps a spoken line in quotation marks. Replace the marks with the ones your " +
                "language uses - German „ “, French « », and so on. {Line} is the line itself. " +
                "Reached from code through LocalizationResources.Quote(string).",
                isSmart: true);

            EnsureEntry(
                CombatTable,
                "Combat.Outcome.LossTile.OutOf",
                "of {_int_MaxUnitCount}",
                "Suffix under a casualty count on the outcome card, e.g. 3 over \"of 10\". " +
                "{_int_MaxUnitCount} is how many units that side started with.",
                isSmart: true);
        }
    }
}
