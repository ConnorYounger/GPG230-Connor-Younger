using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class PhotonPlayerManager : MonoBehaviourPunCallbacks, IPunObservable
{
    #region IPunObservable implementation

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            // We own this player: send the others our data
            stream.SendNext(transformRotationX);
            stream.SendNext(transformRotationY);
            stream.SendNext(transformRotationZ);
            stream.SendNext(transformRotationW);
            //stream.SendNext(Health);
        }
        else
        {
            // Network player, receive data
            this.transformRotationX = (float)stream.ReceiveNext();
            this.transformRotationY = (float)stream.ReceiveNext();
            this.transformRotationZ = (float)stream.ReceiveNext();
            this.transformRotationW = (float)stream.ReceiveNext();
            //this.Health = (float)stream.ReceiveNext();
        }
    }

    #endregion

    private PhotonView photonView;

    [Header("Player Refs")]
    public GameObject[] selfObjects;
    public W8ShipMovement shipMovement;
    public ShipWeaponManager weaponManager;
    public Transform shipRotation;
    public float syncUpdateSpeed = 6;
    public PauseManager pauseManager;

    public float transformRotationX;
    public float transformRotationY;
    public float transformRotationZ;
    public float transformRotationW;

    void Start()
    {
        photonView = gameObject.GetComponent<PhotonView>();

        if (GameObject.Find("PauseManager"))
            pauseManager = GameObject.Find("PauseManager").GetComponent<PauseManager>();

        foreach (GameObject ob in selfObjects)
        {
            if (photonView.IsMine)
            {
                ob.SetActive(true);

                if (pauseManager)
                {
                    pauseManager.shipMovement = shipMovement;
                    pauseManager.weaponManager = weaponManager;
                }
            }
            else
            {
                ob.SetActive(false);
            }
        }

        if (photonView.IsMine)
        {
            shipMovement.enabled = true;
            weaponManager.enabled = true;
        }
        else
        {
            shipMovement.enabled = false;
            weaponManager.enabled = false;
        }
    }

    private void Update()
    {
        if (photonView.IsMine)
        {
            transformRotationX = shipRotation.rotation.x;
            transformRotationY = shipRotation.rotation.y;
            transformRotationZ = shipRotation.rotation.z;
            transformRotationW = shipRotation.rotation.w;
        }
        else
        {
            //shipRotation.rotation = new Quaternion(transformRotationX, transformRotationY, transformRotationZ, transformRotationW);
            Quaternion newRot = new Quaternion(transformRotationX, transformRotationY, transformRotationZ, transformRotationW);
            shipRotation.rotation = Quaternion.Slerp(shipRotation.rotation, newRot, syncUpdateSpeed * Time.deltaTime);
        }
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        if (photonView.IsMine)
        {
            photonView.RPC("PhotonSetsCurrentShip", RpcTarget.All);
        }
        else
        {
            SetShipTrails();
        }

        //base.OnPlayerEnteredRoom(newPlayer);
    }

    [PunRPC]
    public void PhotonSetsCurrentShip()
    {
        PlayerData data = SaveSystem.LoadLevel(W8SaveData.savePath);
        shipMovement.currentShip = data.currentShip;

        SetShipTrails();
    }

    public void SetShipTrails()
    {
        for (int i = 0; i < shipMovement.particlTrail.Length; i++)
        {
            if (i == shipMovement.currentShip)
            {
                shipMovement.particlTrail[i].gameObject.SetActive(true);
            }
            else
            {
                shipMovement.particlTrail[i].gameObject.SetActive(false);
            }
        }
    }
}
