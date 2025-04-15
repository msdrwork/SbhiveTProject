using UnityEngine;

[CreateAssetMenu(fileName = "Weapon Config", menuName = "Game/Combat/Weapon Config")]
public class WeaponConfigSO : ScriptableObject
{
    public Sprite WeaponSprite;

    // Shoots 1 bullet per 1 second; 
    public float AttackSpeed = 1f;
    public float Range = 3f;

    [SerializeField]
    public BulletConfigSO bulletConfig;
}
