using UnityEngine;
using System.Collections.Generic;
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

    private Weapon weapon;
    public Weapon Weapon
    {
        get
        {
            return weapon;
        }
    }

    public void Initialize(int combatantId, CombatantConfigSO combatantConfig)
    {
        this.combatantId = combatantId;
        health = combatantConfig.Health;
        moveSpeed = combatantConfig.MoveSpeed;
        combatantName = combatantConfig.Name;
        spriteRenderer.sprite = combatantConfig.gameSprite;
        CreateWeapon(combatantConfig.weaponConfig);
        currentTargetId = -1;
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

        // TODO: (ADR)(POLISH) Add red animation        

        if (health <= 0)
        {
            currentState = CombatantState.Death;
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
        transform.rotation = Quaternion.Euler(0, 0, angle - 270);
    }

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
        stateTxt.text = currentState.ToString() + " Health: " + health.ToString();
    }
}
