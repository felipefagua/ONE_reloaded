using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;

public class OptionsMenuController : MonoBehaviour {

    [SerializeField]
    public bool _isShowing;

    [SerializeField]
    public UIImageRotator _rotator;

    [SerializeField]
    public Button[] _optionsButtons;

    [SerializeField]
    public Image _lblMusicOn;

    [SerializeField]
    public Image _lblMusicOff;

    [SerializeField]
    public Image _lblSoundOn;

    [SerializeField]
    public Image _lblSoundOff;

    private void Start() {
        //InitMusicButton();
        //InitSoundButton();
        HideOptionsButtons();
    }

    private void InitMusicButton() {
        _lblMusicOn.gameObject.SetActive(true);
        _lblMusicOff.gameObject.SetActive(false);
    }

    private void InitSoundButton() {
        _lblSoundOn.gameObject.SetActive(true);
        _lblSoundOff.gameObject.SetActive(false);
    }

    public void Toggle() {
        _isShowing = !_isShowing;
        if (_isShowing)
            Show();
        else
            Hide();
    }

    private void Show() {
        _rotator.StartRotation(true);
        ShowOptionsButtons();
    }

    private void Hide() {
        HideOptionsButtons();
        _rotator.StopRotation(true);
    }

    private void ShowOptionsButtons() {
        SetActiveOptionsButtons(true);
    }
    
    private void HideOptionsButtons() {
        SetActiveOptionsButtons(false);
    }

    private void SetActiveOptionsButtons(bool active) {
        foreach (Button button in _optionsButtons)
            button.gameObject.SetActive(active);
    }

    public void ToggleMusic() {
        _lblMusicOn.gameObject.SetActive(!_lblMusicOn.gameObject.activeSelf);
        _lblMusicOff.gameObject.SetActive(!_lblMusicOn.gameObject.activeSelf);
    }

    public void ToggleSound() {
        _lblSoundOn.gameObject.SetActive(!_lblSoundOn.gameObject.activeSelf);
        _lblSoundOff.gameObject.SetActive(!_lblSoundOn.gameObject.activeSelf);
    }
}