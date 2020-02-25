using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class weaponInfo : MonoBehaviour
{
    public _Gun gun;
    public int maxAmmo, currentAmmo;

    void Start()
    {
        maxAmmo = gun.alwaysMax;
        currentAmmo = gun.clipSize;
    }
}
