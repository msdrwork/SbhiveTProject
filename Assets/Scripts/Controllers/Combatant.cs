using UnityEngine;
using TMPro;

public class Combatant : MonoBehaviour
{
    [SerializeField]
    private SpriteRenderer spriteRenderer;

    [SerializeField]
    private TextMeshProUGUI stateTxt;

    [SerializeField]
    private Transform weaponContainer;

    private CombatantState currentState;
    public CombatantState CurrentState
    {
        get
        {
            return currentState;
        }
    }

    private int combatantId;
    public int CombatantId
    {
        get
        {
            return combatantId;
        }

    }

    private int currentTargetId;
    public int CurrentTargetId
    {
        get
        {
            return currentTargetId;
        }
    }

    private float health = 3f;
    public float Health
    {
        get
        {
            return health;
        }
    }

    private float moveSpeed = 1f;
    public float MoveSpeed
    {
        get
        {
            return moveSpeed;
        }
    }

    private string combatantName;
    public string CombatantName
    {
        get
        {
            return combatantName;
        }
    }

    public void Initialize(int combatantId, CombatantConfigSO combatantConfig)
    {
        this.combatantId = combatantId;
        health = combatantConfig.Health;
        moveSpeed = combatantConfig.MoveSpeed;
        combatantName = combatantConfig.Name;
        spriteRenderer.sprite = combatantConfig.gameSprite;
        currentTargetId = -1;
    }

    public void SetTarget(int targetId)
    {
        this.currentTargetId = targetId;
    }

    public void SetState(CombatantState combatantState)
    {
        currentState = combatantState;
    }

    public void TakeDamage(float damage)
    {
        health -= damage;

        // TODO: (ADR)(POLISH) Add red animation
        Debug.Log("DAMAGED! remaining health: " + health);

        if (health <= 0)
        {
            currentState = CombatantState.Death;
        }
    }

    public void Shoot(Combatant target)
    {
        // Add Shoot Here
    }
}
