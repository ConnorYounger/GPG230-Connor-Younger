using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class W3Door : MonoBehaviour
{
    public GameObject doorCollider;
    public Animator animator;

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

        if (animator)
            animator.SetBool("DoorOpen", true);
    }

    public void CloseDoor()
    {
        if (doorCollider)
            doorCollider.SetActive(true);

        if (animator)
            animator.SetBool("DoorOpen", false);
    }
}
