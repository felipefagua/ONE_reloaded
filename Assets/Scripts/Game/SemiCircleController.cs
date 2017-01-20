using UnityEngine;
using System.Collections;

public class SemiCircleController : MonoBehaviour {
    public Vector3 Direction;

    private Quaternion _startRotation;

   // Use this for initialization
	void Awake () {
        _startRotation = transform.rotation;
    }
	
	// Update is called once per frame
	void Update () {
        Direction = transform.GetChild(1).position - transform.GetChild(0).position;        
    }
    
    public void InputRotate(float angle) {
        Quaternion rotation = transform.rotation;
        Vector3 eulerAngles = rotation.eulerAngles;
        eulerAngles.z += angle;
        rotation.eulerAngles = eulerAngles;
        transform.rotation = rotation;
        //this.transform.rotation = this.transform.rotation * Quaternion.AngleAxis(angle, Vector3.forward);   
    }

    public void ResetRotation() {
        transform.rotation = _startRotation;
    }
}
