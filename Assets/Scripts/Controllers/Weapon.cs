using UnityEngine;
using UnityEngine.Events;
using System.Collections.Generic;

public class Weapon : MonoBehaviour
{
    [SerializeField]
    private SpriteRenderer spriteRenderer;

    [SerializeField]
    private Transform bulletSpawnPoint;

    private Bullet bulletPrefabRef;
    private List<Bullet> bullets;

    private float attackSpeed;
    public float AttackSpeed
    {
        get
        {
            return attackSpeed;
        }
    }

    private float range;
    public float Range
    {
        get
        {
            return range;
        }
    }

    private bool isReloading;
    public bool IsReloading
    {
        get
        {
            return isReloading;
        }
    }

    private WeaponConfigSO weaponConfig;

    public void Initialize(WeaponConfigSO weaponConfig)
    {
        bullets = new List<Bullet>();
        attackSpeed = weaponConfig.AttackSpeed;
        range = weaponConfig.Range;
        spriteRenderer.sprite = weaponConfig.WeaponSprite;
        this.weaponConfig = weaponConfig;
    }

    public void Fire(Vector2 targetDir, UnityAction<float> onBulletHit)
    {
        bullets.Add(CreateBullet(targetDir, onBulletHit));
        isReloading = true;
    }

    public void Reload()
    {
        isReloading = false;
    }

    // TODO: (ADR) Pool if you have time
    private Bullet CreateBullet(Vector2 targetDir, UnityAction<float> onBulletHit)
    {
        if (bulletPrefabRef == null)
        {
            bulletPrefabRef = Resources.Load<Bullet>("Prefabs/Bullet");
        }

        Bullet createdBullet = Instantiate(bulletPrefabRef, bulletSpawnPoint);
        createdBullet.Initialize(weaponConfig.bulletConfig, targetDir, onBulletHit);
        createdBullet.FireBullet();
        return createdBullet;
    }
}
