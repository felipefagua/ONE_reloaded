using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class MoonDetector : MonoBehaviour {

    public GameObject MoonAfinityEffect;
    public GameObject MoonComboEffect;
    public AudioClip[] MatchClips;
    public AudioClip MoonBadClip;
    public System.Action<EAfinityType, float, int> OnMoonDetected;

    protected float m_PreviousAfinity;
    protected PulseController m_Pulse;
    protected Transform m_Light;
    protected int m_ComboCount;

    // Use this for initialization
	void Start () {
	
        m_Pulse = transform.FindChild("Pulse").GetComponent<PulseController>();
        m_Light = transform.FindChild("Light");
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnTriggerEnter2D(Collider2D hit)
    {
        if(hit.tag == "Moon")
        {
            float moonArc = hit.transform.GetComponent<MoonController>().ArcLenght;
            float moonDir = hit.transform.GetComponent<MoonController>().Direction;
           
            float sliderArc = this.GetComponent<SliderController>().ArcLenght;
            float sliderDir = this.GetComponent<SliderController>().Direction;          

            float deltaArc = Mathf.Abs(moonArc - sliderArc) % 360;  
            float deltaDir = Mathf.Abs(moonDir - sliderDir) % 360; 

            deltaArc = deltaArc > 180 ? 360 - deltaArc: deltaArc;
            deltaDir = deltaDir > 180 ? 360 - deltaDir: deltaDir;

            if (moonArc == 0)
                moonDir = deltaDir = 0;

            float average = (deltaArc + deltaDir) / 2;
  
            float afinity = (180 - average) / 180.0f;

            EAfinityType type = EAfinityType.BAD;

            if (afinity >= Constants.AFINITY_LORD)
                type = EAfinityType.LORD;
            else if (afinity >= Constants.AFINITY_EPIC && afinity < Constants.AFINITY_LORD)
                type = EAfinityType.EPIC;
            else if (afinity >= Constants.AFINITY_AWESOME && afinity < Constants.AFINITY_EPIC)
                type = EAfinityType.AWESOME;
            else if (afinity >= Constants.AFINITY_GREAT && afinity < Constants.AFINITY_AWESOME)
                type = EAfinityType.GREAT;
            else if (afinity >= Constants.AFINITY_GOOD && afinity < Constants.AFINITY_GREAT)
                type = EAfinityType.GOOD;

            if (type != EAfinityType.BAD)
            {
                if (afinity >= m_PreviousAfinity && m_PreviousAfinity != 0)
                {
                    GameObject.Instantiate(MoonComboEffect, this.transform.position, Quaternion.identity);
                    m_Light.GetComponent<Animator>().SetTrigger("Show");

                    m_ComboCount++;
                    OnMoonDetected(type, afinity, m_ComboCount+1);

                    PlaySound(true);
                }
                else
                {               
                    GameObject.Instantiate(MoonAfinityEffect, this.transform.position, Quaternion.identity);
                    m_Light.GetComponent<Animator>().SetTrigger("Show");
                              
                    m_ComboCount = 0;
                    OnMoonDetected(type, afinity, 1);

                    PlaySound(false);
                }               
            }
            else
            {
                afinity = 0;
                m_ComboCount = 0;
                m_PreviousAfinity = 0;
                OnMoonDetected(type, 0, 1);

                PlayBad();
            }

            m_PreviousAfinity = afinity;  

            DestroyObject(hit.gameObject);
        }
    }

    void PlaySound(bool isCombo)
    {
        AudioClip clip = MatchClips[Random.Range(0, MatchClips.Length)];

        this.GetComponent<AudioSource>().clip = clip;
        this.GetComponent<AudioSource>().Play();
    }

    void PlayBad()
    {
        this.GetComponent<AudioSource>().clip = MoonBadClip;
        this.GetComponent<AudioSource>().Play();
    }
}
