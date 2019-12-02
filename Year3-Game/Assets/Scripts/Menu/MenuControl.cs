using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class MenuControl : MonoBehaviour
{

    public Animator transitionsAnim;
    public string sceneName;

    void Start()
    {    
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }



    // Update is called once per frame
    public void PlayGame()
    {
        StartCoroutine(LoadScene());
    }

    public void QuitGame()
    {
        Debug.Log("Quit");
        Application.Quit();
    }

    IEnumerator LoadScene()
    {
        transitionsAnim.SetTrigger("end");
        yield return new WaitForSeconds(0.7f);
        SceneManager.LoadScene(sceneName);


    }
}
