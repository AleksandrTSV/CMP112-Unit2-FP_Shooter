using UnityEngine;

public class EnemySpawn : MonoBehaviour
{
    [SerializeField] private Transform[] spawnPoints;
    [SerializeField] private GameObject enemyPrefab;
    [SerializeField] int poolSize = 5;
    [SerializeField] Transform target;

    GameObject[] enemies;

    void Awake()
    {
        enemies = new GameObject[poolSize];

        for (int i = 0; i < poolSize; i++)
        {
            enemies[i] = Instantiate(enemyPrefab);
            enemies[i].gameObject.SetActive(false);
        }
    }

    private void Start()
    {
        InvokeRepeating(nameof(SpawnEnemy), 1f, 3f);
    }

    public void SpawnEnemy()
    {
        GameObject enemy = GetFreeEnemy();
        if (enemy == null) return;

        Transform spawnPoint = spawnPoints[Random.Range(0, spawnPoints.Length)];
        enemy.transform.position = spawnPoint.position;
        enemy.transform.parent = null;

        EnemyMovement enemyScript = enemy.GetComponent<EnemyMovement>();
        enemyScript.SetTarget(target);

        enemy.SetActive(true);
    }

    private GameObject GetFreeEnemy()
    {
        foreach (GameObject enemy in enemies)
        {
            if (!enemy.activeInHierarchy)
                return enemy;
        }
        return null;
    }
}
