using UnityEngine;
using System.Collections;

public class AutoRotate : MonoBehaviour {
    public float AngularSpeed;
    public Vector3 Axis = Vector3.forward;
    public int Dir = 1;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        this.transform.Rotate(Axis, Dir * AngularSpeed * Time.deltaTime);	
	}
}
