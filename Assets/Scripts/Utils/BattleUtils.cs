using UnityEngine;
using System.Collections.Generic;

public static class BattleUtils
{
    // Finds the nearest Combatant Via Vector2
    public static int GetClosesCombatantId(Dictionary<int, Combatant> allCombatants, int combatantId)
    {
        int closesCombatantId = 0;
        Combatant currentCombatant = allCombatants[combatantId];
        Vector2 combatantPosition = currentCombatant.transform.position;
        float minDistance = float.MaxValue;

        for (int i = 0; i < allCombatants.Count; i++)
        {
            if (i == combatantId)
            {
                continue;
            }

            Combatant targetCombatant = allCombatants[i];

            if (targetCombatant.CurrentState != CombatantState.Death)
            {
                Vector2 possibleTargetVector = targetCombatant.transform.position;
                float distance = Vector2.Distance(combatantPosition, possibleTargetVector);
                if (distance < minDistance)
                {
                    minDistance = distance;
                    closesCombatantId = i;
                }
            }
        }

        return closesCombatantId;
    }

    // Finds a random combatant that is still alive
    public static int GetRandomCombatantId(Dictionary<int, Combatant> allCombatants, int combatantId)
    {
        int randomCombatantId = 0;
        List<Combatant> aliveCombatants = new List<Combatant>();

        for (int i = 0; i < allCombatants.Count; i++)
        {
            Combatant combatant = allCombatants[i];
            if (combatant.CurrentState != CombatantState.Death && combatant.CombatantId != combatantId)
            {
                aliveCombatants.Add(combatant);
            }
        }

        if (aliveCombatants.Count > 0)
        {
            int randIdx = Random.Range(0, aliveCombatants.Count);
            randomCombatantId = aliveCombatants[randIdx].CombatantId;
        }
        else
        {
            randomCombatantId = -1;
        }

        return randomCombatantId;
    }
}
