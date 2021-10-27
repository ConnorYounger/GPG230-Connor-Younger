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
    public bool spawnAfterDeath;
    private bool completedStartSpawn;

    public Transform spawnPointParent;
    public List<Transform> spawnPoints;
    private int spawnPointsIndex = 0;
    public float enemySpawnOffset;

    public float startSpawnDelay;
    private bool canSpawn;
    private bool startedSpawning;

    public int maxSpawnCount = 4;
    private int spawnedCount;
    public GameObject[] spawnedEnemies;

    public W5ScoreManager scoreManager;

    [Header("Enemy Stats")]
    public int fireRate;

    // Start is called before the first frame update
    void Start()
    {
        spawnPoints = new List<Transform>();
        spawnedEnemies = new GameObject[maxSpawnCount];

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
            StartCoroutine("StartSpawnSequence");
    }

    // Update is called once per frame
    void Update()
    {
        StartCoroutine("SpawnSequence");

        if(!canSpawn && BPeerM.beatFull && startedSpawning)
        {
            canSpawn = true;
        }
    }
    
    IEnumerator StartSpawnSequence()
    {
        yield return new WaitForSeconds(startSpawnDelay);

        startedSpawning = true;
    }

    void SpawnSequence()
    {
        if (canSpawn)
        {
            if (!spawnAfterDeath && spawnedCount < maxSpawnCount)
            {
                SpawnEnemyAtPoint();
            }
            else if (spawnAfterDeath && !completedStartSpawn && spawnedCount < maxSpawnCount)
            {
                SpawnEnemyAtPoint();
            }
            else
            {
                completedStartSpawn = true;
            }

            canSpawn = false;
        }

        //StartCoroutine("StartSpawnSequence", spawnTime);
    }

    void SpawnEnemyAtPoint(Transform point)
    {
        SpawnEnemy(point);
    }

    void SpawnEnemyAtPoint()
    {
        Transform spawnPoint = gameObject.transform;

        if (spawnPoints.Count > 0)
        {
            //Debug.Log("sp index: " + spawnPointsIndex);
            //Debug.Log("sp length " + spawnPoints.Count);

            spawnPoint = spawnPoints[spawnPointsIndex];

            if (spawnPointsIndex < spawnPoints.Count - 1)
                spawnPointsIndex++;
            else
                spawnPointsIndex = 0;
        }

        SpawnEnemy(spawnPoint);
    }

    void SpawnEnemy(Transform spawnPoint)
    {
        //Debug.Log("SpawnEnemies");

        GameObject spawnedEnemy = null;

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

            spawnedCount++;
        }
        else
            Debug.LogError("Missing base enemy prefab");
    }

    GameObject InstantiateEnemy(GameObject enemyPrefab, Transform spawnPoint)
    {
        GameObject enemy = Instantiate(enemyPrefab, spawnPoint.position, spawnPoint.rotation);

        int spawnedIndex = spawnPointsIndex;

        if (spawnedIndex > 0)
            spawnedIndex--;
        else
            spawnedIndex = spawnPoints.Count - 1;

        spawnedEnemies[spawnedIndex] = enemy;

        if (enemy.GetComponent<W5EnemyHealth>())
        {
            enemy.GetComponent<W5EnemyHealth>().spawner = this;

            enemy.GetComponent<W5EnemyHealth>().spawnIndex = spawnedIndex;
        }

        if (enemy.GetComponent<W5EnemyShooting>())
        {
            enemy.GetComponent<W5EnemyShooting>().scoreManager = scoreManager;
            enemy.GetComponent<W5EnemyShooting>().fireRate = fireRate;
        }

        if (enemySpawnOffset > 0)
            enemy.transform.position = spawnPoint.forward * enemySpawnOffset;

        if (setEnemyToChild)
            enemy.transform.parent = transform;

        return enemy;
    }

    public void RemoveEnemy(GameObject enemy, int index, int scoreValue)
    {
        if (scoreManager)
        {
            scoreManager.AddScore(scoreValue);
        }

        if (index >= 0 && index <= spawnPoints.Count - 1)
        {
            //Debug.Log("Remove: " + spawnedEnemies[index]);

            spawnedEnemies[index] = null;
            spawnedCount--;

            if (spawnAfterDeath)
            {
                //SpawnEnemyAtPoint(spawnPoints[index]);
            }

            CheckForRespawn();
        }
    }

    public void CheckForRespawn()
    {
        bool respawn = true;

        for(int i = 0; i < spawnedEnemies.Length; i++)
        {
            if(spawnedEnemies[i] != null)
            {
                respawn = false;
            }
        }

        Debug.Log("respawn = " + respawn);

        if(respawn == true)
        {
            if (spawnAfterDeath)
            {
                completedStartSpawn = false;
                StartSpawnSequence();            }
        }
    }
}
