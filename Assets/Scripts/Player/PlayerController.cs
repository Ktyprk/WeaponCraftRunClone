using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public static PlayerController Instance { get; private set; }
    [Header("Movement Settings")]
    public float forwardSpeed = 5f;
    public float sidewaysSpeed = 5f;
    public float maxSidewaysPosition = 5f;

    [Header("Shooting Settings")]
    public BulletPoolScript bulletPool;
    public Transform firePoint;
    public float fireRate = 0.5f;
    private float nextFireTime;

    [Header("Bullet Settings")]
    public float bulletSpeed = 10f;
    public float maxBulletDistance = 10f; 
    
    [Header("Gun Settings")]
    public List<GameObject> gunList; 
    private int currentGunIndex = 0; 

    [Header("Player Settings")]
    public int currentYear = 1800; 
    private int nextGunActivationYear = 1850; 


    private bool isDragging = false;
    private Vector2 dragStartPosition;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        
    }
    private void Update()
    {
        MovePlayer();
        HandleShooting();
        
        transform.Translate(Vector3.forward * forwardSpeed * Time.deltaTime);
        
        UpdateGun();
    }
    
    public void IncreaseFireRate(float amount)
    {
        fireRate += amount;
    }
    
    public void IncreaseFireRange(float amount)
    {
        maxBulletDistance += amount;
    }
    
    public void IncreaseYear(int amount)
    {
        currentYear += amount;
    }

    private void MovePlayer()
    {
        if (Input.GetMouseButtonDown(0))
        {
            isDragging = true;
            dragStartPosition = Input.mousePosition;
        }
        else if (Input.GetMouseButtonUp(0))
        {
            isDragging = false;
        }

        if (isDragging)
        {
            Vector2 dragDelta = (Vector2)Input.mousePosition - dragStartPosition;
            float deltaX = dragDelta.x * sidewaysSpeed * Time.deltaTime;
            float newXPos = Mathf.Clamp(transform.position.x + deltaX, -maxSidewaysPosition, maxSidewaysPosition);
            transform.position = new Vector3(newXPos, transform.position.y, transform.position.z);
            dragStartPosition = Input.mousePosition;
        }
    }
    
    private void UpdateGun()
    {
        if (currentYear >= nextGunActivationYear && currentGunIndex < gunList.Count - 1)
        {
            currentGunIndex++;
            gunList[currentGunIndex].SetActive(true);
            gunList[currentGunIndex - 1].SetActive(false);
            nextGunActivationYear += 50;
            
            GameObject activeGun = gunList[currentGunIndex];
            Transform newFirePoint = activeGun.transform.Find("FirePoint");
            if (newFirePoint != null)
            {
               firePoint = newFirePoint;
            }
            else
            {
                Debug.LogError("FirePoint not found under active gun!");
            }
        }
    }

    private void HandleShooting()
    {
        if (Time.time >= nextFireTime && Input.GetMouseButton(0))
        {
            Shoot();
            nextFireTime = Time.time + 1f / fireRate;
        }
    }

    private void Shoot()
    {
        GameObject bullet = bulletPool.GetBullet();
        bullet.transform.position = firePoint.position;
        bullet.transform.rotation = firePoint.rotation;
        bullet.SetActive(true);
        
        StartCoroutine(MoveBulletAndReturnToPool(bullet));
    }

     private IEnumerator MoveBulletAndReturnToPool(GameObject bullet)
    {
        Vector3 initialPosition = bullet.transform.position;
        float distanceTraveled = 0f;

        while (distanceTraveled < maxBulletDistance)
        {
            bullet.transform.Translate(Vector3.forward * bulletSpeed * Time.deltaTime);
            distanceTraveled += bulletSpeed * Time.deltaTime;
            yield return null;
        }
        
        bullet.SetActive(false);
        bullet.transform.position = initialPosition;
        bulletPool.ReturnBullet(bullet);
    }
}
