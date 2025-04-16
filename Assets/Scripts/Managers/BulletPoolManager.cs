using UnityEngine;
using System.Collections.Generic;

public class BulletPoolManager : MonoBehaviour
{
    private static BulletPoolManager instance;
    public static BulletPoolManager Instance
    {
        get
        {
            if (instance == null)
            {
                GameObject managerObject = new GameObject("BulletPoolManager");
                instance = managerObject.AddComponent<BulletPoolManager>();
                instance.transform.SetParent(GameObject.Find("GameManager").transform);
            }

            return instance;
        }
    }

    [SerializeField]
    private int initialPoolSize = 15;
    private Bullet bulletPrefabRef;
    private List<Bullet> bulletPool;

    public void Initialize()
    {
        bulletPrefabRef = Resources.Load<Bullet>("Prefabs/Bullet");
        bulletPool = new List<Bullet>();
        CreateInitialBullets();
    }

    private void CreateInitialBullets()
    {
        for (int i = 0; i < initialPoolSize; i++)
        {
            Bullet createdBullet = Instantiate(bulletPrefabRef, transform);
            createdBullet.gameObject.SetActive(false);
            bulletPool.Add(createdBullet);
        }
    }

    public Bullet GetBullet()
    {
        for (int i = bulletPool.Count - 1; i >= 0; i--)
        {
            Bullet bullet = bulletPool[i];
            if (!bullet.gameObject.activeInHierarchy)
            {
                bullet.gameObject.SetActive(true);
                return bullet;
            }
        }

        Bullet createdBullet = Instantiate(bulletPrefabRef, transform);
        createdBullet.gameObject.SetActive(true);
        bulletPool.Add(createdBullet);
        return createdBullet;
    }

    public void ReturnBullet(Bullet bullet)
    {
        bullet.gameObject.SetActive(false);
        bullet.transform.SetParent(transform);
    }
}
