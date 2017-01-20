using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;

public class InputController : MonoBehaviour
{

    protected SemiCircleController P1LeftSemiCircle;
    protected SemiCircleController P1RightSemiCircle;
    protected Transform m_P1LeftInput;
    protected Transform m_P1RightInput;

    public DragRecognizer DragGestureRecongnizer;
    public float MaxDragAngle = 90.0f;
    
    protected Transform m_LeftSlider;
    protected Transform m_RightSlider;

    protected float m_OldP1LeftAngle = 0;
    protected float m_OldP1RightAngle = 0;
    protected float m_Radio = 0;

    [SerializeField]
    private CircleController _leftCircleController;

    [SerializeField]
    private CircleController _rightCircleController;

    // Use this for initialization
    void Start() {
        InitComponents();
    }

    protected virtual void InitComponents() {
        P1LeftSemiCircle = GameObject.FindWithTag("Player").GetComponentsInChildren<SemiCircleController>()[0];
        P1RightSemiCircle = GameObject.FindWithTag("Player").GetComponentsInChildren<SemiCircleController>()[1];

        m_P1LeftInput = this.transform.FindChild("P1LeftInput");
        m_P1RightInput = this.transform.FindChild("P1RightInput");

        m_LeftSlider = this.transform.FindChild("LeftSlider");
        m_RightSlider = this.transform.FindChild("RightSlider");

        bool useCircles = InitialParameters.ControlMode == "Circles";

        m_P1LeftInput.gameObject.SetActive(useCircles);
        m_P1RightInput.gameObject.SetActive(useCircles);

        m_LeftSlider.GetComponent<Image>().enabled = false;
        m_RightSlider.GetComponent<Image>().enabled = false;

        m_LeftSlider.gameObject.SetActive(!useCircles);
        m_RightSlider.gameObject.SetActive(!useCircles);

        m_Radio = Vector3.Distance(m_P1LeftInput.transform.position, m_P1LeftInput.transform.FindChild("Trail").transform.position);
    }

    // Update is called once per frame
    void Update() {	
        Joystikcs();
    }

    protected virtual void Joystikcs() {
        if (Input.GetAxis("Horizontal") != 0 || Input.GetAxis("Vertical") != 0) {
            float angle = Mathf.Atan2(Input.GetAxis("Vertical"), Input.GetAxis("Horizontal")) * Mathf.Rad2Deg;
            float delta = angle - m_OldP1LeftAngle; 

            if (Mathf.Abs(delta) < MaxDragAngle) {
                m_P1LeftInput.rotation = m_P1LeftInput.rotation * Quaternion.AngleAxis(-delta, Vector3.forward);
                P1LeftSemiCircle.InputRotate(-delta);
            }
            m_OldP1LeftAngle = angle;
            Debug.LogWarning("Here");
        }

        if (Input.GetAxis("Horizontal2") != 0 || Input.GetAxis("Vertical2") != 0) {
            float angle = Mathf.Atan2(Input.GetAxis("Vertical2"), Input.GetAxis("Horizontal2")) * Mathf.Rad2Deg;
            float delta = angle - m_OldP1RightAngle; 

            if (Mathf.Abs(delta) < MaxDragAngle) {
                m_P1RightInput.rotation = m_P1RightInput.rotation * Quaternion.AngleAxis(-delta, Vector3.forward);
                P1RightSemiCircle.InputRotate(-delta);
            }

            m_OldP1RightAngle = angle;
        }
    }

    protected virtual void OnDrag(DragGesture gesture)
    {           
        if (Time.timeScale == 0)
            return;   

        bool useCircles = InitialParameters.ControlMode == "Circles";

        if(useCircles)
            DragKnob(gesture);
        else
            DragSlider(gesture);  
    }

    protected virtual void OnFingerDown(FingerDownEvent evt)
    {
        if (Time.timeScale == 0)
            return;

        Debug.Log(GameObject.Find("BtnPause").GetComponent<RectTransform>().rect);
        Debug.Log(evt.Position);
        Rect rectButton = GameObject.Find("BtnPause").GetComponent<RectTransform>().rect;
        Rect zone = new Rect(Screen.width - rectButton.width, Screen.height - rectButton.height, rectButton.width, rectButton.height);

        //Debug.Log(zone);
        //Debug.Log(evt.Position);

        //if (zone.Contains(evt.Position))
        //return;


        bool useCircles = InitialParameters.ControlMode == "Circles";

        if (useCircles)
            BeginKnob(evt);
        else
            BeginSlider(evt);

    }

    public virtual void BeginSlider(FingerDownEvent evt)
    {      

        if (evt.Position.x < Screen.width / 2)
        {
            m_LeftSlider.GetComponent<Image>().enabled = true;
            m_LeftSlider.transform.position = evt.Position;
            m_LeftSlider.transform.FindChild("Trail").localPosition = Vector3.zero;
        }
        else
        {
            m_RightSlider.GetComponent<Image>().enabled = true;
            m_RightSlider.transform.position = evt.Position;
            m_RightSlider.transform.FindChild("Trail").localPosition = Vector3.zero;            
        }
    }

    public virtual void DragSlider(DragGesture gesture)
    {
        switch (gesture.Phase)
        {
            case  ContinuousGesturePhase.Updated:
                if (gesture.Position.x < Screen.width / 2)
                {
                    m_LeftSlider.transform.position = new Vector3(m_LeftSlider.transform.position.x, gesture.Position.y, 0);
                    //m_LeftSlider.transform.FindChild("Trail").localPosition = Mathf.Sign(gesture.DeltaMove.y) * Vector3.up * 80;
                    P1LeftSemiCircle.InputRotate(-gesture.DeltaMove.y * 720 / Screen.height);
                }
                else
                {
                    m_RightSlider.transform.position = new Vector3(m_RightSlider.transform.position.x, gesture.Position.y, 0);
                    //m_RightSlider.transform.FindChild("Trail").localPosition = Mathf.Sign(gesture.DeltaMove.y) * Vector3.up * 80;
                    P1RightSemiCircle.InputRotate(gesture.DeltaMove.y * 720 / Screen.height);
                }
                break;
            
            case ContinuousGesturePhase.Ended:

                if (gesture.Position.x < Screen.width / 2)
                    m_LeftSlider.GetComponent<Image>().enabled = false;
                else
                    m_RightSlider.GetComponent<Image>().enabled = false;            
                break;
        }
    }

    public virtual void BeginKnob(FingerDownEvent evt) {
        Vector3 inputPosition = evt.Position;
        if (CanBeginLeftKnob(evt)) {
            m_P1LeftInput.gameObject.SetActive(true);   
            m_P1LeftInput.transform.position = evt.Position - Vector2.up * m_Radio;
            m_P1LeftInput.rotation = Quaternion.identity;
            m_OldP1LeftAngle = 90;
            _leftCircleController.TouchIsInDragRange(inputPosition);            
        } else if (CanBeginRightKnob(evt)) {
            m_P1RightInput.gameObject.SetActive(true);   
            m_P1RightInput.transform.position = evt.Position - Vector2.up * m_Radio;
            m_P1RightInput.rotation = Quaternion.identity;
            m_OldP1RightAngle = 90;
            _rightCircleController.TouchIsInDragRange(inputPosition);
        }        
    }

    protected virtual bool CanBeginLeftKnob(FingerDownEvent evt) {
        return (evt.Position.x < (Screen.width / 2));
    }

    protected virtual bool CanBeginRightKnob(FingerDownEvent evt) {
        return (evt.Position.x >= (Screen.width / 2));
    }

    public virtual void DragKnob(DragGesture gesture) {
        Vector3 inputPosition = gesture.Position;
        if (CanDragLeftKnob(gesture)) {
            if (!_leftCircleController.TouchIsInDragRange(inputPosition)) {
                Vector3 controllerToTouch = inputPosition - _leftCircleController.transform.position;
                float stepSize = controllerToTouch.magnitude - m_Radio;
                if (stepSize < 0)
                    stepSize = 0;
                Vector3 step = controllerToTouch.normalized * stepSize;
                _leftCircleController.transform.position += (step);
                                
            }

            Vector3 dir = new Vector3(gesture.Position.x, gesture.Position.y) - _leftCircleController.transform.position;
            float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
            float delta = angle - m_OldP1LeftAngle;

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

            m_OldP1LeftAngle = angle;
        } else if (CanDragRightKnob(gesture)) {
            if (!_rightCircleController.TouchIsInDragRange(inputPosition)) {
                Vector3 controllerToTouch = inputPosition - _rightCircleController.transform.position;
                float stepSize = controllerToTouch.magnitude - m_Radio;
                if (stepSize < 0)
                    stepSize = 0;
                Vector3 step = controllerToTouch.normalized * stepSize;
                _rightCircleController.transform.position += (step);
            }

            Vector3 dir = new Vector3(gesture.Position.x, gesture.Position.y) - m_P1RightInput.position;
            float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
            float delta = angle - m_OldP1RightAngle; 

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

            m_OldP1RightAngle = angle;
        }
    }

    protected virtual bool CanDragLeftKnob(DragGesture gesture) {
        return (gesture.Position.x < (Screen.width / 2));
    }

    protected virtual bool CanDragRightKnob(DragGesture gesture) {
        return (gesture.Position.x >= (Screen.width / 2));
    }

}
