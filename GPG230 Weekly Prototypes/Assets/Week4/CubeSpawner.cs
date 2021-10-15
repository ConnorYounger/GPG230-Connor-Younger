using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeSpawner : MonoBehaviour
{
    public GameObject cubePrefab;
    private GameObject spawnedCube;

    public Transform spawnPoint;

    public float spawnForce = 150;

    public bool spawnAtStart = true;

    // Start is called before the first frame update
    void Start()
    {
        if (spawnAtStart)
            SpawnNewCube();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SpawnNewCube()
    {
        if (spawnedCube)
        {
            Destroy(spawnedCube);
            spawnedCube = null;
        }

        spawnedCube = Instantiate(cubePrefab, spawnPoint.position, spawnPoint.rotation);

        if (!spawnedCube.GetComponent<SpawnedPuzzleObject>())
        {
            SpawnedPuzzleObject p = spawnedCube.AddComponent<SpawnedPuzzleObject>();
            p.spawner = this;
        }

        if (spawnedCube.GetComponent<Rigidbody>())
        {
            spawnedCube.GetComponent<Rigidbody>().AddForce(spawnPoint.forward * spawnForce);
        }
    }
}
