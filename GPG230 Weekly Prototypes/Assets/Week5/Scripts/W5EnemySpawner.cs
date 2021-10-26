using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class W5EnemySpawner : MonoBehaviour
{
    public GameObject baseEnemyPrefab;
    public GameObject[] timedEnemyPrefab;
    private int spawnedEnemyIndex;
    public bool spawnAtStart;
    public bool spawnBaseOnEmpty;
    public bool setEnemyToChild;

    public Transform spawnPointParent;
    public List<Transform> spawnPoints;
    private int spawnPointsIndex = 0;
    public float enemySpawnOffset;

    public float startSpawnDelay;
    public float spawnTime = 0.4f;
    public int band = 4;

    public int maxSpawnCount = 4;
    public List<GameObject> spawnedEnemies;

    // Start is called before the first frame update
    void Start()
    {
        spawnPoints = new List<Transform>();
        spawnedEnemies = new List<GameObject>();

        if (spawnPointParent)
        {
            foreach (Transform t in spawnPointParent)
            {
                spawnPoints.Add(t);
            }
        }

        if (spawnPoints.Count == 0)
        {
            spawnPoints.Add(transform);
        }

        if(spawnAtStart)
            StartCoroutine("StartSpawnSequence", startSpawnDelay);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    IEnumerator StartSpawnSequence(float time)
    {
        yield return new WaitForSeconds(time);

        Debug.Log("SpawnEnemies");

        if(spawnedEnemies.Count < maxSpawnCount)
        {
            SpawnEnemy();
        }

        StartCoroutine("StartSpawnSequence", spawnTime);
    }

    void SpawnEnemy()
    {
        Transform spawnPoint = gameObject.transform;
        GameObject spawnedEnemy = null;

        if (spawnPoints.Count > 0)
        {
            Debug.Log("sp index: " + spawnPointsIndex);
            Debug.Log("sp length " + spawnPoints.Count);

            spawnPoint = spawnPoints[spawnPointsIndex];
            
            if(spawnPointsIndex < spawnPoints.Count - 1)
                spawnPointsIndex++;
            else
                spawnPointsIndex = 0;
        }

        if (baseEnemyPrefab)
        {
            if (timedEnemyPrefab.Length > 0)
            {
                if(timedEnemyPrefab[spawnedEnemyIndex] != null)
                {
                    spawnedEnemy = InstantiateEnemy(timedEnemyPrefab[spawnedEnemyIndex], spawnPoint);
                }
                else
                {
                    if (spawnBaseOnEmpty)
                    {
                        spawnedEnemy = InstantiateEnemy(baseEnemyPrefab, spawnPoint);
                    }
                }

                if (spawnedEnemyIndex < timedEnemyPrefab.Length - 1)
                    spawnedEnemyIndex++;
                else
                    spawnedEnemyIndex = 0;
            }
            else
            {
                spawnedEnemy = InstantiateEnemy(baseEnemyPrefab, spawnPoint);
            }
        }
        else
            Debug.LogError("Missing base enemy prefab");
    }

    GameObject InstantiateEnemy(GameObject enemyPrefab, Transform spawnPoint)
    {
        GameObject enemy = Instantiate(enemyPrefab, spawnPoint.position, spawnPoint.rotation);
        spawnedEnemies.Add(enemy);

        if (enemySpawnOffset > 0)
            enemy.transform.position = spawnPoint.forward * enemySpawnOffset;

        if (setEnemyToChild)
            enemy.transform.parent = transform;

        return enemy;
    }
}
