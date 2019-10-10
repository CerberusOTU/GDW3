using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Gun", menuName = "Gun")]
public class _Gun : ScriptableObject
{
   public string name;
   public float firerate;
   public float bloom;
   public float recoil;
   public float maxRecoil_x;
   public float recoilSpeed;
   public float kickBack;
   public float aimSpeed;
   public GameObject obj;
}
