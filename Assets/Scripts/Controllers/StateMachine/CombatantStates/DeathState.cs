public class DeathState : State<CombatantState>
{
    private Combatant combatant;

    public DeathState(Combatant combatant, StateMachine<CombatantState> stateMachine) : base(stateMachine)
    {
        this.combatant = combatant;
    }

    public override void OnEnter()
    {
        // Send Death Event
        combatant.SetTarget(-1);
        stateMachine.SetActive(false);
    }

    public override void OnUpdate()
    {

    }
}
