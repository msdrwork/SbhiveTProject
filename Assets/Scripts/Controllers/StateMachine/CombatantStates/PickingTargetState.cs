using UnityEngine;
using System.Collections.Generic;

public class PickingTargetState : State<CombatantState>
{
    private Dictionary<int, Combatant> allCombatants;
    private Combatant combatant;

    private float elapsedTime;

    public PickingTargetState(Combatant combatant, Dictionary<int, Combatant> allCombatants, StateMachine<CombatantState> stateMachine) : base(stateMachine)
    {
        this.allCombatants = allCombatants;
        this.combatant = combatant;
    }

    public override void OnEnter()
    {
        elapsedTime = 0;
    }

    public override void OnUpdate()
    {
        if (combatant.CurrentState == CombatantState.Death)
        {
            combatant.SetState(CombatantState.Death);
            stateMachine.ChangeState(CombatantState.Death);
            return;
        }

        elapsedTime += Time.deltaTime;

        if (elapsedTime < 0.25f)
        {
            return;
        }

        //combatant.SetTarget(BattleUtils.GetClosesCombatantId(allCombatants, combatant.CombatantId));
        combatant.SetTarget(BattleUtils.GetRandomCombatantId(allCombatants, combatant.CombatantId));

        if (combatant.CurrentTargetId != -1)
        {
            combatant.FaceTarget(allCombatants[combatant.CurrentTargetId].transform.position);
            combatant.SetState(CombatantState.Tracking);
            stateMachine.ChangeState(CombatantState.Tracking);
        }
        else
        {
            combatant.SetState(CombatantState.Idle);
            stateMachine.ChangeState(CombatantState.Idle);
        }
    }
}
