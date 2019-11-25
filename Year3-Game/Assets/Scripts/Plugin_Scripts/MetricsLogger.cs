using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.InteropServices;

public class MetricsLogger : MonoBehaviour
{
   const string DLL_NAME = "Plugin";
   [DllImport(DLL_NAME)]
   private static extern int Test();
   [DllImport(DLL_NAME)]
   private static extern void LogMetrics(Vector3 _position, float _accuracy, float _time);

   //Metrics Values
   [System.NonSerialized]
   public float shotsTaken = 0;
   [System.NonSerialized]
   public float shotsHit = 0;
   private float weaponAccuracy = 0;

   [System.NonSerialized]
   public float startTime = 0;
   private float totalTime = 0;
   
   void Start()
   {
      //shotsHit = 0;
      //shotsTaken = 0;
      //weaponAccuracy = 0;
      startTime = Time.time;
   }

   void Update()
   {
      if (Input.GetKeyDown(KeyCode.L))
      {
         Debug.Log(Test());
      }

      //Log Metrics of Tutorial
      if (Input.GetKeyDown(KeyCode.M))
      {
         
         if (shotsTaken>0)
         {
            weaponAccuracy = (shotsHit/shotsTaken);
            Debug.Log(weaponAccuracy + "=" + shotsHit +"/" + shotsTaken);
         }
         totalTime = Time.time - startTime;
         LogMetrics(this.transform.position, weaponAccuracy, totalTime);
      }
           
   }
}