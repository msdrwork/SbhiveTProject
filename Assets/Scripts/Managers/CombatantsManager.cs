using UnityEngine;
using System.Collections.Generic;

public class CombatantsManager : MonoBehaviour, IEventObserver
{
    [SerializeField]
    private List<CombatantConfigSO> combatantConfigs;

    [SerializeField]
    private Transform combatantsContainer;

    private Dictionary<int, Combatant> combatants;
    public Dictionary<int, Combatant> Combatants
    {
        get
        {
            return combatants;
        }
    }

    private Dictionary<int, StateMachine<CombatantState>> combatantStateMachines;

    private CombatConfiguration currentCombatConfig;

    private GameUpdateManager gameUpdateManager;

    public void Initialize(GameUpdateManager gameUpdateManager)
    {
        EventManager.Instance.AddEventListener(EventId.ON_COMBATANT_DEAD_EVENT, this);
        combatants = new Dictionary<int, Combatant>();
        combatantStateMachines = new Dictionary<int, StateMachine<CombatantState>>();
        this.gameUpdateManager = gameUpdateManager;
    }

    public void SetCombatConfiguration(CombatConfiguration combatConfig)
    {
        this.currentCombatConfig = combatConfig;
    }

    // TODO: (ADR) If theres time, convert this into a pool.
    public void LoadCombatants()
    {
        Combatant combatantPrefab = Resources.Load<Combatant>("Prefabs/Combatant");
        for (int i = 0; i < currentCombatConfig.CombatantCount; i++)
        {
            Combatant newCombatant = Instantiate(combatantPrefab, combatantsContainer);
            newCombatant.Initialize(i, combatantConfigs[Random.Range(0, combatantConfigs.Count)]);
            newCombatant.transform.position = new Vector2(Random.Range(0f, 10f), Random.Range(0f, 10f));
            EventManager.Instance.SendEvent(EventId.ON_COMBATANT_CREATED_EVENT,
                new OnCombatantCreatedPayload()
                {
                    Combatant = newCombatant,
                });
            combatants.Add(i, newCombatant);
        }
    }

    public void LoadCombatantAI()
    {
        for (int i = 0; i < combatants.Count; i++)
        {
            Combatant combatant = combatants[i];
            combatant.DebugPlayers(combatants);
            StateMachine<CombatantState> combatantStateMachine = new StateMachine<CombatantState>();
            combatantStateMachine.AddState(CombatantState.Idle, new IdleState(combatant, combatantStateMachine));
            combatantStateMachine.AddState(CombatantState.PickingTarget, new PickingTargetState(combatant, combatants, combatantStateMachine));
            combatantStateMachine.AddState(CombatantState.Tracking, new TrackingState(combatant, combatants, combatantStateMachine));
            combatantStateMachine.AddState(CombatantState.PrepareAttack, new PrepareAttackState(combatant, combatants, combatantStateMachine));
            combatantStateMachine.AddState(CombatantState.Shooting, new ShootingState(combatant, combatants, combatantStateMachine));
            combatantStateMachine.AddState(CombatantState.Reloading, new ReloadingState(combatant, combatantStateMachine));
            combatantStateMachine.AddState(CombatantState.Celebrating, new PickingTargetState(combatant, combatants, combatantStateMachine));
            combatantStateMachine.AddState(CombatantState.Death, new DeathState(combatant, combatantStateMachine));
            gameUpdateManager.SetUpdateable(combatantStateMachine);
            combatantStateMachines.Add(i, combatantStateMachine);
        }
    }

    public void ActivateCombatants()
    {
        for (int i = 0; i < combatantStateMachines.Keys.Count; i++)
        {
            StateMachine<CombatantState> combatantStateMachine = combatantStateMachines[i];
            combatantStateMachine.ChangeState(CombatantState.Idle);
            combatantStateMachine.SetActive(true);
        }
    }

    public void OnEvent(EventId eventId, object payload)
    {
        if (eventId == EventId.ON_COMBATANT_DEAD_EVENT)
        {
            int aliveCombantantCount = BattleUtils.GetAliveCombatantsCount(combatants);

            if (aliveCombantantCount == 1)
            {
                int winningCombatantId = BattleUtils.GetWinningCombatantId(combatants);

                if (winningCombatantId != -1)
                {
                    EventManager.Instance.SendEvent(EventId.ON_COMBATANT_WIN_EVENT,
                        new OnCombatantWinPayload()
                        {
                            Combatant = combatants[winningCombatantId],
                        });
                }
                else
                {
                    EventManager.Instance.SendEvent(EventId.ON_ALL_COMBATANT_DEAD_EVENT, null);
                }

            }
            else if (aliveCombantantCount <= 0)
            {
                EventManager.Instance.SendEvent(EventId.ON_ALL_COMBATANT_DEAD_EVENT, null);
            }
        }
    }

    public void Destroy()
    {
        EventManager.Instance.RemoveEventListener(EventId.ON_COMBATANT_DEAD_EVENT, this);
    }
}