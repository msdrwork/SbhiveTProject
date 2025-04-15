using UnityEngine;

[CreateAssetMenu(fileName = "Bullet Config", menuName = "Game/Combat/Bullet Config")]
public class BulletConfigSO : ScriptableObject
{
    public float Damage;
    public float Speed;
}