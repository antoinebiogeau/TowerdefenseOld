using UnityEngine;

public class TowerPlacement : MonoBehaviour
{
    public static TowerPlacement instance;
    public GameObject standardTowerPrefab;
    private GameObject currentTowerGhost;

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

    public void StartPlacingTower(GameObject towerPrefab)
    {
        if (currentTowerGhost != null) Destroy(currentTowerGhost);

        currentTowerGhost = Instantiate(towerPrefab, Input.mousePosition, Quaternion.identity);
        currentTowerGhost.GetComponent<Renderer>().material.color = new Color(1, 1, 1, 0.5f);

        Tower towerScript = currentTowerGhost.GetComponent<Tower>();
        if (towerScript != null)
        {
        }
    }

    void Update()
    {
        if (currentTowerGhost != null)
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
            {
                if (!hit.collider.CompareTag("EnemyPath"))
                {
                    currentTowerGhost.transform.position = hit.point;

                    if (Input.GetMouseButtonDown(0)) 
                    {
                        PlaceTower();
                    }
                }
            }
        }
    }

    private void PlaceTower()
    {
        if (currentTowerGhost != null)
        {
            Tower towerScript = currentTowerGhost.GetComponent<Tower>();
            if (towerScript != null)
            {
            }
            currentTowerGhost.GetComponent<Renderer>().material.color = Color.white;
            currentTowerGhost = null;

            GameManager.instance.SpendGold(50); 
        }
    }
}
