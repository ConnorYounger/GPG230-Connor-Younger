using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeSpawner : MonoBehaviour
{
    [System.Serializable] public enum cubeTypes { defult, green, blue, yellow };
    public cubeTypes cubeType;

    public GameObject cubePrefab;
    private GameObject spawnedCube;

    public Transform spawnPoint;

    public float spawnForce = 150;

    public bool spawnAtStart = true;

    [Header("Cube Colours")]
    public Material cubeGreen;
    public Material cubeBlue;
    public Material cubeYellow;

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
            if (spawnedCube.GetComponent<SpawnedPuzzleObject>())
            {
                spawnedCube.GetComponent<SpawnedPuzzleObject>().DestroyObject();
            }
            else
            {
                Destroy(spawnedCube);
            }

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

        if(cubeType != cubeTypes.defult)
        {
            switch (cubeType)
            {
                case cubeTypes.green:
                    spawnedCube.tag = "green";
                    if(spawnedCube.GetComponent<MeshRenderer>() && cubeGreen)
                        spawnedCube.GetComponent<MeshRenderer>().material = cubeGreen;
                    break;
                case cubeTypes.blue:
                    spawnedCube.tag = "blue";
                    if (spawnedCube.GetComponent<MeshRenderer>() && cubeBlue)
                        spawnedCube.GetComponent<MeshRenderer>().material = cubeBlue;
                    break;
                case cubeTypes.yellow:
                    spawnedCube.tag = "yellow";
                    if (spawnedCube.GetComponent<MeshRenderer>() && cubeYellow)
                        spawnedCube.GetComponent<MeshRenderer>().material = cubeYellow;
                    break;
            }
        }
    }
}
