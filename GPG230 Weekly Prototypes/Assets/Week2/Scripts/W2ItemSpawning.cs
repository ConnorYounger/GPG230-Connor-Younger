using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class W2ItemSpawning : MonoBehaviour
{
    [System.Serializable]
    public struct spawnPoints
    {
        public string itemType;
        public GameObject itemPrefab;
        public List<Transform> itemSpawnPoints;
    }

    public spawnPoints[] itemSpawnPoints;

    void Start()
    {
        SpawnItems();
    }

    void SpawnItems()
    {
        foreach(spawnPoints sp in itemSpawnPoints)
        {
            int randPointIndex = Random.Range(0, sp.itemSpawnPoints.Count);
            Transform spawnPoint = sp.itemSpawnPoints[randPointIndex];

            GameObject spawnedItem = Instantiate(sp.itemPrefab, spawnPoint.position, spawnPoint.rotation);

            // Remove the spawn point for other item spawns
            foreach (spawnPoints s in itemSpawnPoints)
            {
                if (sp.itemPrefab != s.itemPrefab)
                {
                    Transform pointToRemove = null;

                    foreach (Transform point in s.itemSpawnPoints)
                    {
                        if(point == spawnPoint)
                            pointToRemove = point;
                    }

                    if (pointToRemove != null)
                        s.itemSpawnPoints.Remove(pointToRemove);
                }
            }
        }
    }
}
