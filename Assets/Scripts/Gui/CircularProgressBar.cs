using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;

public class CircularProgressBar : MonoBehaviour {

    [SerializeField]
    private Image _circularImage;

    [SerializeField]
    private Sprite _completeSprite;

    [SerializeField]
    private float _alpha;

    [SerializeField]
    private float _progressMinValue;

    [SerializeField]
    private float _progressMaxValue;

    [SerializeField]
    private float _currentProgressValue;

    [SerializeField]
    private float _animationTime;

    [SerializeField]
    private float _elapsedTime;

    [SerializeField]
    private float _targetAlpha;

    [SerializeField]
    private bool _isPlayingAnimation;

    [SerializeField]
    private Text _text;

    [SerializeField]
    private TextFormat _textFormat;

    // Update is called once per frame
	void Update () {
        if (_isPlayingAnimation)
            UpdateAnimation();

        UpdateFillAmount();
        UpdateProgressValue();

        if (_text != null)
            UpdateProgressText();
    }

    private void UpdateAnimation() {
        _elapsedTime += Time.deltaTime;

        if (_elapsedTime >= _animationTime)
            StopAnimation();

        float alphaAnim = _elapsedTime / _animationTime;
        _alpha = alphaAnim * _targetAlpha;

        if (_alpha >= 1)
            SetCompleteSprite();
    }

    private void StopAnimation() {
        _isPlayingAnimation = false;
        _elapsedTime = _animationTime;        
    }

    private void SetCompleteSprite() {
        if (_completeSprite != null) 
            _circularImage.sprite = _completeSprite;        
    }

    private void UpdateFillAmount() {
        _alpha = Mathf.Clamp(_alpha, 0, 1);
        _circularImage.fillAmount = _alpha;
    }

    private void UpdateProgressValue() {
        _currentProgressValue = _progressMinValue + ((_progressMaxValue - _progressMinValue) * _alpha);
        
    }

    public void SetProgressRange(float minValue, float maxValue) {
        _progressMinValue = minValue;
        _progressMaxValue = maxValue;
    }

    public void StartAnimation(float targetAlpha) {
        _elapsedTime = 0;
        _targetAlpha = targetAlpha;
        _isPlayingAnimation = true;
    }

    private void UpdateProgressText() {
        string progressText = GetTextFormat();
        _text.text = progressText;
    }

    private string GetTextFormat() {
        switch (_textFormat) {
            case TextFormat.integer:
                return GetIntegerFormat();                
            case TextFormat.time:
                return GetTimedFormat();                
        }
        return "" + _currentProgressValue;
    }

    private string GetTimedFormat() {
        TimeSpan timeSpan = TimeSpan.FromMilliseconds((double)_currentProgressValue);

        string timeFormatedString = string.Format(  "{0:D2}:{1:D2}:{2:D3}",
                                                    timeSpan.Minutes, timeSpan.Seconds, timeSpan.Milliseconds);

        return timeFormatedString;
    }

    private string GetIntegerFormat() {
        int roundedValue = Mathf.RoundToInt(_currentProgressValue);
        return "" + roundedValue;
    }
}
