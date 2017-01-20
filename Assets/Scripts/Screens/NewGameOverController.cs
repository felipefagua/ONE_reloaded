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

    // Use this for initialization
    void Start () {
        Invoke("ShowAnimations", _displayAnimationsDelay);
	}

    private void ShowAnimations() {
        PlayData topGameData = Persistence.Data.TopGame;
        PlayData lastGameData = Persistence.Data.LastGame;

        StartAnimation(_timeProgressBar, 1000 * lastGameData.Time, 1000 * topGameData.Time);
        StartAnimation(_scoreProgressBar, lastGameData.Score, topGameData.Score);
        StartAnimation(_experienceProgressBar, lastGameData.Experience, topGameData.Experience);        
    }

    private void StartAnimation(CircularProgressBar progressBar, float lastGameData, float topGameData) {
        progressBar.SetProgressRange(0, topGameData);
        progressBar.StartAnimation(lastGameData / topGameData);
    }
}
