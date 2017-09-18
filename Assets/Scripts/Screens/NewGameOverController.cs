using UnityEngine;
using System.Collections;
using System;

public class NewGameOverController : MonoBehaviour {

    [SerializeField]
    private CircularProgressBar _timeProgressBar;

    [SerializeField]
    private CircularProgressBar _scoreProgressBar;

    [SerializeField]
    private CircularProgressBar _experienceProgressBar;

    [SerializeField]
    private float _displayAnimationsDelay;

	private GameOverAnalytics analytics;

    // Use this for initialization
    void Start () {
        Invoke("ShowAnimations", _displayAnimationsDelay);
		analytics = GameObject.Find("Analytics").GetComponent<GameOverAnalytics>();
	}

    private void ShowAnimations() {
        PlayData topGameData = Persistence.Data.TopGame;
        PlayData lastGameData = Persistence.Data.LastGame;

		if (lastGameData.IsHighScore)
			analytics.HighScore (lastGameData.Score);

        StartAnimation(_timeProgressBar, 1000 * lastGameData.Time, 1000 * topGameData.Time);
        StartAnimation(_scoreProgressBar, lastGameData.Score, topGameData.Score);
        StartAnimation(_experienceProgressBar, lastGameData.Experience, topGameData.Experience);        
    }

    private void StartAnimation(CircularProgressBar progressBar, float lastGameData, float topGameData) {
        progressBar.SetProgressRange(0, topGameData);
		if(topGameData == 0)
			progressBar.StartAnimation(0);
		else
			progressBar.StartAnimation(lastGameData / topGameData);
    }
}
