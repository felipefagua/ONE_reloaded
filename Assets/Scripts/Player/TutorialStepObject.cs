using UnityEngine;
using System.Collections;

public class TutorialStepObject : MonoBehaviour {

	[SerializeField]
	private GameObject[] stepComponents;

	public void SetActive (bool state)
	{
		this.gameObject.SetActive (state);

		foreach (GameObject stepComponent in stepComponents) 
		{
			stepComponent.SetActive (state);
		}
	}
}
