using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class TutorialMenuController : MonoBehaviour {

    public enum TutorialStep {
        leftController_intro,
        leftController_finger,
        rightController_intro,
        rightController_finger,
        bothControllers,
        pause_button,
        lives_intro,
        lives_counter,
        xp_counter,
        score_counter,
        tutorial_ending_intro,
        tutorial_ending,
        none
    }

    [SerializeField]
    private GameObject _leftTrial;

    [SerializeField]
    private GameObject _rightTrial;

    [SerializeField]
    private TutorialStep _currentTutorialStep = TutorialStep.none;

    [SerializeField]
    private GameObject[] _tutorialGameObjects;

    private MenuController _menuController;

    public TutorialStep CurrentTutorialStep { get { return _currentTutorialStep; } }

    private void Awake() {
        _menuController = GetComponent<MenuController>();
    }

    public void ShowNextTutorialStep() {
        TutorialStep nextTutorialStep = (TutorialStep)((int)_currentTutorialStep + 1);
        ShowTutorial(nextTutorialStep);
    }

    public void ShowTutorial(TutorialStep tutorialStep) {
        TutorialPause();
        _currentTutorialStep = tutorialStep;
        ShowCurrentTutorialGameObject();
    }

    public void GotoMainMenu() {
        SceneManager.UnloadScene(SceneManager.GetActiveScene().name);
        SceneManager.LoadScene(_menuController.mainMenuSceneName, LoadSceneMode.Single);
    }
    

    private void TutorialPause() {
        _menuController.OnPause();
        _menuController.HidePauseMenu();
    }

    public void TutorialResume() {
        _menuController.OnResume();
        HideAllTutorialGameObjects();
        ShowTrials();
    }

    private void ShowTrials() {
        _leftTrial.SetActive(true);
        _rightTrial.SetActive(true);
    }

    private void ShowCurrentTutorialGameObject() {
        HideAllTutorialGameObjects();
        int index = (int)_currentTutorialStep;
        _tutorialGameObjects[index].SetActive(true);
    }

    private void HideAllTutorialGameObjects() {
        foreach (GameObject tutorialGameObject in _tutorialGameObjects)
            tutorialGameObject.SetActive(false);
    }
}
