using UnityEngine;
using UnityEngine.UI;

public class PlayerHealthUI : MonoBehaviour
{
    public Image healthBarImage;
    private float maxHealth = 100f;
    private float currentHealth;
    private RectTransform healthBarRect;

    void Start()
    {
        currentHealth = maxHealth;
        healthBarRect = healthBarImage.GetComponent<RectTransform>();
    }

    public void ReduceHealth(float damage)
    {
        currentHealth -= damage;
        currentHealth = Mathf.Max(currentHealth, 0);
        UpdateHealthBar();
    }

    private void UpdateHealthBar()
    {
        float healthPercentage = currentHealth / maxHealth;
        healthBarRect.localScale = new Vector3(healthPercentage, 1, 1);
    }

    void OnEnable()
    {
        EnnemyDep.OnEnemyReachedEnd += EnemyReachedEnd;
    }

    void OnDisable()
    {
        EnnemyDep.OnEnemyReachedEnd -= EnemyReachedEnd;
    }

    private void EnemyReachedEnd(EnnemyDep enemy)
    {
        ReduceHealth(10); 
    }
}