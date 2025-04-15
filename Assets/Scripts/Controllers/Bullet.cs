using UnityEngine;
using UnityEngine.Events;

public class Bullet : MonoBehaviour
{
    [SerializeField]
    private Rigidbody2D rigidBody;

    private float damage;
    public float Damage
    {
        get
        {
            return damage;
        }
    }

    private float speed;
    public float Speed
    {
        get
        {
            return speed;
        }
    }

    private UnityAction<float> onBulletHit;
    private Vector3 targetVector;

    private float lifeTime = 2f;

    public void Initialize(BulletConfigSO bulletConfig, Vector3 targetVector, UnityAction<float> onBulletHit)
    {
        damage = bulletConfig.Damage;
        speed = bulletConfig.Speed;
        this.targetVector = targetVector;
        this.onBulletHit = onBulletHit;        
        Destroy(gameObject, lifeTime);
    }

    public void FireBullet()
    {
        Vector2 dir = (targetVector - gameObject.transform.position).normalized;
        rigidBody.linearVelocity = dir * speed;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Combatant"))
        {
            onBulletHit?.Invoke(damage);
            Destroy(gameObject);
        }
    }
}
