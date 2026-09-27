using System.Collections;
using Common.Infrastructure.Gameplay;
using Common.Infrastructure.Observation;
using Common.UI.Elements;
using Features.Combat.Logic;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.UI;

namespace Features.Combat.UI
{
    public sealed class CombatScreenUI : InitializableBehavior
    {
        [SerializeField, Required]
        private TeamBattleUI playerBattle, banditBattle;

        [SerializeField, Required]
        private TeamSummaryUI playerSummary, banditSummary;

        [SerializeField, Required]
        private AdvantageBar advantageBar;

        [SerializeField, Required]
        private RectTransform swordLayer, outcomeParent;

        [SerializeField, Required]
        private SwordProjectile swordPrefab;

        [SerializeField, Required]
        private BattleOutcomeUI outcomePrefab;

        [SerializeField, Required]
        private Button nextRoundButton, autoButton;

        [SerializeField, Required]
        private GameObject autoActiveMarker;

        [SerializeField]
        private float swordFlightSeconds = 0.42f, swordStaggerSeconds = 0.03f, settleSeconds = 0.42f;

        private CombatService _combatService;
        private Logic.Combat _combat;
        private BattleOutcomeUI _outcome;
        private Coroutine _round;

        private int _pendingHits;
        private bool _auto, _isOver;

        private readonly Bindings _outcomeBindings = new();

        public override void Initialize()
        {
            _combatService = GameplayContext.Instance.Services.CombatService;

            nextRoundButton.onClick.AddListener(OnNextRoundClicked);
            autoButton.onClick.AddListener(OnAutoClicked);

            Hide();
        }

        public override void CleanUp()
        {
            base.CleanUp();

            StopRound();
            ClearOutcome();

            nextRoundButton.onClick.RemoveListener(OnNextRoundClicked);
            autoButton.onClick.RemoveListener(OnAutoClicked);
        }

        public void Show(Logic.Combat combat)
        {
            if (combat == null)
                return;

            _combat = combat;
            _isOver = false;

            playerBattle.Bind(combat.Player, combat.Player.Name);
            banditBattle.Bind(combat.Bandits, combat.Bandits.Name);

            playerSummary.Bind(combat.Player, combat.Player.Name);
            banditSummary.Bind(combat.Bandits, combat.Bandits.Name);

            advantageBar.Bind(combat);

            SetAuto(false);
            RefreshControls();

            gameObject.SetActive(true);
        }

        public void Hide()
        {
            StopRound();
            ClearOutcome();
            SetAuto(false);

            playerBattle.Unbind();
            banditBattle.Unbind();
            playerSummary.Unbind();
            banditSummary.Unbind();
            advantageBar.Unbind();

            _combat = null;
            gameObject.SetActive(false);
        }

        private void OnNextRoundClicked()
        {
            ResolveRound();
        }

        private void ResolveRound()
        {
            if (_combat == null || _isOver || _round != null)
                return;

            var result = _combatService.ResolveRound();

            if (result == null)
                return;

            _round = StartCoroutine(PlayRound(result));
        }

        private IEnumerator PlayRound(RoundResult result)
        {
            RefreshControls();

            _pendingHits = result.Attacks.Count;

            foreach (var attack in result.Attacks)
            {
                LaunchSword(attack);

                if (swordStaggerSeconds > 0f)
                {
                    yield return new WaitForSeconds(swordStaggerSeconds);
                }
            }

            while (_pendingHits > 0)
            {
                yield return null;
            }

            playerBattle.SyncAllTokens();
            banditBattle.SyncAllTokens();

            playerSummary.SetRoundDeltas(result.Guards);
            banditSummary.SetRoundDeltas(result.Bandits);

            playerSummary.Say(result.PlayerMood);
            banditSummary.Say(result.BanditMood);

            yield return new WaitForSeconds(settleSeconds);

            _round = null;
            _isOver = result.Status != CombatStatus.Ongoing;

            RefreshControls();

            if (_isOver)
            {
                ShowOutcome(result);
                yield break;
            }

            if (_auto)
            {
                ResolveRound();
            }
        }

        private void StopRound()
        {
            if (_round != null)
            {
                StopCoroutine(_round);
                _round = null;
            }

            _pendingHits = 0;
        }

        private void LaunchSword(Attack attack)
        {
            var from = TokenFor(attack.Attacker);
            var to = TokenFor(attack.Defender);

            if (from == null || to == null)
            {
                _pendingHits--;
                return;
            }

            var sword = Instantiate(swordPrefab, swordLayer);
            var damage = attack.Damage;

            sword.Throw(
                (RectTransform)from.transform,
                (RectTransform)to.transform,
                swordFlightSeconds,
                () => OnSwordLanded(to, damage));
        }

        private void OnSwordLanded(UnitToken target, float damage)
        {
            if (target != null)
            {
                target.ApplyHit(damage);
            }

            _pendingHits--;
        }

        private UnitToken TokenFor(CombatUnit unit)
        {
            var token = playerBattle.GetToken(unit);
            return token != null ? token : banditBattle.GetToken(unit);
        }

        private void ShowOutcome(RoundResult result)
        {
            ClearOutcome();

            _outcome = Instantiate(outcomePrefab, outcomeParent);
            _outcomeBindings.Track(_outcome.NextRequested.Observe(OnOutcomeNextRequested));

            _outcome.Show(result.Status, _combat.Player, _combat.Bandits, result.Round);
        }

        private void OnOutcomeNextRequested()
        {
            // TODO: hand off to the loot screen for this outcome. Its own UI, not this screen's.
            Hide();
        }

        private void ClearOutcome()
        {
            _outcomeBindings.Unbind();

            if (_outcome == null)
                return;

            Destroy(_outcome.gameObject);
            _outcome = null;
        }

        private void OnAutoClicked()
        {
            SetAuto(!_auto);

            if (_auto && _round == null)
            {
                ResolveRound();
            }
        }

        private void SetAuto(bool isOn)
        {
            _auto = isOn;
            autoActiveMarker.SetActive(isOn);
        }

        // Next Round is blocked while a round is playing out; Auto stays live so a run can be
        // called off mid-volley.
        private void RefreshControls()
        {
            nextRoundButton.interactable = !_isOver && _round == null;
            autoButton.interactable = !_isOver;
        }
    }
}