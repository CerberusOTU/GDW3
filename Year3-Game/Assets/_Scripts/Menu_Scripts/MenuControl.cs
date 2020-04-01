using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;
using UnityEngine.Playables;
public class MenuControl : MonoBehaviour
{

    public Animator transitionsAnim;
    public string sceneName;
    public PlayableDirector Timeline;
    public PlayableDirector TimelineBack;
    public Vector3 origin;

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

    public void PlayAnimation()
    {
        Timeline.Play();
    }

    public void PlayBackAnimation()
    {
        TimelineBack.Play();
    }

    public void QuitGame()
    {
        Debug.Log("Quit");
        Application.Quit();
    }

    public void Click()
    {
        FMODUnity.RuntimeManager.PlayOneShot("event:/General/Click", origin);
    }

    IEnumerator LoadScene()
    {
        transitionsAnim.SetTrigger("end");
        yield return new WaitForSeconds(0.7f);
        SceneManager.LoadScene(sceneName);


    }
}
