using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Analytics;

public class MainMenuAnalytics : MonoBehaviour {

	public void NewGameSingle()
	{
		Analytics.CustomEvent (CustomEventTypes.SINGLEPLAYER, new Dictionary<string, object> {
			{"New Game", 1}
		});
	}

	public void NewGameMulti()
	{
		Analytics.CustomEvent (CustomEventTypes.MULTIPLAYER, new Dictionary<string, object> {
			{"New Game", 1}
		});
	}
}
