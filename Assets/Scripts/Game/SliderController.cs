using UnityEngine;
using System.Collections;

public class SliderController : MonoBehaviour {
    public float ArcLenght;
    public float Direction;

    protected SemiCircleController m_LeftSemiCircle;
    protected SemiCircleController m_RightSemiCircle;
    protected Transform m_Arrow;
    protected bool m_Flip;

	// Use this for initialization
	void Start () {
        m_LeftSemiCircle = transform.FindChild("P1LeftSemiCircle").GetComponent<SemiCircleController>();
        m_RightSemiCircle = transform.FindChild("P1RightSemiCircle").GetComponent<SemiCircleController>();
        m_Arrow = transform.FindChild("Arrow");
	}
	
	// Update is called once per frame
	void Update () {
        ArcLenght = Vector3.Angle(m_LeftSemiCircle.Direction, m_RightSemiCircle.Direction); 
        Vector3 dir = (m_LeftSemiCircle.Direction + m_RightSemiCircle.Direction).normalized; 

        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        m_Arrow.rotation = Quaternion.AngleAxis(angle + 90, Vector3.forward);

        Debug.DrawLine(m_Arrow.transform.position, m_Arrow.up, Color.red);
        RaycastHit2D[] hits = Physics2D.RaycastAll(m_Arrow.transform.position, m_Arrow.up, 1.0f);

        bool isLookingInside = false;
        for (int i = 0; i < hits.Length; i++)
        {
            if (hits[i].transform.tag == "Slider")
                isLookingInside = true;
        }

        if(isLookingInside)
            m_Arrow.rotation = Quaternion.AngleAxis(angle + 270, Vector3.forward);

        Direction = m_Arrow.rotation.eulerAngles.z;
	}
}
