using UnityEngine;

[CreateAssetMenu(fileName = "Combatant Config", menuName = "Game/Combat/Combatant Config")]
public class CombatantConfigSO : ScriptableObject
{
    public Sprite gameSprite;    
    public string Name;
    public float MoveSpeed;
    public float Health;

    [SerializeField]
    public WeaponConfigSO weaponConfig;
}
