using System.Collections.Generic;
using UnityEngine;

public class BulletPoolScript : MonoBehaviour
{
    public GameObject bulletPrefab; // Mermi objesinin prefabı
    public int poolSize = 10; // Havuz boyutu

    private Queue<GameObject> bulletPool = new Queue<GameObject>(); // Mermi havuzu

    // Başlangıçta havuzu doldur
    private void Start()
    {
        InitializePool();
    }

    // Havuzu doldur
    private void InitializePool()
    {
        for (int i = 0; i < poolSize; i++)
        {
            GameObject bullet = Instantiate(bulletPrefab, transform.position, Quaternion.identity);
            bullet.SetActive(false);
            bulletPool.Enqueue(bullet);
        }
    }

    // Havuzdan mermi al
    public GameObject GetBullet()
    {
        if (bulletPool.Count == 0)
        {
            Debug.LogWarning("Bullet pool is empty. Consider increasing the pool size.");
            return null;
        }

        GameObject bullet = bulletPool.Dequeue();
        return bullet;
    }

    // Havuza mermi geri koy
    public void ReturnBullet(GameObject bullet)
    {
        bullet.SetActive(false);
        bullet.transform.position = transform.position; // Mermiyi havuzun başlangıç pozisyonuna geri konumlandırabilirsiniz.
        bulletPool.Enqueue(bullet);
    }
}