using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuzzleGun : MonoBehaviour
{
    [Header("Gravity Gun")]
    public Camera playerCamera;

    [SerializeField]
    private LineRenderer _pickLine;
    public Material lineMaterial;
    public Transform barrelPos;

    private float _pickDistance;
    private Vector3 _pickOffset;

    public bool canUse;
    public bool canSpawnObjects;
    public bool canSwitchModes;

    public float fireForce = 2000;

    public Rigidbody _grabbedObject;

    public Animator animatior;

    [Header("Object Gun")]
    public GameObject cubeObject;
    public GameObject sphereObject;
    public Transform spawnPoint;
    public int fireMode;
    public Material playerMaterial;

    [Header("Particles")]
    public ParticleSystem muzzleParticles;
    public ParticleSystem objectParticles;
    public GameObject fireFx;

    private GameObject previouslySpawnedObject;

    [Header("Audio")]
    public AudioSource audioSource;
    public AudioSource audioSourceLoop;
    public AudioClip[] fireSound;
    public AudioClip holdStart;
    public AudioClip createCubeSound;
    public AudioClip cubeFizzleSound;

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

        if (!_pickLine)
        {
            var obj = new GameObject("PhysGun Pick Line");
            _pickLine = obj.AddComponent<LineRenderer>();
            _pickLine.startWidth = 0.02f;
            _pickLine.endWidth = 0.02f;
            _pickLine.useWorldSpace = true;
            _pickLine.gameObject.SetActive(false);

            if (lineMaterial)
            {
                _pickLine.material = lineMaterial;
            }
        }

        if(canSpawnObjects && !canSwitchModes)
        {
            fireMode = 1;
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

                if (canSwitchModes)
                {
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
    }

    void OnKeyDown(KeyCode button)
    {
        var ray = playerCamera.ViewportPointToRay(Vector3.one * .5f);

        if (button == KeyCode.Mouse0)
        {
            if (Physics.SphereCast(ray, 0.5f, out RaycastHit hit, 3)
                && hit.rigidbody
                && !hit.rigidbody.CompareTag("Player"))
            {
                _grabbedObject = hit.rigidbody;

                _pickLine.gameObject.SetActive(true);

                _pickDistance = hit.distance;
                _pickOffset = hit.transform.InverseTransformVector(hit.point - hit.transform.position);

                if (_grabbedObject.GetComponent<SpawnedPuzzleObject>())
                {
                    _grabbedObject.GetComponent<SpawnedPuzzleObject>().puzzleGun = this;
                }

                if(audioSource && holdStart)
                {
                    audioSource.clip = holdStart;
                    audioSource.Play();
                    StopCoroutine("StartHoldLoop");
                    StartCoroutine("StartHoldLoop");
                }
            }
        }
        else if (button == KeyCode.Mouse1 && _grabbedObject)
        {
            _grabbedObject.velocity = playerCamera.transform.forward * 30f;
            _grabbedObject = null;

            _pickLine.gameObject.SetActive(false);
        }
    }

    IEnumerator StartHoldLoop()
    {
        yield return new WaitForSeconds(0.2f);

        if (_grabbedObject && audioSourceLoop)
        {
            audioSourceLoop.Play();
        }
    }

    void OnKeyUp(KeyCode button)
    {
        if (_grabbedObject)
        {
            Rigidbody ob = _grabbedObject;
            _grabbedObject = null;

            _pickLine.gameObject.SetActive(false);

            if (button == KeyCode.Mouse1)
            {
                ob.AddForce(playerCamera.transform.forward * fireForce);

                if (animatior)
                {
                    animatior.Play("PhisGunFire");
                }

                if (fireFx)
                {
                    GameObject fx = Instantiate(fireFx, barrelPos.position, barrelPos.rotation);
                    Destroy(fx, 1);
                }

                if (audioSource && fireSound.Length > 0)
                {
                    int rand = Random.Range(0, fireSound.Length);
                    audioSource.clip = fireSound[rand];
                    audioSource.Play();
                }
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

            if (animatior)
            {
                animatior.SetBool("isHolding", true);
            }

            if (objectParticles && !objectParticles.isPlaying)
            {
                objectParticles.transform.position = _grabbedObject.transform.position;
                objectParticles.transform.parent = _grabbedObject.transform;
                objectParticles.transform.localScale = new Vector3(1, 1, 1);
                objectParticles.gameObject.SetActive(true);
            }

            if (muzzleParticles && !muzzleParticles.isPlaying)
            {
                muzzleParticles.Play();
            }
        }
        else
        {
            if (animatior)
            {
                animatior.SetBool("isHolding", false);
            }

            if (objectParticles && objectParticles.isPlaying)
            {
                objectParticles.gameObject.SetActive(false);
            }

            if (muzzleParticles && muzzleParticles.isPlaying)
            {
                muzzleParticles.Stop();
            }

            if (audioSourceLoop)
            {
                audioSourceLoop.Stop();
            }
        }
    }

    private void LateUpdate()
    {
        if (_grabbedObject)
        {
            var midpoint = playerCamera.transform.position + playerCamera.transform.forward * _pickDistance * .5f;
            DrawQuadraticBezierCurve(_pickLine, barrelPos.position, midpoint, _grabbedObject.position);
        }
        else
        {
            _pickLine.gameObject.SetActive(false);

            if (objectParticles)
            {
                objectParticles.transform.parent = spawnPoint.transform;
            }
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
            obj.GetComponent<SpawnedPuzzleObject>().puzzleGun = this;
            obj.GetComponent<SpawnedPuzzleObject>().destroySound = cubeFizzleSound;
        }

        if (obj.GetComponent<MeshRenderer>() && playerMaterial)
        {
            obj.GetComponent<MeshRenderer>().material = playerMaterial;
        }

        if (audioSource && holdStart)
        {
            audioSource.clip = createCubeSound;
            audioSource.Play();
            StopCoroutine("StartHoldLoop");
            StartCoroutine("StartHoldLoop");
        }

        _pickLine.gameObject.SetActive(true);
        previouslySpawnedObject = obj;
    }

    void DespawnPreviousObject()
    {
        if (previouslySpawnedObject)
        {
            if (objectParticles)
            {
                objectParticles.transform.parent = spawnPoint.transform;
            }

            if (previouslySpawnedObject.GetComponent<SpawnedPuzzleObject>())
                previouslySpawnedObject.GetComponent<SpawnedPuzzleObject>().DestroyObject();
            else
                Destroy(previouslySpawnedObject);
        }
    }

    void DrawQuadraticBezierCurve(LineRenderer line, Vector3 point0, Vector3 point1, Vector3 point2)
    {
        line.positionCount = 20;
        float t = 0f;
        Vector3 B = new Vector3(0, 0, 0);
        for (int i = 0; i < line.positionCount; i++)
        {
            B = (1 - t) * (1 - t) * point0 + 2 * (1 - t) * t * point1 + t * t * point2;
            line.SetPosition(i, B);
            t += (1 / (float)line.positionCount);
        }
    }
}
