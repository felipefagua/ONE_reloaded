using UnityEngine;
using System.Collections;
using UnityEngine.Analytics;
using System.Collections.Generic;

public class MultiplayerGameAnalytics : MonoBehaviour, IGameAnalytics {

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
		Analytics.CustomEvent (CustomEventTypes.MULTIPLAYER, new Dictionary<string, object> {
			{CustomEventTypes.FORFEITGAME, 1},
			{CustomEventTypes.FORFEITTIME, player.GameTime},
			{CustomEventTypes.FORFEITSCORE, player.TotalScore},
			{CustomEventTypes.FORFEITCOMBO, player.TotalExperience}
		});
	}

	public void GameOver()
	{
		Analytics.CustomEvent (CustomEventTypes.MULTIPLAYER, new Dictionary<string, object> {
			{CustomEventTypes.COMPLETEDGAME, 1},
			{CustomEventTypes.ENDGAMETIME, player.GameTime},
			{CustomEventTypes.ENDGAMESCORE, player.TotalScore},
			{CustomEventTypes.ENDGAMECOMBO, player.TotalExperience}
		});
	}
}
