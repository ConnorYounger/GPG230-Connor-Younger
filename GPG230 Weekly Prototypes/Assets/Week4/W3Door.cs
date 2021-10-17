using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class W3Door : MonoBehaviour
{
    public GameObject doorCollider;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OpenDoor()
    {
        if (doorCollider)
            doorCollider.SetActive(false);
    }

    public void CloseDoor()
    {
        if (doorCollider)
            doorCollider.SetActive(true);
    }
}
