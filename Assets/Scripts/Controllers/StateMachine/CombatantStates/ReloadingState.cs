using UnityEngine;

public class ReloadingState : State<CombatantState>
{
    private Combatant combatant;

    private float elapsedTime;

    public ReloadingState(Combatant combatant, StateMachine<CombatantState> stateMachine) : base(stateMachine)
    {
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

        if (elapsedTime >= 1f / combatant.Weapon.AttackSpeed)
        {
            combatant.Weapon.Reload();
            combatant.SetState(CombatantState.PrepareAttack);
            stateMachine.ChangeState(CombatantState.PrepareAttack);
        }
    }
}
