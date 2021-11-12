using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Ship Weapon", menuName = "Week8/ShipWeapon")]
public class ShipWeaponStats : ScriptableObject
{
    public int damage = 5;
    public float fireRate;
    public GameObject projectilePrefab;

    public float projectileLifeTime;
    public float projectileSpeed;
}
