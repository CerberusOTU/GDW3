using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Gun", menuName = "Gun")]
public class _Gun : ScriptableObject
{
    public int weaponID;
    //0   M1911
    //1   TommyGun
    //2   MP40
    //3   Shotgun
    //4   Revolver

    public string name;
    public string className;
    public string ShotType;
    public float damage;
    public float firerate;
    public float bloom;
    public float recoil;
    public float maxRecoil_x;
    public float recoilSpeed;
    public float recoilDampen; //Less is more
    public float kickBack;
    public float aimSpeed;

    //Ammo & reloading
    public float reloadTime;
    public int alwaysMax;
    public int clipSize;
    public bool isReloading;
    public int currentAmmo;
    public int maxAmmo;

    //Sound
    public string ShotPath;
    public string ReloadPath;

    public GameObject weaponObj;
    public GameObject weaponObj_Arms;
}
