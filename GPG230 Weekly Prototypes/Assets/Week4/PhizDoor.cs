using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhizDoor : MonoBehaviour
{
    public BoxCollider boxCollider;
    public MeshRenderer mesh;
    public List<GameObject> actives;
    public AudioSource audioSource;

    // Start is called before the first frame update
    void Start()
    {
        actives = new List<GameObject>();
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

        if (audioSource)
            audioSource.Play();
    }

    public void TurnOn(GameObject active)
    {
        actives.Remove(active);

        if (actives.Count <= 0)
        {
            if (boxCollider)
                boxCollider.enabled = true;

            if (mesh)
                mesh.enabled = true;

            if (audioSource)
                audioSource.Play();
        }
    }

    public void TurnOff()
    {
        if (boxCollider)
            boxCollider.enabled = false;

        if (mesh)
            mesh.enabled = false;

        if (audioSource)
            audioSource.Stop();
    }

    public void TurnOff(GameObject active)
    {
        if (actives.Count > 0)
        {
            bool inList = false;

            foreach (GameObject o in actives)
            {
                if (active == o)
                {
                    inList = true;
                }
            }

            if (!inList)
                actives.Add(active);
        }
        else
            actives.Add(active);

        if (actives.Count > 0)
        {
            if (boxCollider)
                boxCollider.enabled = false;

            if (mesh)
                mesh.enabled = false;

            if (audioSource)
                audioSource.Stop();
        }
    }
}
