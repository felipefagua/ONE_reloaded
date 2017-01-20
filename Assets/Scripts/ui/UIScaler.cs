using UnityEngine;
using System.Collections;
using System;

public class UIScaler : MonoBehaviour {

    [SerializeField]
    private float _gameScreenHeight = 0f;

    [SerializeField]
    private float _gameScreenWidth = 0f;

    [SerializeField]
    private float _gameStartScale = 0f;

    [SerializeField]
    private float _deviceScreenHeight = 0f;

    [SerializeField]
    private bool _scaleOnUpdate = false;

    private Transform _myTransform;

    // Use this for initialization
    void Start () {
        _myTransform = transform;
        Scale();
	}
	
	// Update is called once per frame
	void Update () {
        if(_scaleOnUpdate)
            Scale();
    }

    private void Scale() {
        _deviceScreenHeight = Screen.height;
        float newScale = (_deviceScreenHeight/ _gameScreenHeight);
        float newHeight = _gameScreenHeight * newScale;
        float newWidth = _gameScreenWidth * newScale;

        //_myTransform.localScale = new Vector3(newScale, newScale, newScale);
        RectTransform rectTraansform = (RectTransform)_myTransform;
        rectTraansform.sizeDelta = new Vector2(newWidth, newHeight);
    }
}
