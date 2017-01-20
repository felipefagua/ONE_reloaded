using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;

public class LoadingScene : MonoBehaviour {
    AsyncOperation async;

	// Use this for initialization
	void Start () {
        Time.timeScale = 1;
        this.StartCoroutine(LoadGameCoroutine());
	}
	
	// Update is called once per frame
	void Update () {
        if(async != null)
            GameObject.Find("Percent").GetComponent<Text>().text = string.Format("{0:###}%",async.progress * 100);
	}

    IEnumerator LoadGameCoroutine()
    {
        yield return new WaitForSeconds(2);
        async = SceneManager.LoadSceneAsync(Persistence.SelectedGameMode, LoadSceneMode.Single);

        yield return async;
        Debug.Log("Loading complete");
    }
}
