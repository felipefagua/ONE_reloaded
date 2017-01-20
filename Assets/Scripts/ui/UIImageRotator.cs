using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class UIImageRotator : MonoBehaviour {

    [SerializeField]
    private Image[] _images;

    [SerializeField]
    private Vector3[] _rotationSpeed;

    [SerializeField]
    private bool _rotate;
    
    private void Update () {
        if (_rotate)
            Rotate();
	}

    public void StopRotation(bool reset = false) {
        if (reset)
            Reset();
        _rotate = false;
    }

    public void StartRotation(bool reset = false) {
        if (reset)
            Reset();
        _rotate = true;
    }

    private void Reset() {
        int m = _images.Length;
        for (int i = 0; i < m; i++) {
            Transform imageTransform = _images[i].transform;
            imageTransform.rotation = new Quaternion();
        }
    }

    private void Rotate() {
        int m = _images.Length;
        for (int i = 0; i < m; i++) {
            Transform imageTransform = _images[i].transform;
            Vector3 speed = _rotationSpeed[i] * Time.deltaTime;
            imageTransform.Rotate(speed);
        }
    }
}
