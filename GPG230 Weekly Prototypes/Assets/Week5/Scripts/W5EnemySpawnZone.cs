using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class W5EnemySpawnZone : MonoBehaviour
{
    public W5EnemySpawner spawner;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.name == "Player")
        {
            spawner.StartCoroutine("StartSpawnSequence", spawner.startSpawnDelay);
            this.enabled = false;
        }
    }
}
