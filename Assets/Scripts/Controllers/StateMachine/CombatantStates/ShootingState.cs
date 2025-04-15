using System.Collections.Generic;
using UnityEngine;

public class ShootingState : State<CombatantState>
{
    private Dictionary<int, Combatant> allCombatants;
    private Combatant combatant;
    private Combatant targetCombatant;

    public ShootingState(Combatant combatant, Dictionary<int, Combatant> allCombatants, StateMachine<CombatantState> stateMachine) : base(stateMachine)
    {
        this.allCombatants = allCombatants;
        this.combatant = combatant;
    }

    public override void OnEnter()
    {
        if (combatant.CurrentState == CombatantState.Death)
        {
            combatant.SetState(CombatantState.Death);
            stateMachine.ChangeState(CombatantState.Death);
            return;
        }

        targetCombatant = allCombatants[combatant.CurrentTargetId];
        combatant.FaceTarget(targetCombatant.transform.position);
        combatant.Shoot(targetCombatant);
        combatant.SetState(CombatantState.PrepareAttack);
        stateMachine.ChangeState(CombatantState.PrepareAttack);

    }

    public override void OnUpdate()
    {
    }
}
