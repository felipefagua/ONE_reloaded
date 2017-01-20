using UnityEngine;
using System.Collections;

public class Colorize : MonoBehaviour {
    public float Interval;

    protected float m_Interval;
    protected Color m_PreviousColor;
    protected Color m_CurrentColor;

	// Use this for initialization
	void Start () {
        m_PreviousColor = this.GetComponent<Light>().color;
	}
	
	// Update is called once per frame
	void Update () {
        m_Interval += Time.deltaTime;

        if (m_Interval > Interval)
        {
            m_CurrentColor = new Color(Random.Range(0, 255), Random.Range(0, 255), Random.Range(0, 255));
            m_Interval = 0;
        }

        this.GetComponent<Light>().color = Color.Lerp(this.GetComponent<Light>().color, m_CurrentColor, m_Interval / Interval);
	}
}
