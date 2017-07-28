using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class PostTitleController : MonoBehaviour {

	void Start () {
		this.Invoke("BeginGame", 2.0f);
	}

	void BeginGame()
	{
		SceneManager.LoadScene("main_menu", LoadSceneMode.Single);
	}
}
