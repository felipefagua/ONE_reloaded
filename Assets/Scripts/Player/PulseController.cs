using UnityEngine;
using System.Collections;

public class PulseController : MonoBehaviour {

    public float Speed;
    public float MaxSpeed;
    public Sprite[] PulseSprite;
    public int StateIndex;
    public float LightMaxIntensity;
    public float LightAnimationTime;

    protected GameObject m_Color;
    protected Vector3 m_InitialScale;
    protected int m_PreviousIndex;

    //protected Light m_Light;

	// Use this for initialization
	void Start () {
        m_InitialScale = this.transform.localScale;    

       //m_Light = this.GetComponentInChildren<Light>();
	}
	
	// Update is called once per frame
	void Update () {

        if (m_PreviousIndex != StateIndex && StateIndex >= 0)
        {
            this.GetComponent<SpriteRenderer>().sprite = PulseSprite[StateIndex];
            this.GetComponent<Animator>().speed = Speed;    

            m_PreviousIndex = StateIndex;
        }
    }
}
