using UnityEngine;
using System.Collections.Generic;

public class TrackingState : State<CombatantState>
{
    private Combatant combatant;
    private Dictionary<int, Combatant> allCombatants;
    private Combatant targetCombatant;

    public TrackingState(Combatant combatant, Dictionary<int, Combatant> allCombatants, StateMachine<CombatantState> stateMachine) : base(stateMachine)
    {
        this.combatant = combatant;
        this.allCombatants = allCombatants;
    }

    public override void OnEnter()
    {
        targetCombatant = allCombatants[combatant.CurrentTargetId];
    }

    public override void OnUpdate()
    {
        if (combatant.CurrentState == CombatantState.Death)
        {
            combatant.SetState(CombatantState.Death);
            stateMachine.ChangeState(CombatantState.Death);
            return;
        }

        if (targetCombatant.CurrentState == CombatantState.Death)
        {
            // Find new target immediatelly 
            combatant.SetTarget(BattleUtils.GetRandomCombatantId(allCombatants, combatant.CombatantId));
            if (combatant.CurrentTargetId != -1)
            {
                targetCombatant = allCombatants[combatant.CurrentTargetId];
            }
            else
            {
                combatant.SetState(CombatantState.Idle);
                stateMachine.ChangeState(CombatantState.Idle);
            }
        }

        combatant.FaceTarget(targetCombatant.transform.position);
        combatant.transform.position = Vector2.MoveTowards(combatant.transform.position, targetCombatant.transform.position, Time.deltaTime * combatant.MoveSpeed);     
        
        if (Vector2.Distance(combatant.transform.position, targetCombatant.transform.position) <= combatant.Weapon.Range)
        {
            combatant.SetState(CombatantState.PrepareAttack);
            stateMachine.ChangeState(CombatantState.PrepareAttack);
        }
    }
}
