using UnityEngine;

public class IdleState : State<CombatantState>
{
    private Combatant combatant;

    private float elapsedTime;

    public IdleState(Combatant combatant, StateMachine<CombatantState> stateMachine) : base(stateMachine)
    {
        this.combatant = combatant;
    }

    public override void OnEnter()
    {
        combatant.SetTarget(-1);
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

        combatant.SetState(CombatantState.PickingTarget);
        stateMachine.ChangeState(CombatantState.PickingTarget);
    }
}
