using UnityEngine;
using System.Collections;

public class ScreenObjectFollower : MonoBehaviour {

    [SerializeField]
    private Camera _camera;

    [SerializeField]
    private float _forcedZ = 0;

    [SerializeField]
    private GameObject _2dObject;

    private Transform _myTransform;

    private void Start() {
        _myTransform = transform;
    }

    private void Update() {
        if (_2dObject != null) {
            Vector3 screenObjectScreenPosition = _2dObject.transform.position;
            screenObjectScreenPosition.z = Camera.main.transform.position.z;

            Vector3 screenObjectWorldPosition = _camera.ScreenToWorldPoint(_2dObject.transform.position);// * -1;

            screenObjectWorldPosition.z = _forcedZ;

            _myTransform.position = screenObjectWorldPosition;
        }
    }

    public void Hide() {
        gameObject.SetActive(false);
    }

    public void Show() {
        gameObject.SetActive(true);
    }
}
