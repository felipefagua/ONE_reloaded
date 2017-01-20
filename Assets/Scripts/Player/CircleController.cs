using UnityEngine;
using System.Collections;

public class CircleController : MonoBehaviour {

    [SerializeField]
    private bool _isOnDebugMode;

    [SerializeField]
    private GameObject _touchDebuggerTemplate;

    private Transform _myTransform;

    [SerializeField]
    private float _dragRange;    

    // Use this for initialization
    void Start () {
        _myTransform = transform;
	}
	
	// Update is called once per frame
	void Update () {
        if (_isOnDebugMode)
            DebugController();
	}

    public bool TouchIsInDragRange(Vector3 touchPosition) {
        if (_isOnDebugMode)
            InstatiateTouchDebugger(touchPosition);

        if (Vector3.Distance(touchPosition, _myTransform.position) < _dragRange)
            return true;
        return false;
    }

    // Debug methods


    private Vector3[] GetDragRangePoints() {
        int m = 36;
        Vector3[] dragRangePoints = new Vector3[m];

        for (int i = 0; i < m; i++) {
            float alpha = i / (float)m;
            float angle = 360 * alpha;
            float x = _dragRange * Mathf.Cos(angle * Mathf.Deg2Rad);
            float y = _dragRange * Mathf.Sin(angle * Mathf.Deg2Rad);
            dragRangePoints[i] = (new Vector3(x, y) + _myTransform.position);
        }

        return dragRangePoints;
    }

    private void DrawCirclePoints(Vector3[] circlePoints) {
        for (int i = 0; i < circlePoints.Length; i++) {
            int nextIndex = GetNextCircleIndex(i, circlePoints.Length);
            Debug.DrawLine(circlePoints[i], circlePoints[nextIndex]);
        }
    }

    private int GetNextCircleIndex(int index, int maxIndex) {
        int nextIndex = index + 1;

        if (nextIndex >= maxIndex)
            nextIndex = 0;

        return nextIndex;
    }

    private void InstatiateTouchDebugger(Vector3 touchPosition)
    {
        GameObject touchDebuggerGO = Instantiate<GameObject>(_touchDebuggerTemplate);
        touchDebuggerGO.transform.position = touchPosition;
        TouchDebugObject touchDebugger = touchDebuggerGO.GetComponent<TouchDebugObject>();
        touchDebugger.PositionToCompare = _myTransform.position;
        touchDebugger.DistanceToCompare = _dragRange;
    }

    private void DebugController()
    {
        DebugDragRange();
    }

    private void DebugDragRange()
    {
        Vector3[] dragRangePoints = GetDragRangePoints();
        DrawCirclePoints(dragRangePoints);
    }
}
