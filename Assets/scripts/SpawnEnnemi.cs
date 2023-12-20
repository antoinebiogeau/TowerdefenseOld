using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnEnnemi : MonoBehaviour
{
    [SerializeField] private GameObject NMISlow;
    [SerializeField] private GameObject NMISpeeed;
    [SerializeField] private Transform SpawnPoint;

    public int currentWave = 0;
    public int numberOfEnemiesInWave;
    [SerializeField] private float delayBetweenEnemies = 1f;
    private bool isWaveInProgress = false;

    void Start()
    {
        StartCoroutine(SpawnWave());
    }

    private IEnumerator SpawnWave()
    {
        numberOfEnemiesInWave = currentWave * 5;
        for (int i = 0; i < numberOfEnemiesInWave; i++)
        {
            float random = Random.Range(0, 100); 
            if (random < currentWave * 10.0f) 
            {
                Instantiate(NMISpeeed, SpawnPoint.position, Quaternion.identity);
            }
            else
            {
                Instantiate(NMISlow, SpawnPoint.position, Quaternion.identity);
            }
            yield return new WaitForSeconds(delayBetweenEnemies);
        }
        isWaveInProgress = false;
    }


    public void StartNextWave(int waveNumber)
    {
        if (!isWaveInProgress)
        {
            isWaveInProgress = true;
            currentWave = waveNumber;
            StartCoroutine(SpawnWave());
        }
    }

}