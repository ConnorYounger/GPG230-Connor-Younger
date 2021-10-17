using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhizDoor : MonoBehaviour
{
    public BoxCollider boxCollider;
    public MeshRenderer mesh;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void TurnOn()
    {
        if (boxCollider)
            boxCollider.enabled = true;

        if (mesh)
            mesh.enabled = true;
    }

    public void TurnOff()
    {
        if (boxCollider)
            boxCollider.enabled = false;

        if (mesh)
            mesh.enabled = false;
    }
}
