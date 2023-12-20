using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosionEffect : MonoBehaviour
{
    public float maxSize = 5f;
    [SerializeField] private float growSpeed = 1f;
    [SerializeField] private float duration = 1f; 
    private float currentSize = 0f;
    private float timeElapsed = 0f;

    void Update()
    {
        if (timeElapsed < duration)
        {
            currentSize = Mathf.Lerp(0f, maxSize, timeElapsed / duration);
            transform.localScale = new Vector3(currentSize, currentSize, 1f);

            timeElapsed += Time.deltaTime * growSpeed;
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
