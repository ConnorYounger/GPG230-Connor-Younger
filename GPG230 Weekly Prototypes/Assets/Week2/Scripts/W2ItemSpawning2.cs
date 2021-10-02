using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class W2ItemSpawning2 : MonoBehaviour
{
    [System.Serializable]
    public struct spawnPoints
    {
        public GameObject itemPrefab;
        public bool smallItem;
    }

    public spawnPoints[] itemSpawnPoints;

    public List<Transform> smallSpawnPoints;
    public List<Transform> largeSpawnPoints;

    // Start is called before the first frame update
    void Start()
    {
        SpawnItems();
    }

    void SpawnItems()
    {
        foreach (spawnPoints sp in itemSpawnPoints)
        {
            Transform spawnPoint = null;

            if (sp.smallItem)
            {
                int randPointIndex = Random.Range(0, smallSpawnPoints.Count);
                spawnPoint = smallSpawnPoints[randPointIndex];
            }
            else
            {
                int randPointIndex = Random.Range(0, largeSpawnPoints.Count);
                spawnPoint = largeSpawnPoints[randPointIndex];
            }

            RaycastHit hit;
            Physics.Raycast(spawnPoint.position, Vector3.down, out hit);

            Quaternion randRotation = new Quaternion(0, Random.Range(0, 360), 0, 1);

            GameObject spawnedItem = Instantiate(sp.itemPrefab, hit.point, randRotation);

            if (sp.smallItem)
            {
                smallSpawnPoints.Remove(spawnPoint);
            }
            else
            {
                largeSpawnPoints.Remove(spawnPoint);
            }
        }
    }
}
