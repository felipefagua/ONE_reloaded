using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Advertisements;
using UnityEngine.SceneManagement;
using ChartboostSDK;
using System.Collections;
using System;

public class MyScoreController : MonoBehaviour
{
    public Text TextScore;
    public Text TextTime;
    public Text TextExperience;
    public Text HighScore;


    // Use this for initialization
    void Start()
    {
        this.transform.localScale = Vector3.zero;
        Time.timeScale = 1;

        Chartboost.cacheInterstitial(CBLocation.Default);

        HighScore.transform.localScale = Vector3.zero;
        this.Invoke("ShowScore", 1.0f);
    }
	
    // Update is called once per frame
    void Update()
    {

    }


    void ShowScore()
    {
        this.transform.localScale = Vector3.one;
        HighScore.transform.localScale = Vector3.zero;

		this.StartCoroutine(OnTextCoroutine(TextScore, Persistence.Data.TopGame.Score, "number"));
		this.StartCoroutine(OnTextCoroutine(TextTime, Persistence.Data.TopGame.Time, "time"));
		this.StartCoroutine(OnTextCoroutine(TextExperience, Persistence.Data.TopGame.Experience, "float"));
    }

    IEnumerator OnTextCoroutine(Text text, float value, string type)
    {
        iTween.ScaleTo(text.gameObject, iTween.Hash(
                "scale", Vector3.one * 1.05f, 
                "time", 0.5f));

        int step = (int)value / 5;
        
        for (int i = 0; i < 5; i++)
        {
            switch (type)
            {
                case "number":
                    text.GetComponent<Text>().text = (i * step).ToString();            
                    break;
                case "float":
                    text.GetComponent<Text>().text = value.ToString("0.00");            
                    break;
                case "time":
                    TimeSpan time = TimeSpan.FromSeconds(i * step);
                    text.GetComponent<Text>().text = time.ToString();            
                    break;
            }            
             
            yield return new WaitForSeconds(0.1f);
        }        

        iTween.ScaleTo(text.gameObject, iTween.Hash(
                "scale", Vector3.one, 
                "time", 0.5f));        

        switch (type)
        {
            case "number":
                text.GetComponent<Text>().text = value.ToString();            
                break;
            case "float":
                text.GetComponent<Text>().text = value.ToString("0.00");            
                break;
            case "time":
                TimeSpan time = TimeSpan.FromSeconds(value);
                text.GetComponent<Text>().text = time.ToString();            
                break;
        } 

        if (Persistence.Data.LastGame.IsHighScore)
        {
            HighScore.transform.localScale = Vector3.one;
        }
    }

    public void TryAgain()
    {
        Chartboost.showInterstitial(CBLocation.Default);
        this.StartCoroutine(OnAction("tryagain"));
    }

    public void MainMenu()
    {
        if (Advertisement.IsReady())
            Advertisement.Show();

        this.StartCoroutine(OnAction("mainmenu"));
    }

    IEnumerator OnAction(string action)
    {
        yield return new WaitForSeconds(0.5f);
        switch(action)
        {
		case "tryagain":  
			SceneManager.UnloadScene (Persistence.SelectedGameMode);
            SceneManager.LoadScene("Loading", LoadSceneMode.Single);
                break;
      	case "mainmenu":
                SceneManager.UnloadScene(Persistence.SelectedGameMode);
                SceneManager.LoadScene("main_menu", LoadSceneMode.Single);
                break;
        }
    }
}

