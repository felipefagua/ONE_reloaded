using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System;

public class TutorialInputController : InputController {

    public enum InputType { left, right, both, none }

    #region Editor fields

    [SerializeField]
    private InputType _botInputType;

    [SerializeField]
    private float _rotationSpeed;

    [SerializeField]
    private GameObject _currentSemicircleTarget;

    [SerializeField]
    private float _minSemicircleDistance = 3.5f;

    [SerializeField]
    private float _minRotationDiference = 5f;

    [SerializeField]
    private bool _leftControllerArrive = false;

    [SerializeField]
    private bool _rightControllerArrive = false;

    [SerializeField]
    private float _leftRotationTarget = 0;

    [SerializeField]
    private float _currentLeftRotation = 0;

    [SerializeField]
    private float _rightRotationTarget = 0;

    [SerializeField]
    private float _currentRightRotation = 0;

    [SerializeField]
    private float _rotationOffset = 90;

    [SerializeField]
    private GameObject _leftActiveRobot;

    [SerializeField]
    private GameObject _leftSleepingRobot;

    [SerializeField]
    private GameObject _rightActiveRobot;

    [SerializeField]
    private GameObject _rightSleepingRobot;

    [SerializeField]
    private float _turnOnRobotsDelay = 0.5f;

    #endregion

    private Vector3 _startPositionLeftInput;

    private Vector3 _startPositionRightInput;

    public InputType botInputType {
        set { _botInputType = value;
            ResetInputTransforms();
            SetRobotSprites();
        }
        get { return _botInputType; }
    }

    private void SetRobotSprites() {
        TurnOffRobotSprites();
        Invoke("TurnOnRobotSprites", _turnOnRobotsDelay);
    }

    private void TurnOffRobotSprites() {
        _leftActiveRobot.SetActive(false);
        _leftSleepingRobot.SetActive(false);
        _rightActiveRobot.SetActive(false);
        _rightSleepingRobot.SetActive(false);
    }

    private void TurnOnRobotSprites() {
        switch (_botInputType) {
            case InputType.left:
                _leftActiveRobot.SetActive(true);
                break;
            case InputType.right:
                _rightActiveRobot.SetActive(true);
                break;
            case InputType.both:

                break;
            case InputType.none:
                _leftSleepingRobot.SetActive(true);
                _rightSleepingRobot.SetActive(true);
                break;
        }
    }

    private void UpdateAIInput() {
        if (_currentSemicircleTarget == null) {
            _currentSemicircleTarget = GetNextSemicircle();
            SetRotationTargets();
        }

        _currentLeftRotation = P1LeftSemiCircle.transform.rotation.eulerAngles.z;
        _currentRightRotation = P1RightSemiCircle.transform.rotation.eulerAngles.z;

        if (_currentSemicircleTarget != null) {
            SetRotationTargets();
            if (_botInputType == InputType.left && !ApplicationPause.instance.isPaused)
                RotateLeftController();

            if (_botInputType == InputType.right && !ApplicationPause.instance.isPaused)
                RotateRightController();
        }
    }

    private GameObject GetNextSemicircle() {
        GameObject nextSemicircle = null;

        GameObject[] semicircles = GameObject.FindGameObjectsWithTag("Moon");

        GameObject nearestSemicircle = GetNearestSemicircle(semicircles);

        if (nearestSemicircle != null &&
            nearestSemicircle.transform.position.magnitude <= _minSemicircleDistance)
            nextSemicircle = nearestSemicircle;

        return nextSemicircle;
    }

    private GameObject GetNearestSemicircle(GameObject[] semicircles) {
        GameObject nearestSemicircle = null;

        foreach (GameObject semicircle in semicircles) {
            if (nearestSemicircle == null) {
                nearestSemicircle = semicircle;
                continue;
            }
            if (semicircle.transform.position.magnitude < nearestSemicircle.transform.position.magnitude)
                nearestSemicircle = semicircle;
        }

        return nearestSemicircle;
    }

    private void SetRotationTargets() {
        if (_currentSemicircleTarget == null)
            return;

        float arcOffset = 180 - _currentSemicircleTarget.GetComponent<MoonController>().ArcLenght;

        _leftRotationTarget = _currentSemicircleTarget.transform.rotation.eulerAngles.z + 90;

        if (_leftRotationTarget > 360)
            _leftRotationTarget -= 360;

        _rightRotationTarget = (_leftRotationTarget + arcOffset);

        if (_rightRotationTarget > 360)
            _rightRotationTarget -= 360;

        _leftControllerArrive = false;
        _rightControllerArrive = false;
    }

    private void RotateLeftController() {
        float rotationDiference = _leftRotationTarget - P1LeftSemiCircle.transform.rotation.eulerAngles.z;

        if (Mathf.Abs(rotationDiference) < _minRotationDiference) 
            _leftControllerArrive = true;
        else 
            _leftControllerArrive = false;

        if (_leftControllerArrive)
            return;

        float direction = (rotationDiference / Mathf.Abs(rotationDiference));

        float delta = _rotationSpeed * direction;

        if (Mathf.Abs(delta) < MaxDragAngle) {
            Vector3 eulerAngles = m_P1LeftInput.rotation.eulerAngles;
            eulerAngles.z += delta;
            Quaternion rotation = new Quaternion();
            rotation.eulerAngles = eulerAngles;
            m_P1LeftInput.rotation = rotation;
            P1LeftSemiCircle.InputRotate(delta);

            P1LeftSemiCircle.GetComponent<SpriteRenderer>().sortingOrder = 1;
            P1RightSemiCircle.GetComponent<SpriteRenderer>().sortingOrder = 0;
        }        
    }

    private void RotateRightController() {
        float rotationDiference = _rightRotationTarget - P1RightSemiCircle.transform.rotation.eulerAngles.z;

        if (Mathf.Abs(rotationDiference) < _minRotationDiference)
            _rightControllerArrive = true;

        if (_rightControllerArrive)
            return;

        float direction = (rotationDiference / Mathf.Abs(rotationDiference));

        float delta = _rotationSpeed * direction;

        if (Mathf.Abs(delta) < MaxDragAngle) {
            Vector3 eulerAngles = m_P1RightInput.rotation.eulerAngles;
            eulerAngles.z += delta;
            Quaternion rotation = new Quaternion();
            rotation.eulerAngles = eulerAngles;
            m_P1RightInput.rotation = rotation;
            P1RightSemiCircle.InputRotate(delta);

            P1LeftSemiCircle.GetComponent<SpriteRenderer>().sortingOrder = 0;
            P1RightSemiCircle.GetComponent<SpriteRenderer>().sortingOrder = 1;
        }
    }

    #region Overloaded methods

    protected override void InitComponents() {
        base.InitComponents();
        GetInputStartConfig();
        botInputType = InputType.left;
    }

    private void GetInputStartConfig() {
        _startPositionLeftInput = m_P1LeftInput.position;
        _startPositionRightInput = m_P1RightInput.position;
    }

    private void ResetInputTransforms() {
        m_P1LeftInput.transform.position = _startPositionLeftInput;
        m_P1LeftInput.rotation = Quaternion.identity;
        m_OldP1LeftAngle = 90;
        P1LeftSemiCircle.ResetRotation();

        m_P1RightInput.transform.position = _startPositionRightInput;
        m_P1RightInput.rotation = Quaternion.identity;
        m_OldP1RightAngle = 90;
        P1RightSemiCircle.ResetRotation();
    }

    protected override void Joystikcs() {
        if (_botInputType != InputType.none)
            base.Joystikcs();

        UpdateAIInput();
    }

    protected override void OnDrag(DragGesture gesture) {
        if (_botInputType != InputType.none)
            base.OnDrag(gesture);
    }

    protected override void OnFingerDown(FingerDownEvent evt) {
        if (_botInputType != InputType.none)
            base.OnFingerDown(evt);
    }

    public override void BeginSlider(FingerDownEvent evt) {
        if (_botInputType != InputType.none)
            base.BeginSlider(evt);
    }

    public override void DragSlider(DragGesture gesture) {
        if (_botInputType != InputType.none)
            base.DragSlider(gesture);
    }

    public override void BeginKnob(FingerDownEvent evt) {
        if (_botInputType != InputType.none)
            base.BeginKnob(evt);
    }

    protected override bool CanBeginLeftKnob(FingerDownEvent evt) {
        return (base.CanBeginLeftKnob(evt) &&
                (_botInputType == InputType.right || _botInputType == InputType.both));
    }

    protected override bool CanBeginRightKnob(FingerDownEvent evt) {
        return (base.CanBeginRightKnob(evt) &&
                (_botInputType == InputType.left || _botInputType == InputType.both));
    }

    public override void DragKnob(DragGesture gesture) {
        if (_botInputType != InputType.none)
            base.DragKnob(gesture);
    }

    protected override bool CanDragLeftKnob(DragGesture gesture) {
        return (base.CanDragLeftKnob(gesture) &&
                (_botInputType == InputType.right || _botInputType == InputType.both ));
    }

    protected override bool CanDragRightKnob(DragGesture gesture) {
        return (base.CanDragRightKnob(gesture) &&
                (_botInputType == InputType.left || _botInputType == InputType.both));
    }

    #endregion
}
