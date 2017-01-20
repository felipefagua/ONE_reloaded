using UnityEngine;
using System.Collections;

public class MoonController : MonoBehaviour {
    public Sprite[] MoonSprites;
    public float ArcLenght;
    public float Direction;
    public bool EnableRotation;
    public bool EnableBlinking;
    public float AngularSpeed;

    protected int m_Index;
    protected Transform Destiny;
    protected Transform m_Arrow;   
    protected bool IsClockWise;
    protected float Speed;
    protected float m_Distance;

    // Use this for initialization
	void Start () {
        IsClockWise = Random.Range(0, 2) == 0 ? true : false;

        Destiny = GameObject.FindWithTag("Player").transform;
        this.transform.localScale = Destiny.localScale;
       
        m_Distance = Vector3.Distance(this.transform.position, Destiny.position);

        m_Arrow = this.transform.FindChild("Arrow");
        m_Arrow.localRotation = Quaternion.AngleAxis(-90 + (180 - ArcLenght)/2, Vector3.forward);

        iTween.MoveTo(this.gameObject, iTween.Hash("position", Destiny.position, "speed", Speed, "easetype", iTween.EaseType.linear));
	}
	
	// Update is called once per frame
	void Update () {

        if (EnableRotation)
        {
            int dir = IsClockWise ? -1 : 1;
            this.transform.Rotate(Vector3.forward, dir * AngularSpeed * Time.deltaTime);
        }

        if (EnableBlinking)
        {
            //if (Vector3.Distance(this.transform.position, Destiny.position) > 2 * m_Distance / 3)
                //iTween.ColorTo(this.gameObject, iTween.Hash("a", 1, "time", 0.5f, "looptype", iTween.LoopType.none));

            //else if (Vector3.Distance(this.transform.position, Destiny.position) > m_Distance / 3)
                //iTween.ColorTo(this.gameObject, iTween.Hash("a", 0, "time", 0.5f, "looptype", iTween.LoopType.none));
            
            iTween.ColorTo(this.gameObject, iTween.Hash("a", 0, "time", 0.5f, "looptype", iTween.LoopType.pingPong));
        }

        Direction = m_Arrow.rotation.eulerAngles.z;
    }

    public void Setup(int moonIndex, float speed, float arcLenght, bool enableRotation, bool enableBlinking, float angularSpeed)
    {
        this.GetComponent<SpriteRenderer>().sprite = MoonSprites[moonIndex];
        Speed = speed;
        this.ArcLenght = arcLenght;
        this.EnableRotation = enableRotation;
        this.EnableBlinking = enableBlinking;
        this.AngularSpeed = angularSpeed;
        this.m_Index = moonIndex;
    }
}
