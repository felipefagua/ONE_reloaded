using UnityEngine;
using System.Collections;
using System;
using UnityEngine.UI;

public class UIFader : MonoBehaviour {

    [SerializeField]
    private FaderState _state;

    [SerializeField]
    private FaderState _stateOnStart;

    [SerializeField]
    private float _startFadeDelay;

    [SerializeField]
    private float _alpha;

    [SerializeField]
    private float _startAlpha;

    [SerializeField]
    private float _fadeTime;

    private float _elapsedTime;

    [SerializeField]
    private bool _disableOnFadeOut;

    [SerializeField]
    private Image[] _images;

    [SerializeField]
    private SpriteRenderer[] _sprites;

    private void Start() {
        _alpha = _startAlpha;
        UpdateAlpha();
        switch (_stateOnStart) {
            case FaderState.fadeIn:
                Invoke("StartFadeIn", _startFadeDelay);                
                break;
            case FaderState.fadeOut:
                Invoke("StartFadeOut", _startFadeDelay);                
                break;
            default:
                break;
        }
    }

    public void StartFadeIn() {
        Debug.Log("Start");
        _state = FaderState.fadeIn;
        _alpha = 0;
        _elapsedTime = 0;
    }

    public void StartFadeOut() {
        _state = FaderState.fadeOut;
        _alpha = 1;
        _elapsedTime = 0;
    }

    // Update is called once per frame
    void LateUpdate () {
        UpdateState();
	}

    private void UpdateState() {
        switch (_state) {
            case FaderState.fadeIn:
                UpdateFadeIn();
                break;
            case FaderState.fadeOut:
                UpdateFadeOut();
                break;
            default:
                break;
        }
    }

    private void UpdateFadeIn() {
        UpdateTime();
        _alpha = _elapsedTime / _fadeTime;
        UpdateAlpha();
    }

    private void UpdateFadeOut() {
        UpdateTime();
        _alpha = 1 - _elapsedTime / _fadeTime;
        UpdateAlpha();
    }

    private void UpdateTime() {
        _elapsedTime += Time.deltaTime;

        if (_elapsedTime >= _fadeTime) {
            _elapsedTime = _fadeTime;
            if (_state == FaderState.fadeOut && _disableOnFadeOut)
                gameObject.SetActive(false);
            _state = FaderState.none;
        }
    }

    private void UpdateAlpha() {
        foreach (Image image in _images) {
            Color newColor = image.color;
            newColor.a = _alpha;
            image.color = newColor;
        }
        foreach (SpriteRenderer sprite in _sprites) {
            Color newColor = sprite.color;
            newColor.a = _alpha;
            sprite.color = newColor;
        }
    }
}
