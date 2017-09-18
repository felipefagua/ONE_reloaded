using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Analytics;

public class GameOverAnalytics : MonoBehaviour {

	public void HighScore(int highScore)
	{
		Analytics.CustomEvent (CustomEventTypes.GENERALSTATS, new Dictionary<string, object> {
			{CustomEventTypes.HIGHSCORE, highScore}
		});
	}
}
