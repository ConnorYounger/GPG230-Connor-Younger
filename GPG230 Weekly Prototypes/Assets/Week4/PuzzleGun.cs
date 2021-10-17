using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuzzleGun : MonoBehaviour
{
    [Header("Gravity Gun")]
    public Camera playerCamera;

    public bool canUse;
    public bool canSpawnObjects;

    public float fireForce = 2000;

    private Rigidbody _grabbedObject;

    [Header("Object Gun")]
    public GameObject cubeObject;
    public GameObject sphereObject;
    public Transform spawnPoint;
    public int fireMode;

    private GameObject previouslySpawnedObject;

    void OnDisable()
    {
        _grabbedObject = null;
    }

    private void Start()
    {
        if (!playerCamera)
        {
            playerCamera = GameObject.Find("FirstPersonCharacter").GetComponent<Camera>();
        }
    }

    private void Update()
    {
        PlayerInput();
    }

    void PlayerInput()
    {
        if (canUse)
        {
            bool createOb = true;

            // Mouse fire
            if (Input.GetKeyDown(KeyCode.Mouse0))
            {
                if (!_grabbedObject)
                {
                    OnKeyDown(KeyCode.Mouse0);
                }
                else
                {
                    OnKeyUp(KeyCode.Mouse0);
                    createOb = false;
                }
            }

            // Lauch Held Object
            if (Input.GetKey(KeyCode.Mouse1))
            {
                OnKeyUp(KeyCode.Mouse1);
            }

            if (canSpawnObjects)
            {
                // Create Object
                if (Input.GetMouseButtonDown(0) && !_grabbedObject && createOb)
                {
                    if (fireMode == 1)
                    {
                        FireObject(cubeObject);
                    }
                    else if (fireMode == 2)
                    {
                        FireObject(sphereObject);
                    }
                }

                // Mouse scroll wheel
                float axis = Input.GetAxisRaw("Mouse ScrollWheel");

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
    }

    void OnKeyDown(KeyCode button)
    {
        var ray = playerCamera.ViewportPointToRay(Vector3.one * .5f);

        if (button == KeyCode.Mouse0)
        {
            if (Physics.Raycast(ray, out RaycastHit hit, 3)
                && hit.rigidbody
                && !hit.rigidbody.CompareTag("Player"))
            {
                _grabbedObject = hit.rigidbody;
            }
        }
        else if (button == KeyCode.Mouse1 && _grabbedObject)
        {
            _grabbedObject.velocity = playerCamera.transform.forward * 30f;
            _grabbedObject = null;
        }
    }

    void OnKeyUp(KeyCode button)
    {
        if (_grabbedObject)
        {
            Rigidbody ob = _grabbedObject;
            _grabbedObject = null;

            if (button == KeyCode.Mouse1)
            {
                ob.AddForce(playerCamera.transform.forward * fireForce);
            }
        }
    }

    private void FixedUpdate()
    {
        if (_grabbedObject)
        {
            var targetPosition = playerCamera.transform.position + playerCamera.transform.forward * 2f;
            var forceDir = targetPosition - _grabbedObject.position;
            var force = forceDir / Time.fixedDeltaTime * 0.3f / _grabbedObject.mass;
            _grabbedObject.velocity = force;
            _grabbedObject.transform.Rotate(playerCamera.transform.forward, 20f * Time.fixedDeltaTime);
        }
    }

    void FireObject(GameObject ob)
    {
        DespawnPreviousObject();

        GameObject obj = Instantiate(ob, spawnPoint.position, spawnPoint.rotation);

        if (obj.GetComponent<Rigidbody>())
        {
            //obj.GetComponent<Rigidbody>().AddForce(spawnPoint.forward * fireForce);

            _grabbedObject = obj.GetComponent<Rigidbody>();
        }

        if (!obj.GetComponent<SpawnedPuzzleObject>())
        {
            obj.AddComponent<SpawnedPuzzleObject>();
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
