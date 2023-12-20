using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField] private float speed = 10f;
    [SerializeField] private float damage = 40f;
    [SerializeField] public bool isAreaOfEffect = false;
    [SerializeField] private float areaEffectRadius = 5f;
    [SerializeField] private Transform target;
    [SerializeField] private GameObject explosionPrefab;
    public void SetTarget(Transform newTarget)
    {
        target = newTarget;
    }
    void Update()
    {
        if (target == null)
        {
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
