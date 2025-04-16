using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using TMPro;

public class Combatant : MonoBehaviour
{
    [SerializeField]
    private Transform rotationPivot;

    [SerializeField]
    private SpriteRenderer spriteRenderer;

    [SerializeField]
    private TextMeshProUGUI nameTxt;

    [SerializeField]
    private Transform weaponContainer;

    [SerializeField]
    private Image healthBarFill;

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

    private float maxHealth;
    private float health;
    public float Health
    {
        get
        {
            return health;
        }
    }

    private float moveSpeed;
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

    private Weapon weapon;
    public Weapon Weapon
    {
        get
        {
            return weapon;
        }
    }

    private Sprite combatantSprite;
    public Sprite CombatantSprite
    {
        get
        {
            return combatantSprite;
        }
    }

    public void Initialize(int combatantId, CombatantConfigSO combatantConfig)
    {
        this.combatantId = combatantId;
        maxHealth = combatantConfig.Health;
        health = combatantConfig.Health;
        moveSpeed = combatantConfig.MoveSpeed;
        combatantName = combatantConfig.Name;
        combatantSprite = combatantConfig.gameSprite;
        CreateWeapon(combatantConfig.weaponConfig);
        currentTargetId = -1;

        nameTxt.text = combatantName;
        healthBarFill.fillAmount = 1f;
        spriteRenderer.sprite = combatantSprite;
    }

    private void CreateWeapon(WeaponConfigSO weaponConfig)
    {
        weapon = Instantiate(Resources.Load<Weapon>("Prefabs/Weapon"), weaponContainer.transform);
        weapon.Initialize(weaponConfig);
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

        UIUtils.UpdateHealthBar(healthBarFill, health, maxHealth);
        EventManager.Instance.SendEvent(EventId.ON_COMBATANT_DAMAGED_EVENT,
            new OnCombatantDamagedPayload()
            {
                CombatantId = combatantId,
                healthPercentage = health / maxHealth,
            });

        if (health <= 0 && currentState != CombatantState.Death)
        {
            currentState = CombatantState.Death;
            EventManager.Instance.SendEvent(EventId.ON_COMBATANT_DEAD_EVENT,
            new OnCombatantDeadPayload()
            {
                CombatantId = combatantId,
            });
            gameObject.SetActive(false);
        }
    }

    public void Shoot(Combatant target)
    {
        weapon.Fire(target.transform.position,
        (damage) =>
        {
            if (target.currentState != CombatantState.Death)
            {
                target.TakeDamage(damage);
            }
        });
    }

    public void FaceTarget(Vector3 targetVector)
    {
        Vector2 dir = (transform.position - targetVector).normalized;
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        angle += 90; // adjustment since sprite is looking to the north
        rotationPivot.transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
    }

    // DEBUGGING - DELETE ME =============
    Dictionary<int, Combatant> combatants;
    public void DebugPlayers(Dictionary<int, Combatant> combatants)
    {
        this.combatants = combatants;
    }

    private void Update()
    {
        if (currentTargetId != -1)
        {
            Debug.DrawLine(this.transform.position, combatants[currentTargetId].transform.position, Color.green);
        }
        nameTxt.text = currentState.ToString() + " Health: " + health.ToString();
    }
    // DEBUGGING - DELETE ME ================
}
