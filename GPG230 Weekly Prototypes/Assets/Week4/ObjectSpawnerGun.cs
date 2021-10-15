using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectSpawnerGun : MonoBehaviour
{
    public bool canUse;

    public GameObject defultObject;
    public GameObject lifeObject;
    public GameObject heavyObject;
    public Transform spawnPoint;
    public float fireForce = 200;
    public int fireMode;
    public float defultObjectDespawnTime = 3;
    public float fireRate = 0.5f;

    private float fireRateTimer;

    private GameObject previouslySpawnedObject;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(canUse)
            PlayerInput();

        FireRateTimer();
    }

    void FireRateTimer()
    {
        if(fireRateTimer > 0)
        {
            fireRateTimer -= Time.deltaTime;
        }
    }

    void PlayerInput()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if(fireMode == 1)
            {
                FireObject(lifeObject);
            }
            else if(fireMode == 2)
            {
                FireObject(heavyObject);
            }
        }

        if (Input.GetMouseButton(0) && fireRateTimer <= 0)
        {
            if(fireMode == 0)
            {
                FireDefultObject();
            }
        }

        // Mouse scroll wheel
        float axis = Input.GetAxisRaw("Mouse ScrollWheel");

        if (Input.GetKey(KeyCode.Mouse1))
        {
            if (axis > 0)
            {
                if (fireMode < 2)
                    fireMode++;
                else
                    fireMode = 0;
            }
            else if (axis < 0)
            {
                if (fireMode > 0)
                    fireMode--;
                else
                    fireMode = 2;
            }
        }
    }

    void FireDefultObject()
    {
        fireRateTimer = fireRate;

        GameObject obj = Instantiate(defultObject, spawnPoint.position, spawnPoint.rotation);

        if (obj.GetComponent<Rigidbody>())
        {
            obj.GetComponent<Rigidbody>().AddForce(spawnPoint.forward * fireForce);
        }

        Destroy(obj, defultObjectDespawnTime);
    }

    void FireObject(GameObject ob)
    {
        DespawnPreviousObject();

        GameObject obj = Instantiate(ob, spawnPoint.position, spawnPoint.rotation);

        if (obj.GetComponent<Rigidbody>())
        {
            obj.GetComponent<Rigidbody>().AddForce(spawnPoint.forward * fireForce);
        }

        previouslySpawnedObject = obj;
    }

    void DespawnPreviousObject()
    {
        if (previouslySpawnedObject)
        {
            Destroy(previouslySpawnedObject);
        }
    }
}
