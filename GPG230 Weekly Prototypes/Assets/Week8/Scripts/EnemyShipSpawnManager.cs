using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyShipSpawnManager : MonoBehaviour
{
    [System.Serializable]
    public struct spawnSlots
    {
        public GameObject enemyPrefab;
        public Transform spawnPoint;
        public Transform enemyPath;
    }
    public spawnSlots[] enemySpawns;

    public List<GameObject> spawnedEnemies;

    void Start()
    {
        spawnedEnemies = new List<GameObject>();

        SpawnEnemies();
    }

    public void SpawnEnemies()
    {
        foreach(spawnSlots slot in enemySpawns)
        {
            GameObject spawnedEnemy = Instantiate(slot.enemyPrefab, slot.spawnPoint.position, slot.spawnPoint.rotation);
            EnemyShipHealth enemyHealth = spawnedEnemy.GetComponent<EnemyShipHealth>();
            EnemyShipAI enemyAI = spawnedEnemy.GetComponent<EnemyShipAI>();

            if(enemyHealth != null)
            {
                enemyHealth.spawnManager = this;
            }

            if(enemyAI != null)
            {
                enemyAI.SetTravelPaths(slot.enemyPath);
            }

            spawnedEnemies.Add(spawnedEnemy);
        }
    }

    public void RemoveShip(GameObject ship)
    {
        spawnedEnemies.Remove(ship);

        if(spawnedEnemies.Count <= 0)
        {
            ScenarioComplete();
        }
    }

    void ScenarioComplete()
    {
        Debug.Log("Scenario Complete");
    }

    public void PlayerDeath()
    {
        if (spawnedEnemies.Count > 0)
        {
            foreach (GameObject enemy in spawnedEnemies)
            {
                if (enemy.GetComponent<EnemyShipAI>())
                {
                    enemy.GetComponent<EnemyShipAI>().enabled = false;
                }

                foreach (Transform c in enemy.transform)
                {
                    enemy.gameObject.SetActive(false);
                }
            }
        }
    }
}
