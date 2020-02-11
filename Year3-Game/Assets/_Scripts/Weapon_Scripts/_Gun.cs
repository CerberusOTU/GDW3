using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Gun", menuName = "Gun")]
public class _Gun : ScriptableObject
{
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
   public int maxAmmo;
   public int clipSize;

   public int currentAmmo;

    //Sound
    public string ShotPath;
    public string ReloadPath;

   [SerializeField]
   public bool isReloading;
   //////////////
   public GameObject weaponObj;
   public GameObject weaponObj_Arms;
}
