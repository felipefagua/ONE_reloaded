using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class LogoController : MonoBehaviour {

	// Use this for initialization
	void Start () {
        this.Invoke("BeginGame", 6.0f);
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void BeginGame()
    {
        SceneManager.LoadScene("PostTitle", LoadSceneMode.Single);
    }
}
