using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class GameManager : MonoBehaviour
{
        //Bon j'avoue que j'aurais pu encaspuler le code et pas avoir un gamemanager aussi fat
        [SerializeField] private TextMeshProUGUI gameOverText;
        public Image healthBarImage;
        private float maxHealth = 100f;
        private float currentHealth;
        private RectTransform healthBarRect;
        [SerializeField] private TextMeshProUGUI goldText; 
        private int goldAmount = 0;
        [SerializeField] private GameObject standardTowerPrefab;
        [SerializeField] private LayerMask pathLayer; 
        private GameObject currentTowerGhost;
        [SerializeField] private static GameManager instance;
        [SerializeField] private SpawnEnnemi spawnEnnemiScript;
        [SerializeField] private TextMeshProUGUI waveText;
        private bool isGameActive = true;
        private int currentWave = 0;
        private int enemiesRemaining;
        [SerializeField] private float tempsEntreLesVagues = 5f;
        private bool isWaitingForNextWave = false;
        [SerializeField] private GameObject advancedTowerPrefab;
        void Awake()
        {
            if (instance != null && instance != this)
            {
                Destroy(gameObject);
            }
            else
            {
                instance = this;
            }
        }
        void Start()
        {
            currentHealth = maxHealth;
            healthBarRect = healthBarImage.GetComponent<RectTransform>();
            UpdateGoldUI();
            StartNextWave();
        }
        void Update()
        {
            if (currentTowerGhost != null)
            {
                currentTowerGhost.transform.position = GetMouseWorldPosition();
                if (Input.GetMouseButtonDown(0))
                {
                    PlaceTower();
                }
            }
            if (isGameActive && currentHealth <= 0)
            {
                StopGame();
            }
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
            EnnemyDep.OnEnemyDied += EnemyDied;
        }

        void OnDisable()
        {
            EnnemyDep.OnEnemyReachedEnd -= EnemyReachedEnd;
            EnnemyDep.OnEnemyDied -= EnemyDied;
        }
        public void AddGold(int amount)
        {
            goldAmount += amount;
            UpdateGoldUI();
        }

        public void SpendGold(int amount)
        {
            if (goldAmount >= amount)
            {
                goldAmount -= amount;
                UpdateGoldUI();
            }
            else
            {
                Debug.Log("Not Enough gold!");
            }
        }
        private void UpdateGoldUI()
        {
            goldText.text = $"Gold : {goldAmount}";
        }
        public void OnStandardTowerButtonClicked()
        {
            if (goldAmount >= 50)
            {
                StartPlacingTower(standardTowerPrefab);
            }
            else
            {
                Debug.Log("Not Enough gold!");
            }
        }

        private void StartPlacingTower(GameObject towerPrefab)
        {
            if (currentTowerGhost != null) Destroy(currentTowerGhost);
            currentTowerGhost = Instantiate(towerPrefab, GetMouseWorldPosition(), Quaternion.identity);
            Tower towerScript = currentTowerGhost.GetComponent<Tower>();
            towerScript.canShoot = false;
            currentTowerGhost.GetComponent<Renderer>().material.color = new Color(1, 1, 1, 0.5f);
        }

        private Vector3 GetMouseWorldPosition()
        {
            Vector3 mouseScreenPosition = new Vector3(Input.mousePosition.x, Input.mousePosition.y, Camera.main.nearClipPlane);
            Vector3 mouseWorldPosition = Camera.main.ScreenToWorldPoint(mouseScreenPosition);
            mouseWorldPosition.z = 0; 
            return mouseWorldPosition;
        }


        private void PlaceTower()
        {
            Vector2 positionToCheck = currentTowerGhost.transform.position;
            if (Physics2D.OverlapCircle(positionToCheck, 0.5f, pathLayer) == null)
            {
                Tower towerScript = currentTowerGhost.GetComponent<Tower>();
                currentTowerGhost.GetComponent<Renderer>().material.color = Color.white;
                towerScript.canShoot = true;
                currentTowerGhost = null;
                SpendGold(50);
            }
        }

        public void StartNextWave()
        {
            if (isGameActive && currentHealth > 0)
            {
                isWaitingForNextWave = false;
                currentWave++;
                waveText.text = "Wave: " + currentWave;
                spawnEnnemiScript.StartNextWave(currentWave);
                enemiesRemaining = spawnEnnemiScript.numberOfEnemiesInWave;
            }
            else
            {
            }
        }
        private void EnemyDied(EnnemyDep enemy)
        {
            AddGold(enemy.goldValue);
            enemiesRemaining--;
            CheckWaveCompletion();
        }
        private void StopGame()
        {
            isGameActive = false;
            gameOverText.gameObject.SetActive(true); 
            Debug.Log("Game Over");

        }
        private void EnemyReachedEnd(EnnemyDep enemy)
        {
            ReduceHealth(10);
            enemiesRemaining--; 
            CheckWaveCompletion();

            if (currentHealth <= 0)
            {
                StopGame();
            }
        }
        private IEnumerator DelayedStartNextWave()
        {
            yield return new WaitForSeconds(tempsEntreLesVagues);
            StartNextWave();
        }

        private void CheckWaveCompletion()
        {
            if (!isWaitingForNextWave && enemiesRemaining <= 0)
            {
                isWaitingForNextWave = true;
                StartCoroutine(DelayedStartNextWave());
            }
        }
        public void OnAdvancedTowerButtonClicked()
        {
            if (goldAmount >= 100)
            {
                StartPlacingTower(advancedTowerPrefab);
            }
            else
            {
                Debug.Log("Not Enougth gold for advanced tower!");
            }
        }
}
