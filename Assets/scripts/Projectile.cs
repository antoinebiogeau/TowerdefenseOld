using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float speed = 10f;
    public float damage = 40f;
    public bool isAreaOfEffect = false; 
    public float areaEffectRadius = 5f;
    private Transform target;
    public GameObject explosionPrefab;
    public void SetTarget(Transform newTarget)
    {
        Debug.Log("Projectile setting target: " + newTarget.name);
        target = newTarget;
    }
    void Update()
    {
        if (target == null)
        {
            Debug.Log("Projectile target is null, destroying projectile.");
            Destroy(gameObject);
            return;
        }


        Vector3 direction = (target.position - transform.position).normalized;
        transform.Translate(direction * speed * Time.deltaTime, Space.World);

        if (Vector3.Distance(transform.position, target.position) < 0.5f)
        {
            HitTarget();
        }
    }
    void HitTarget()
    {
        if (isAreaOfEffect)
        {
            Explode();
        }
        else
        {
            Damage(target);
        }

        Destroy(gameObject);
    }
    void Explode()
    {
        GameObject explosionEffect = Instantiate(explosionPrefab, transform.position, Quaternion.identity);
        explosionEffect.GetComponent<ExplosionEffect>().maxSize = areaEffectRadius;
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, areaEffectRadius);
        foreach (Collider2D nearbyObject in colliders)
        {
            if (nearbyObject.CompareTag("Enemy"))
            {
                Damage(nearbyObject.transform);
            }
        }
    }

    
    void Damage(Transform enemy)
    {
        EnnemyDep enemyHealth = enemy.GetComponent<EnnemyDep>();
        if (enemyHealth != null)
        {
            enemyHealth.TakeDamage(damage);
        }
    }

}
