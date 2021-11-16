using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New ShipSaveData", menuName = "Week8/ShipSaveData", order = 2)]
[System.Serializable]
public class ShipSaveData : ScriptableObject
{
    public int shipHull;

    [System.Serializable]
    public struct weaponSlotData
    {
        public string weaponName;
        public int weaponLevel;
    }
    public weaponSlotData primaryWeapon;
    public weaponSlotData secondaryWeapon;
}
