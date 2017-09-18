using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Analytics;

public class SimpleGameAnalytics : MonoBehaviour, IGameAnalytics {

	private PlayerController player;

	private int lifesGained = 0;

	private void Start()
	{
		player = GameObject.FindWithTag("Player").GetComponent<PlayerController>();
	}

	public void GainedALife()
	{
		lifesGained++;
	}

	public void ExitSession()
	{
		Analytics.CustomEvent (CustomEventTypes.SINGLEPLAYER, new Dictionary<string, object> {
			{CustomEventTypes.FORFEITGAME, 1},
			{CustomEventTypes.FORFEITTIME, player.GameTime},
			{CustomEventTypes.FORFEITSCORE, player.TotalScore},
			{CustomEventTypes.FORFEITCOMBO, player.TotalExperience},
			{CustomEventTypes.FORFEITLIFESGAINED, lifesGained}
		});
	}

	public void GameOver()
	{
		Analytics.CustomEvent (CustomEventTypes.SINGLEPLAYER, new Dictionary<string, object> {
			{CustomEventTypes.COMPLETEDGAME, 1},
			{CustomEventTypes.ENDGAMETIME, player.GameTime},
			{CustomEventTypes.ENDGAMESCORE, player.TotalScore},
			{CustomEventTypes.ENDGAMECOMBO, player.TotalExperience},
			{CustomEventTypes.ENDGAMELIFESGAINED, lifesGained}
		});
	}
}
