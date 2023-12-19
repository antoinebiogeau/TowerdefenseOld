using UnityEngine;
using System.Collections.Generic;

public class Tower : MonoBehaviour
{
    public enum TowerType { Standard, AreaOfEffect }
    public TowerType towerType;
    public GameObject projectilePrefab;
    public float shootingRange = 10f;
    public float standardShootingDelay = 0.5f; 
    public float aoeShootingDelay = 1f; 
    private float lastShootTime;
    private List<Transform> enemiesInRange = new List<Transform>();
    private CircleCollider2D rangeCollider;

    void Start()
    {
        rangeCollider = gameObject.AddComponent<CircleCollider2D>(); 
        rangeCollider.radius = shootingRange;
        rangeCollider.isTrigger = true; 
        lastShootTime = Time.time; 
    }

    void Update()
    {
        UpdateEnemiesList();
        HandleShooting();
    }

    private void HandleShooting()
    {
        float timeSinceLastShot = Time.time - lastShootTime;
        float delay = GetShootingDelay();
        
        if (enemiesInRange.Count > 0 && timeSinceLastShot >= delay)
        {
            ShootAt(enemiesInRange[0]);
            lastShootTime = Time.time;
        }
    }



    private float GetShootingDelay()
    {
        return towerType == TowerType.Standard ? standardShootingDelay : aoeShootingDelay;
    }

    private void UpdateEnemiesList()
    {
        enemiesInRange.RemoveAll(enemy => enemy == null);
        Debug.Log("Enemies in range: " + enemiesInRange.Count);
    }


    private void ShootAt(Transform target)
    {
        GameObject projectile = Instantiate(projectilePrefab, transform.position, Quaternion.identity);
        Projectile projectileScript = projectile.GetComponent<Projectile>();
        projectileScript.SetTarget(target);
        projectileScript.isAreaOfEffect = (towerType == TowerType.AreaOfEffect);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Enemy"))
        {
            enemiesInRange.Add(other.transform);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Enemy"))
        {
            enemiesInRange.Remove(other.transform);
        }
    }
}
