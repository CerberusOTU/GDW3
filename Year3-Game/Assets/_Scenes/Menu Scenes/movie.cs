using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class movie : MonoBehaviour {
	
    public string sceneName;
    float timer = 0.0f;

	// Use this for initialization
	void Start () {
		((MovieTexture)GetComponent<Renderer>().material.mainTexture).Play ();


	}

    void Update()
    {
        timer += Time.deltaTime;

        Debug.Log("MOVIE TIME = " + timer);

        if (timer > 35)
        SceneManager.LoadScene(sceneName);
    }

}