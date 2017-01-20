using UnityEngine;
using System.Collections;

public class TouchDebugObject : MonoBehaviour {

    [SerializeField]
    private float _lifeTime;

    [SerializeField]
    private float _touchRadius;

    private Vector3 _positionToCompare;

    public Vector3 PositionToCompare { set { _positionToCompare = value; } }

    private float _distanceToCompare;

    public float DistanceToCompare { set { _distanceToCompare = value; } }

    private Transform _myTransform;

    // Use this for initialization
    void Start () {
        _myTransform = transform;
        Destroy(gameObject, _lifeTime);
	}
	
	// Update is called once per frame
	void Update () {
        DebugTouchCircle();
        DebugPositionToCompare();
	}

    private void DebugTouchCircle() {
        Vector3[] dragRangePoints = GetDragRangePoints();
        DrawCirclePoints(dragRangePoints);
    }

    private void DebugPositionToCompare() {
        Color color = Color.red;
        if (Vector3.Distance(_myTransform.position, _positionToCompare) < _distanceToCompare)
            color = Color.green;

        Debug.DrawLine(_myTransform.position, _positionToCompare, color);
    }

    private Vector3[] GetDragRangePoints() {
        int m = 36;
        Vector3[] dragRangePoints = new Vector3[m];

        for (int i = 0; i < m; i++) {
            float alpha = i / (float)m;
            float angle = 360 * alpha;
            float x = _touchRadius * Mathf.Cos(angle * Mathf.Deg2Rad);
            float y = _touchRadius * Mathf.Sin(angle * Mathf.Deg2Rad);
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
}
