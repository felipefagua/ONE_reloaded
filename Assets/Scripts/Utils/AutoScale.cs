using UnityEngine;
using System.Collections;

public class AutoScale : MonoBehaviour {
    public Vector3 Scale;
    public float Time;

	// Use this for initialization
	void Start () {
        iTween.ScaleTo(this.gameObject, iTween.Hash("scale", Scale, "time", Time, "looptype", iTween.LoopType.loop));
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
