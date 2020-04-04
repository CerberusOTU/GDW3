using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class afterGameScript : MonoBehaviour
{
    private Text winner1;
    private Text winner2;
    public Text Victory;

    // Start is called before the first frame update
    void Start()
    {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;


    }

   void Update()
    {
        Victory.text = winner1.ToString() + " and " + winner2.ToString() + " win!";
    }

}
