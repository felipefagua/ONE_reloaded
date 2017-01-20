using UnityEngine;

public class SpriteController : MonoBehaviour {

    [SerializeField]
    private bool _debug;

    [SerializeField]
    private GameObject _touchDebuggerTemplate;

    [SerializeField]
    private Transform _semicircle;

    [SerializeField]
    private Camera _camera;

    [SerializeField]
    private Transform _button;

    [SerializeField]
    private float _buttonDistance;

    [SerializeField]
    private bool _isDragging;

    [SerializeField]
    private float _dragDistance;

    private Vector3 _inputPostion;

    private Transform _myTransform;

    private float _lastAngle;

    // Use this for initialization
    void Start () {
        _myTransform = transform;        
	}

    // Update is called once per frame
    private void Update() {
        if (Input.GetMouseButton(0)) {
            _inputPostion = _camera.ScreenToWorldPoint(Input.mousePosition);
            _inputPostion.z = _myTransform.position.z;

            if (TouchIsInDragRange(_inputPostion)) 
                StartDrag(_camera.ScreenToWorldPoint(_inputPostion));            
        }
        if (Input.GetMouseButtonUp(0))
            StopDrag();

        if (_isDragging)
            UpdateDrag();
    }

    private void StartDrag(Vector3 inputPosition) {
        _isDragging = true;
    }

    private void StopDrag() {
        _isDragging = false;
    }

    private void UpdateDrag () {
        _inputPostion = _camera.ScreenToWorldPoint(Input.mousePosition);
        _inputPostion.z = _myTransform.position.z;

        if (!TouchIsInDragRange(_inputPostion)) {
            Vector3 controllerToTouch = _inputPostion - _myTransform.position;
            float stepSize = controllerToTouch.magnitude - _buttonDistance;
            if (stepSize < 0)
                stepSize = 0;
            Vector3 step = controllerToTouch.normalized * stepSize;
            _myTransform.position += (step);
        }

        Vector3 direction = _inputPostion - transform.position;
        RotateSprite(direction);
        RotateSemicircle();

        if (_debug)
            DebugController();
    }

    private void RotateSemicircle() {
        float currentAngle = _myTransform.rotation.eulerAngles.z;
        float deltaRotation = currentAngle - _lastAngle;
        _lastAngle = currentAngle;

        _semicircle.Rotate(new Vector3(0, 0, deltaRotation));
    }

    private void RotateSprite(Vector3 direction) {
        float angles = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        Vector3 eulerAngles = new Vector3(0, 0, angles);
        Quaternion newRotation = new Quaternion();
        newRotation.eulerAngles = eulerAngles;
        _myTransform.rotation = newRotation;
    }

    public bool TouchIsInDragRange(Vector3 touchPosition) {
        if (_debug)
            InstatiateTouchDebugger(touchPosition);

        if (Vector3.Distance(touchPosition, _myTransform.position) < _dragDistance)
            return true;

        return false;
    }

    // Debug methods
    private void InstatiateTouchDebugger(Vector3 touchPosition) {
        GameObject touchDebuggerGO = Instantiate<GameObject>(_touchDebuggerTemplate);
        touchDebuggerGO.transform.position = touchPosition;
        TouchDebugObject touchDebugger = touchDebuggerGO.GetComponent<TouchDebugObject>();
        touchDebugger.PositionToCompare = _myTransform.position;
        touchDebugger.DistanceToCompare = _dragDistance;
    }

    private void DebugController() {
        DebugDragRange();
        DebugButtonRatio();
    }

    private void DebugButtonRatio() {
        Vector3[] dragRangePoints = GetDragButtonDistancePoints();
        DrawCirclePoints(dragRangePoints);
    }

    private void DebugDragRange() {
        Vector3[] dragRangePoints = GetDragRangePoints();
        DrawCirclePoints(dragRangePoints);
    }

    private Vector3[] GetDragButtonDistancePoints() {
        int m = 36;
        Vector3[] dragRangePoints = new Vector3[m];

        for (int i = 0; i < m; i++)
        {
            float alpha = i / (float)m;
            float angle = 360 * alpha;
            float x = _buttonDistance * Mathf.Cos(angle * Mathf.Deg2Rad);
            float y = _buttonDistance * Mathf.Sin(angle * Mathf.Deg2Rad);
            dragRangePoints[i] = (new Vector3(x, y) + _myTransform.position);
        }

        return dragRangePoints;
    }

    private Vector3[] GetDragRangePoints()
    {
        int m = 36;
        Vector3[] dragRangePoints = new Vector3[m];

        for (int i = 0; i < m; i++)
        {
            float alpha = i / (float)m;
            float angle = 360 * alpha;
            float x = _dragDistance * Mathf.Cos(angle * Mathf.Deg2Rad);
            float y = _dragDistance * Mathf.Sin(angle * Mathf.Deg2Rad);
            dragRangePoints[i] = (new Vector3(x, y) + _myTransform.position);
        }

        return dragRangePoints;
    }

    private void DrawCirclePoints(Vector3[] circlePoints)
    {
        for (int i = 0; i < circlePoints.Length; i++)
        {
            int nextIndex = GetNextCircleIndex(i, circlePoints.Length);
            Debug.DrawLine(circlePoints[i], circlePoints[nextIndex]);
        }
    }

    private int GetNextCircleIndex(int index, int maxIndex)
    {
        int nextIndex = index + 1;

        if (nextIndex >= maxIndex)
            nextIndex = 0;

        return nextIndex;
    }
}
