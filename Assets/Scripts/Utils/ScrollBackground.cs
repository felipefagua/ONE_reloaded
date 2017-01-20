using UnityEngine;
using System.Collections;

public class ScrollBackground : MonoBehaviour {
    public float Speed;
    public float Distance;

    protected float m_Distance;
    protected float m_Dir;
    protected float m_StartPos;

    void Start()
    {
        m_StartPos = this.transform.position.x;
        m_Dir = 1;
    }

    void Update()
    {
        if (m_Distance < 0)
            m_Dir = 1;

        if (m_Distance > Distance)
            m_Dir = -1;
        
        m_Distance += m_Dir * Speed * Time.deltaTime;

        this.transform.position = new Vector3(m_StartPos + m_Distance, this.transform.position.y, 0);
    }
}
