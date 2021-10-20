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

    public AudioSource audioSource;
    public AudioClip cubeDestroySound;

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
            p.destroySound = cubeDestroySound;
        }

        if (spawnedCube.GetComponent<Rigidbody>())
        {
            spawnedCube.GetComponent<Rigidbody>().AddForce(spawnPoint.forward * spawnForce);
        }

        if (audioSource)
        {
            audioSource.Play();
        }

        if(cubeType != cubeTypes.defult)
        {
            Debug.Log("Spawn different cube");

            if (spawnedCube.GetComponent<Animator>())
            {
                spawnedCube.GetComponent<Animator>().enabled = false;
            }

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
                    {
                        spawnedCube.GetComponent<MeshRenderer>().material = cubeBlue;
                        Debug.Log("make cube blue");
                    }
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
