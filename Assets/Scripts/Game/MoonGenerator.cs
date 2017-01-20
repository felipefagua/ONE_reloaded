using UnityEngine;
using System.Collections;

public class MoonGenerator : MonoBehaviour
{
    public float[] MoonArcs;
    public GameObject MoonPrefab;
    public float CurrentFrecuency;
    public float CurrentSpeed;
    public bool EnableRotation;
    public bool EnableBlinking;
    public float AngularSpeed;
    public bool BonusMode;
    public bool IsDummy;

    protected float m_CurrentTime;
    protected int m_BonusCount = 1;
    protected float m_BonusSpeed = 0;

    // Use this for initialization
    void Start()
    {
        for (int i = 0; i < transform.childCount; i++)
            transform.GetChild(i).GetComponent<SpriteRenderer>().enabled = false;

    }
	
    // Update is called once per frame
    void Update()
    {      
        if (BonusMode)
            SetBonusMode();
        else
            SetNormalMode();
    }

    public void SetupNormalMode()
    {
        this.SetupNormalMode(CurrentFrecuency, CurrentSpeed);    
    }

    public void SetupNormalMode(float frecuency, float speed)
    {
        m_CurrentTime = 0;
        this.CurrentFrecuency = frecuency; 
        this.CurrentSpeed = speed; 
        this.BonusMode = false;
    }

    public void SetupBonusMode()
    {
        m_CurrentTime = 0;
        m_BonusCount = 0;
        m_BonusSpeed = CurrentSpeed;
        this.BonusMode = true;
    }

    void SetNormalMode()
    {
        m_CurrentTime += Time.deltaTime;

        if (m_CurrentTime > CurrentFrecuency)
        {
            int moonIndex = Random.Range(0, MoonArcs.Length);

            int startPos = Random.Range(0, transform.childCount);
            Quaternion startRot = Quaternion.AngleAxis(Random.Range(0, 360), Vector3.forward);
           
            CreateMoon(moonIndex, CurrentSpeed, startPos, startRot, AngularSpeed, EnableRotation, EnableBlinking, 0);

            if (CurrentFrecuency > Constants.MAX_LEVELFRECUENCY)
                CurrentFrecuency -= InitialParameters.ConstantFrecuency;
            else
                CurrentFrecuency = Constants.MAX_LEVELFRECUENCY;

            m_CurrentTime = 0;
        }
    }


    void SetBonusMode()
    {
        m_CurrentTime += Time.deltaTime;
    
        if (!ExistMoon())
        {
            int moonIndex = Random.Range(0, MoonArcs.Length-1);
            int startPos = Random.Range(0, transform.childCount);

            Quaternion startRot = new Quaternion();
            Vector3 eulerAngles = new Vector3(0,0, Random.Range(0, 360));
            startRot.eulerAngles = eulerAngles;

            m_BonusSpeed += InitialParameters.BonusAcceleration;

            int mode = Random.Range(0, 3);
            float angle = InitialParameters.BonusMoonAngle;
            int sign = 1;
            for (int i = 0; i < m_BonusCount; i++)
            {
                if (mode < 1)
                    startRot = Quaternion.AngleAxis(angle, Vector3.forward) * startRot;
    
                if (mode < 2)
                {
                    if (moonIndex > MoonArcs.Length - 2 || moonIndex < 0)
                        sign = sign * -1;
    
                    moonIndex += sign;
                }
                
                CreateMoon(moonIndex, m_BonusSpeed, startPos, startRot, AngularSpeed, false, false, InitialParameters.BonusMoonDistance * i);       
            }

            m_BonusCount++;
            m_CurrentTime = 0;
        }       
    }

    public void DecreaseDifficulty()
    {
        //CurrentFrecuency += 1;
    }

    private void CreateMoon(int moonIndex, float speed, int startPos, Quaternion startRot, float angularSpeed, bool rotate, bool blinking, float delta)
    {
        //TODO: remove after test
        //startRot.eulerAngles = new Vector3();
        Vector3 dir = Vector3.zero;
        float _speed = 0;
        switch (startPos)
        {
            case 0: 
                dir = Vector3.up;
                _speed = 4 * speed / 6;
            break;
            case 1:
                dir = Vector3.down;
                _speed = 4 * speed / 6;
            break;
            case 2:
                dir = Vector3.right;
                _speed = speed;                    
                break;
            case 3:
                dir = Vector3.left;
                _speed = speed;
            break;
        }

        if (IsDummy)
            moonIndex = 12;
        
        Vector3 pos = this.transform.GetChild(startPos).position;
        GameObject moon = GameObject.Instantiate(MoonPrefab, pos + dir * delta, startRot) as GameObject;
        moon.GetComponent<MoonController>().Setup(moonIndex, _speed, MoonArcs[moonIndex], rotate, blinking, angularSpeed);
    }

    public void DestroyMoons()
    {
        GameObject[] moons = GameObject.FindGameObjectsWithTag("Moon");
        for (int i = 0; i < moons.Length; i++)
            DestroyObject(moons[i]);
    }

    public bool ExistMoon()
    {
        return GameObject.FindGameObjectsWithTag("Moon").Length != 0;
    }
}
