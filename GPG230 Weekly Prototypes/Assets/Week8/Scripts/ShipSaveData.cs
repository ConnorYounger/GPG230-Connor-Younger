using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipSaveData : MonoBehaviour
{
    public int shipHull;

    [System.Serializable]
    public struct weaponSlotData
    {
        public string weaponName;
        public int weaponLevel;
    }
    public weaponSlotData[] primaryWeapons;
    public weaponSlotData[] secondaryWeapons;

    void Start()
    {
        
    }

    void Update()
    {
        
    }
}
