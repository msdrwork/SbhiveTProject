public enum EventId
{
    TEST_EVENT,
    
    ON_START_GAME_EVENT,            // OnStartGamePayload: When the sim is triggered to begin
    ON_RESET_GAME_EVENT,            // null: Activated when the game reset button is triggered

    // Combatant Events
    ON_COMBATANT_CREATED_EVENT,     // OnCombatantCreatedPayload: When a combatant is spawned
    ON_COMBATANT_DAMAGED_EVENT,     // OnCombatantDamagedPayload: When a Combatant Is Hit 
    ON_COMBATANT_DEAD_EVENT,        // OnCombatantDeadPayload: When a Combatant enter's Death state.
    ON_COMBATANT_WIN_EVENT,         // OnCombatantWinPayload: When theres only 1 alive left

    ON_ALL_COMBATANT_DEAD_EVENT,    // null: when every combatant died
}
