using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class W8ShipMovement : MonoBehaviour
{

    float noTurn = 0.0f; // Extent of the no-turn zone as a fraction of Screen.height;
    float factor = 150.0f;
    private Vector3 center;

    public float cruseSpeed = 10;
    public float strafeSpeed = 60;
    public float movementSpeed = 100;

    public Transform shipDirection;

    public Rigidbody rb;

    public Transform aimReticle;
    private Vector3 reticleTargetPos;

    public Transform[] particlTrail;
    public AudioSource thrusterAudioSource;

    public PhotonView photonView;
 
    void Start()
    {
        center = new Vector3(Screen.width / 2, Screen.height / 2, 0);
        Cursor.lockState = CursorLockMode.Confined;

        if(photonView != null)
        {
            if (photonView.IsMine)
            {
                aimReticle = GameObject.Find("TargetReticle").transform;

                photonView.RPC("SetShipTrail", RpcTarget.All);
            }
        }
        else
        {
            SetShipTrail();
        }
    }

    [PunRPC]
    void SetShipTrail()
    {
        PlayerData data = SaveSystem.LoadLevel(W8SaveData.savePath);

        for (int i = 0; i < particlTrail.Length; i++)
        {
            if (i == data.currentShip)
            {
                particlTrail[i].gameObject.SetActive(true);
            }
            else
            {
                particlTrail[i].gameObject.SetActive(false);
            }
        }
    }

    void Update()
    {
        ShipMovement();
        ReticleAiming();
        ShipTurning();
        ShipTrail();
    }

    void ShipMovement()
    {
        //transform.position += shipDirection.forward * cruseSpeed * Time.deltaTime;

        //transform.position += shipDirection.forward * Input.GetAxis("Vertical") * movementSpeed * Time.deltaTime;
        //transform.position += shipDirection.right * Input.GetAxis("Horizontal") * strafeSpeed * Time.deltaTime;

        rb.AddForce(shipDirection.forward * cruseSpeed * Time.deltaTime);
        rb.AddForce(shipDirection.forward * Input.GetAxis("Vertical") * movementSpeed * Time.deltaTime);
        rb.AddForce(shipDirection.right * Input.GetAxis("Horizontal") * strafeSpeed * Time.deltaTime);

        shipDirection.Rotate(-Vector3.forward * Input.GetAxis("Roll") * cruseSpeed * Time.deltaTime);
    }

    void ShipTrail()
    {
        if (particlTrail.Length > 0)
        {
            if (Input.GetAxis("Vertical") != 0 || Input.GetAxis("Horizontal") != 0)
            {
                ThrustOn();
            }
            else
            {
                ThrustOff();
            }
        }
    }

    void ThrustOn()
    {
        PlayerData data = SaveSystem.LoadLevel(W8SaveData.savePath);

        foreach (Transform t in particlTrail[data.currentShip])
        {
            if (t.GetComponent<ParticleSystem>())
            {
                t.GetComponent<ParticleSystem>().Play();
            }
        }

        if (thrusterAudioSource && !thrusterAudioSource.isPlaying)
        {
            thrusterAudioSource.Play();
        }
    }

    void ThrustOff()
    {
        PlayerData data = SaveSystem.LoadLevel(W8SaveData.savePath);

        foreach (Transform t in particlTrail[data.currentShip])
        {
            if (t.GetComponent<ParticleSystem>())
            {
                t.GetComponent<ParticleSystem>().Stop();
            }
        }

        if (thrusterAudioSource && thrusterAudioSource.isPlaying)
        {
            thrusterAudioSource.Stop();
        }
    }

    void ReticleAiming()
    {
        if(aimReticle)
            aimReticle.position = Input.mousePosition;
    }

    void ShipTurning()
    {
        var delta = (Input.mousePosition - center) / Screen.height;
        //Debug.Log(delta);
        if (delta.y > noTurn)
            shipDirection.Rotate(-(delta.y - noTurn) * Time.deltaTime * factor, 0, 0);
        if (delta.y < -noTurn)
            shipDirection.Rotate(-(delta.y + noTurn) * Time.deltaTime * factor, 0, 0);
        if (delta.x > noTurn)
            shipDirection.Rotate(0, (delta.x - noTurn) * Time.deltaTime * factor, 0);
        if (delta.x < -noTurn)
            shipDirection.Rotate(0, (delta.x + noTurn) * Time.deltaTime * factor, 0);
    }
}
