using System.Collections.Generic;

public class PrepareAttackState : State<CombatantState>
{
    private Combatant combatant;
    private Dictionary<int, Combatant> allCombatants;
    private Combatant targetCombatant;

    public PrepareAttackState(Combatant combatant, Dictionary<int, Combatant> allCombatants, StateMachine<CombatantState> stateMachine) : base(stateMachine)
    {
        this.combatant = combatant;
        this.allCombatants = allCombatants;
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

        if (targetCombatant.CurrentState == CombatantState.Death)
        {
            combatant.SetState(CombatantState.Idle);
            stateMachine.ChangeState(CombatantState.Idle);
        }
        else if (combatant.Weapon.IsReloading)
        {
            combatant.SetState(CombatantState.Reloading);
            stateMachine.ChangeState(CombatantState.Reloading);
        }
        else
        {
            combatant.SetState(CombatantState.Shooting);
            stateMachine.ChangeState(CombatantState.Shooting);
        }
    }

    public override void OnUpdate()
    {

    }
}
