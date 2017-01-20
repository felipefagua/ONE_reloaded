using UnityEngine;
using System.Collections;
using System;

public class TutorialGameController : GameController {

    public enum TutorialState {
        leftController,
        rightController,
        bothControllers,
        waitingToFail,
        incrementXP,
        incrementScore,
        tutorialEnding,
        normalGame
    }

    #region Editor fields

    [SerializeField]
    private TutorialState _tutorialState;

    [SerializeField]
    private EAfinityType _minAfinityToCount;

    [SerializeField]
    private int _currentCount = 0;

    [SerializeField]
    private int _leftControllerSuccessesToPass;

    [SerializeField]
    private int _rightControllerSuccessesToPass;

    [SerializeField]
    private int _bothControllersSuccessesToPass;

    [SerializeField]
    private int _failuresNeededToPass;

    [SerializeField]
    private int _incrementXpSuccessesToPass;

    [SerializeField]
    private int _incrementScoreSuccessesToPass;

    [SerializeField]
    private int _tutorialEndingSuccessesToPass;

    #endregion

    private TutorialPlayerController _tutorialPlayer;
    private TutorialMenuController _tutorialMenu;
    private TutorialInputController _tutorialInput;

    #region Awake

    protected override void InitExternalComponents() {
        base.InitExternalComponents();
        _tutorialMenu = FindObjectOfType<TutorialMenuController>();
        _tutorialInput = FindObjectOfType<TutorialInputController>();
        InitTutorialPlayer();
    }

    private void InitTutorialPlayer() {
        _tutorialPlayer = _player as TutorialPlayerController;
        _tutorialPlayer.OnMoonDetectedOnTutorial += OnMoonDetectedOnTutorial;
    }

    private void OnMoonDetectedOnTutorial(EAfinityType afinity) {
        if (IsMoonEnableToCount(afinity)) {
            _currentCount++;
            if (TutorialLevelHasBeenCompleted())
                SetUpNextTutorialLevel();
        }
    }

    private bool IsMoonEnableToCount(EAfinityType afinity) {
        if (_tutorialState == TutorialState.normalGame)
            return false;

        if (_tutorialState == TutorialState.waitingToFail) {
            if(afinity == EAfinityType.BAD)
                return true;
            else
                return false;
        }

        if (afinity <= _minAfinityToCount)
            return true;

        return false;
    }

    private bool TutorialLevelHasBeenCompleted() {
        if (_tutorialState == TutorialState.leftController)
            return _currentCount >= _leftControllerSuccessesToPass;

        if (_tutorialState == TutorialState.rightController)
            return _currentCount >= _rightControllerSuccessesToPass;

        if (_tutorialState == TutorialState.bothControllers)
            return _currentCount >= _bothControllersSuccessesToPass;

        if (_tutorialState == TutorialState.waitingToFail)
            return _currentCount >= _failuresNeededToPass;

        if (_tutorialState == TutorialState.incrementXP)
            return _currentCount >= _incrementXpSuccessesToPass;

        if (_tutorialState == TutorialState.incrementScore)
            return _currentCount >= _incrementScoreSuccessesToPass;

        if (_tutorialState == TutorialState.tutorialEnding)
            return _currentCount >= _tutorialEndingSuccessesToPass;

        return false;
    }

    private void SetUpNextTutorialLevel() {
        _currentCount = 0;

        if (_tutorialState == TutorialState.leftController) {
            _tutorialMenu.ShowTutorial(TutorialMenuController.TutorialStep.rightController_intro);
            _tutorialInput.botInputType = TutorialInputController.InputType.right;
        } else if (_tutorialState == TutorialState.rightController) {
            _tutorialMenu.ShowTutorial(TutorialMenuController.TutorialStep.bothControllers);
            _tutorialInput.botInputType = TutorialInputController.InputType.both;
        } else if (_tutorialState == TutorialState.bothControllers) {
            _tutorialMenu.ShowTutorial(TutorialMenuController.TutorialStep.pause_button);
            _tutorialInput.botInputType = TutorialInputController.InputType.none;
        } else if (_tutorialState == TutorialState.waitingToFail) {
            _tutorialMenu.ShowTutorial(TutorialMenuController.TutorialStep.lives_counter);
            _tutorialInput.botInputType = TutorialInputController.InputType.both;
        } else if (_tutorialState == TutorialState.incrementXP) {
            _tutorialMenu.ShowTutorial(TutorialMenuController.TutorialStep.xp_counter);
            _tutorialInput.botInputType = TutorialInputController.InputType.both;
        } else if (_tutorialState == TutorialState.incrementScore) {
            _tutorialMenu.ShowTutorial(TutorialMenuController.TutorialStep.score_counter);
        } else if (_tutorialState == TutorialState.tutorialEnding) {
            _tutorialMenu.ShowTutorial(TutorialMenuController.TutorialStep.tutorial_ending);
        }
        _tutorialState = (TutorialState)(((int)_tutorialState) + 1);
    }

    #endregion

    #region Start

    protected override void InitGameController() {
        base.InitGameController();

        _tutorialMenu.ShowTutorial(TutorialMenuController.TutorialStep.leftController_intro);
    }

    #endregion   
}
