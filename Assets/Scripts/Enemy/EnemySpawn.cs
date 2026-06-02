using UnityEngine;

public class EnemySpawn : MonoBehaviour
{
    [SerializeField] private Transform[] spawnPoints;
    [SerializeField] private GameObject enemyPrefab;
    [SerializeField] int poolSize = 5; // We will have 5 enemies at max
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
        InvokeRepeating(nameof(SpawnEnemy), 1f, 3f); //Invoke SpawnEnemy for the first time after 1 sec
                                                     // next ones each 3 seconds
    }

    public void SpawnEnemy()
    {
        GameObject enemy = GetFreeEnemy(); //search if there is non-active enemy left
        if (enemy == null) return;         // if all enemies are active, it will exit from here

        Transform spawnPoint = spawnPoints[Random.Range(0, spawnPoints.Length)]; //choose random spawnpoint
        enemy.transform.position = spawnPoint.position;
        enemy.transform.parent = null;

        EnemyMovement enemyScript = enemy.GetComponent<EnemyMovement>(); //Gives an opportunity to use
        enemyScript.SetTarget(target);                                   // SetTarget func from Enemy movement script

        enemy.SetActive(true);
    }

    private GameObject GetFreeEnemy() //search if there is non-active enemy left
    {
        foreach (GameObject enemy in enemies)
        {
            if (!enemy.activeInHierarchy)
                return enemy;
        }
        return null;
    }
}
