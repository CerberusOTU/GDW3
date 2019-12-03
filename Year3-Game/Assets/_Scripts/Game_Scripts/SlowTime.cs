using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlowTime : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
            //Time.timeScale = 0.25f;
            Time.timeScale = Mathf.Lerp(Time.timeScale, 0.25f, Time.time);
            Time.fixedDeltaTime = Time.fixedDeltaTime * Time.timeScale;  
    }

}
