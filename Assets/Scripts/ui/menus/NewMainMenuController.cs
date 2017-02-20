using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using System;

public class NewMainMenuController : IMainMenuController {

    [SerializeField]
    private Transform _singlePlayerPanel;

    [SerializeField]
    private Transform _singlePlayerPanelBackgorund;

    [SerializeField]
    private Transform _multiplayerPanel;

    [SerializeField]
    private Transform _multiplayerPanelBackgorund;

    [SerializeField]
    private Transform _statsPanel;

    [SerializeField]
    private Transform _statsPanelBackgorund;

    [SerializeField]
    private Transform _centralBackground;

    private Transform _lastButtonBackground;

    [SerializeField]
    private float _scaleTime;

    public override void Show() {
        _centralBackground.localScale = new Vector3(0, 0, 0);
        UIFader fader = GetComponent<UIFader>();
        fader.StartFadeIn();
    }

    public void LoadScene(string sceneName) {
        Persistence.SelectedGameMode = sceneName;
        SceneManager.LoadScene("Loading", LoadSceneMode.Single);
    }

    public void CleanLocalData() {
        LocalDataController.instance.CleanLocalData();
    }

    public void ShowSinglePlayerPanel() {
        ShowPanel(_singlePlayerPanel, _singlePlayerPanelBackgorund);
    }

    public void ShowMultiplayerPanel() {
        ShowPanel(_multiplayerPanel, _multiplayerPanelBackgorund);
    }

    public void ShowStatsPanel() {
        ShowPanel(_statsPanel, _statsPanelBackgorund);
    }

    private void ShowPanel(Transform centralPanel, Transform buttonBackground) {
        ShowLastButtonBackground();
        _lastButtonBackground = buttonBackground;
        HideAllPanels();
        centralPanel.gameObject.SetActive(true);
        iTween.ScaleTo(centralPanel.gameObject, new Vector3(1, 1, 1), _scaleTime);
        iTween.ScaleTo(buttonBackground.gameObject, new Vector3(0, 0, 0), _scaleTime);
        _centralBackground.localScale = new Vector3(0, 0, 0);
        iTween.ScaleTo(_centralBackground.gameObject, new Vector3(1, 1, 1), _scaleTime);
    }

    private void ShowLastButtonBackground() {
        if (_lastButtonBackground != null) {
            _lastButtonBackground.localScale = new Vector3(0, 0, 0);
            iTween.ScaleTo(_lastButtonBackground.gameObject, new Vector3(1, 1, 1), _scaleTime);
        }
    }

    private void HideAllPanels() {
        HidePanel(_singlePlayerPanel);
        HidePanel(_multiplayerPanel);
        HidePanel(_statsPanel);        
    }

    private void HidePanel(Transform panel) {
        panel.gameObject.SetActive(false);
        panel.localScale = new Vector3(0, 0, 0);
    }

    public override void OnTutorial() {
        Persistence.SelectedGameMode = "test_scene_tutorial";
        SceneManager.LoadScene("Loading", LoadSceneMode.Single);
    }
}
