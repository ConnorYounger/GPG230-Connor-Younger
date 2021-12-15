using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class PlayerShipLoad : MonoBehaviourPunCallbacks
{
    public PlayerShipHealth playerHealth;
    public ShipWeaponManager weaponsManager;
    public MeshRenderer defultShipRenderer;
    public GameObject[] ships;

    [Header("Ship Weapons")]
    public ShipWeapon[] ship0PWeapons;
    public ShipWeapon[] ship1PWeapons;
    public ShipWeapon[] ship2PWeapons;
    public ShipWeapon[] ship3PWeapons;

    public ShipWeapon ship0SecondaryWeapon;
    public ShipWeapon ship1SecondaryWeapon;
    public ShipWeapon ship2SecondaryWeapon;
    public ShipWeapon ship3SecondaryWeapon;

    private PhotonView photonView;
    public int currentShip;

    private void Awake()
    {
        photonView = gameObject.GetComponent<PhotonView>();
    }

    void Start()
    {
        if (photonView == null)
        {
            SetCurrentShip();
        }
        else
        {
            if (photonView.IsMine)
            {
                Debug.Log("Load Ship: Photon View Is Mine");
                photonView.RPC("SetCurrentShip", RpcTarget.All);
            }
            else
            {
                LoadShip();
            }
        }
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        if (photonView.IsMine)
        {
            Debug.Log("Load Ship: Photon View Is Mine");
            photonView.RPC("SetCurrentShip", RpcTarget.All);
        }
        else
        {
            LoadShip();
        }

        //base.OnPlayerEnteredRoom(newPlayer);
    }

    [PunRPC]
    void SetCurrentShip()
    {
        Debug.Log("Set Current Ship, has PhotonView");

        PlayerData data = SaveSystem.LoadLevel(W8SaveData.savePath);
        currentShip = data.currentShip;

        LoadShip();
    }

    void LoadShip()
    {
        Debug.Log("Load Ship: Current Ship = " + currentShip);

        weaponsManager.primaryWeapons = new List<ShipWeapon>();

        if (currentShip == 0)
        {
            defultShipRenderer.enabled = true;

            playerHealth.SetStartingHealth(50);

            for(int i = 0; i < ship0PWeapons.Length; i++)
            {
                weaponsManager.primaryWeapons.Add(ship0PWeapons[i]);
            }
            weaponsManager.secondaryWeapons[0] = ship0SecondaryWeapon;
        }
        else
        {
            defultShipRenderer.enabled = false;
        }

        int health = 100;

        for (int i = 1; i < 4; i++)
        {
            if (currentShip == i)
            {
                ships[i].SetActive(true);
                playerHealth.SetStartingHealth(health);
            }
            else
            {
                ships[i].SetActive(false);
                health += 100;
            }
        }

        switch (currentShip)
        {
            case 1:
                for (int i = 0; i < ship1PWeapons.Length; i++)
                {
                    weaponsManager.primaryWeapons.Add(ship1PWeapons[i]);
                    ship1PWeapons[i].weaponIndex = i;
                }
                weaponsManager.secondaryWeapons[0] = ship1SecondaryWeapon;
                break;
            case 2:
                for (int i = 0; i < ship2PWeapons.Length; i++)
                {
                    weaponsManager.primaryWeapons.Add(ship2PWeapons[i]);
                    ship2PWeapons[i].weaponIndex = i;
                }
                weaponsManager.secondaryWeapons[0] = ship2SecondaryWeapon;
                break;
            case 3:
                for (int i = 0; i < ship3PWeapons.Length; i++)
                {
                    weaponsManager.primaryWeapons.Add(ship3PWeapons[i]);
                    ship3PWeapons[i].weaponIndex = i;
                }
                weaponsManager.secondaryWeapons[0] = ship3SecondaryWeapon;
                break;
        }


        //if (weaponsManager.photonView)
        //{
        //    for (int i = 0; i < weaponsManager.primaryWeapons.Count; i++)
        //    {
        //        weaponsManager.weaponDirX.Add(new float());
        //        weaponsManager.weaponDirY.Add(new float());
        //        weaponsManager.weaponDirZ.Add(new float());
        //        weaponsManager.weaponDirW.Add(new float());
        //    }
        //}
    }
}
