using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class W5PlayerMovement : MonoBehaviour
{
    public GameObject player;
    public float speed;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        PlayerMovement();
    }

    void PlayerMovement()
    {
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        Vector3 newPos = new Vector3(x, 0, z);
        player.transform.position += newPos * speed * Time.deltaTime;
    }
}
