using UnityEngine;
//using GooglePlayGames;
//using GooglePlayGames.BasicApi;
using UnityEngine.SocialPlatforms;
using System.Collections;

public class GooglePlay : MonoBehaviour {

	// Use this for initialization
	void Start () {
        Initialize();

        Social.localUser.Authenticate((bool success) => {
            // handle success or failure
            if(success) Debug.Log("Login OK");
            else Debug.Log("Login Fallo");
        });
	}
	
	// Update is called once per frame
	public void Update () {
	
	}

    void Initialize()
    {
        /*PlayGamesClientConfiguration config = new PlayGamesClientConfiguration.Builder().Build();

        PlayGamesPlatform.InitializeInstance(config);
        // recommended for debugging:
        PlayGamesPlatform.DebugLogEnabled = true;
        // Activate the Google Play Games platform
        PlayGamesPlatform.Activate();
        */

    }
}
