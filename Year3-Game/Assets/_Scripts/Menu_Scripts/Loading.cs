﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Loading : MonoBehaviour
{
    [SerializeField]
    private Image progressBar;

    public string sceneName;

    // Start is called before the first frame update
    void Start()
    {
        //start async 
        StartCoroutine(LoadOperation());
    }

    IEnumerator LoadOperation()
    {
        AsyncOperation gameLevel = SceneManager.LoadSceneAsync(sceneName);
        
        while (gameLevel.progress < 1)
        {
            progressBar.fillAmount = gameLevel.progress;
            yield return new WaitForEndOfFrame();
        }
    }
}
