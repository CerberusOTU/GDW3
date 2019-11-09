using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.InteropServices;

public class PluginReader : MonoBehaviour
{
   const string DLL_NAME = "Plugin";
   [DllImport(DLL_NAME)]
   private static extern int Test();
   [DllImport(DLL_NAME)]
   private static extern void LogMetrics(Vector3 _position, float _accuracy, float _time);

   void Start()
   {

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
         LogMetrics(this.transform.position, 0.0f, 0.0f);
      }
      
      
   }
}