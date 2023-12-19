using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnnemyDep : MonoBehaviour
{
    [SerializeField] LineRenderer line;
    private Vector3[] pos;
    private int currentIndexCheckpoint = 0;
    public float speed = 5;
    public float maxHealth = 100f; 
    public int goldValue = 10; 
    private float currentHealth;
    public Gradient lifeGradient;
    private Renderer renderer;
    public delegate void EnemyDiedHandler(EnnemyDep enemy);
    public static event EnemyDiedHandler OnEnemyDied; 


    public delegate void EnemyReachedEndHandler(EnnemyDep enemy);
    public static event EnemyReachedEndHandler OnEnemyReachedEnd; 

    void Start()
    {
        pos = new Vector3[line.positionCount];
        line.GetPositions(pos);
        currentHealth = maxHealth;
        renderer = GetComponent<Renderer>();
        UpdateColor();
    }

    void Update()
    {
        if (currentIndexCheckpoint < pos.Length)
        {
            MoveToNextPoint();
        }
        else
        {
            OnEnemyReachedEnd?.Invoke(this);
            Destroy(gameObject); 
        }
    }

    private void MoveToNextPoint()
    {
        transform.position = Vector3.MoveTowards(transform.position, pos[currentIndexCheckpoint], speed * Time.deltaTime);

        if (transform.position == pos[currentIndexCheckpoint])
        {
            currentIndexCheckpoint++;
        }
    }

    public void TakeDamage(float damage)
    {
        currentHealth -= damage;
        currentHealth = Mathf.Max(currentHealth, 0);
        UpdateColor();

        if (currentHealth <= 0)
        {
            GiveGold();
            Die();
        }
    }

    private void GiveGold()
    {
        OnEnemyDied?.Invoke(this); 
    }

    private void Die()
    {
        Destroy(gameObject);
    }

    private void UpdateColor()
    {
        Color healthColor = lifeGradient.Evaluate(currentHealth / maxHealth);
        renderer.material.color = healthColor;
    }
    
}
