using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PreasurePad : MonoBehaviour
{
    public PuzzleConnections connector;
    public MeshRenderer mesh;
    public Animator animator;

    public bool singlePress;

    private List<Collider> colliders;

    // Start is called before the first frame update
    void Start()
    {
        colliders = new List<Collider>();
        connector = transform.parent.GetComponent<PuzzleConnections>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter(Collision collision)
    {
        colliders.Add(collision.collider);

        Debug.Log("Collision with: " + collision.collider);

        CheckForCollisions();
    }

    private void OnCollisionExit(Collision collision)
    {
        colliders.Remove(collision.collider);

        CheckForCollisions();
    }

    void CheckForCollisions()
    {
        //Debug.Log("CheckForCollisions");

        if(colliders.Count > 0)
        {
            connector.Activate(transform);
            //Debug.Log("Activate");
        }
        else
        {
            if(!singlePress)
                connector.Deactivate(transform);
            //Debug.Log("Deactivate");
        }
    }
}
