using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhysicsGun : MonoBehaviour
{
    public Camera playerCamera;

    public bool canUse;

    [Header("FireForce")]
    public float fireForce;
    public float maxFireForce = 3000;
    public float minFireForce = 0;

    private Rigidbody _grabbedObject;

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
                }
            }

            // Mouse scroll wheel
            float axis = Input.GetAxisRaw("Mouse ScrollWheel");

            if (Input.GetKey(KeyCode.Mouse1))
            {
                if (axis > 0 && fireForce < maxFireForce)
                {
                    fireForce += 100;
                }
                else if (axis < 0 && fireForce > minFireForce)
                {
                    fireForce -= 100;
                }
            }
        }
    }

    void OnKeyDown(KeyCode button)
    {
        var ray = playerCamera.ViewportPointToRay(Vector3.one * .5f);

        if (button == KeyCode.Mouse0)
        {
            if (Physics.Raycast(ray, out RaycastHit hit)
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
        if (button == KeyCode.Mouse0 && _grabbedObject)
        {
            Rigidbody ob = _grabbedObject;
            _grabbedObject = null;
            ob.AddForce(playerCamera.transform.forward * fireForce);
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

}
