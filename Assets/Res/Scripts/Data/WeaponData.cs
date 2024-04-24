using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "WeaponData", menuName = "Data/Weapon", order = 1)]
public class WeaponData : ScriptableObject
{
    public int AmmoCapacity;
    public int LeftoverAmmo;
    public float Damage;
    public float FireRate;
    public bool isPierceShot;
}
