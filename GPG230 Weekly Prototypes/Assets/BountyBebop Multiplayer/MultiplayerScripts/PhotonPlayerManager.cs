using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class PhotonPlayerManager : MonoBehaviour, IPunObservable
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
            //stream.SendNext(Health);
        }
        else
        {
            // Network player, receive data
            this.transformRotationX = (float)stream.ReceiveNext();
            this.transformRotationY = (float)stream.ReceiveNext();
            this.transformRotationZ = (float)stream.ReceiveNext();
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

    public float transformRotationX;
    public float transformRotationY;
    public float transformRotationZ;

    void Start()
    {
        photonView = gameObject.GetComponent<PhotonView>();

        foreach(GameObject ob in selfObjects)
        {
            if (photonView.IsMine)
            {
                ob.SetActive(true);
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
        }
        else
        {
            shipRotation.rotation = new Quaternion(transformRotationX, transformRotationY, transformRotationZ, 1);
        }
    }
}
