using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class PlayerController : MonoBehaviour {

    public int Lifes;
    public int CurrentLife;
    public int TotalScore;
    public float TotalExperience;
    public float CurrentExperience;
    public float BonusExperience;

    public bool InBonusMode;
    public float GameTime;
    public GameObject DeathEffect;

    public System.Action<EAfinityType, int, int> OnMoon;
    public System.Action<string> OnLevelUp;
    public System.Action OnUpdateLives;
    public GameObject LevelUpEffect;

    protected float m_PulseStep;
    protected PulseController m_Pulse;
    protected DamageController m_Damage;
    protected Light m_Light;

    protected float m_GameTime;

    private void Awake() {
        InitExternalComponents();
    }

    protected virtual void InitExternalComponents() {
        this.GetComponent<MoonDetector>().OnMoonDetected += OnMoonDetected;

        CurrentLife = Lifes;

        m_Pulse = this.GetComponentInChildren<PulseController>();
        m_Damage = this.GetComponentInChildren<DamageController>();

        m_Light = this.GetComponentInChildren<Light>();

        m_PulseStep = (float)(m_Pulse.MaxSpeed - 1) / Lifes;
    }
	
	private void Update () {
        GameTime += Time.deltaTime;
        m_Light.enabled = !InBonusMode;
	}

    protected virtual void OnMoonDetected(EAfinityType type, float afinity, int count) {
        SetPulseAndLife(afinity);
        CalculatePerformance(type, afinity, count);
        OnMoon(type, count, TotalScore);
    }   

    public virtual void SetPulseAndLife(float afinity) {
        if (afinity < 0.6f && !InBonusMode)
        {
            if (CurrentLife >= 0) {
                CurrentLife--;
                OnUpdateLives();
            }
            m_Pulse.Speed += m_PulseStep;        

            if (CurrentLife < Lifes)
            {       
                m_Damage.DamageIndex = CurrentLife-1;
                m_Pulse.StateIndex = CurrentLife-1;
                m_Damage.Speed += m_PulseStep;
            }
         }
     }

    public void Death()
    {
        //this.StartCoroutine(DeathCoroutine());

        transform.FindChild("Damage").GetComponent<Animator>().enabled = false;
        transform.FindChild("Damage").GetComponent<SpriteRenderer>().color = Color.white;

        transform.FindChild("Pulse").GetComponent<Animator>().SetTrigger("Death");

        GameObject.Instantiate(DeathEffect, this.transform.position, Quaternion.identity);
    }

    public void Alive()
    {
        transform.FindChild("Damage").GetComponent<Animator>().enabled = true;
        transform.FindChild("Pulse").GetComponent<Animator>().SetTrigger("Alive");
    }

    public virtual void CalculatePerformance(EAfinityType type, float afinity, int count) {
        IncrementScore(type, count);
        IncrementXP(type, afinity, count);        
    }

    protected virtual void IncrementScore(EAfinityType afinity, int count) {
        int score = GetScore(afinity);
        TotalScore += score * count;
    }

    protected virtual void IncrementXP(EAfinityType type, float afinity, int count) {
        if (!InBonusMode) {
            TotalExperience += afinity * count;
            CurrentExperience += afinity * count;
        }

        if (type == EAfinityType.BAD)
            CurrentExperience = 0;
    }

    protected int GetScore(EAfinityType afinity) {
        int score = 0;
        switch (afinity) {
            case EAfinityType.LORD:
                score = 100;
                break;
            case EAfinityType.EPIC:
                score = 80;
                break;
            case EAfinityType.AWESOME:
                score = 60;
                break;
            case EAfinityType.GREAT:
                score = 30;
                break;
            case EAfinityType.GOOD:
                score = 20;
                break;
        }
        return score;
    }

    public void SetLevelUp()
    {   
        this.StartCoroutine(LevelUpCoroutine());
    }

    IEnumerator LevelUpCoroutine()
    {        
        yield return new WaitForSeconds(0.5f);
        GameObject effect = GameObject.Instantiate(LevelUpEffect, this.transform.position, Quaternion.identity) as GameObject;

        OnLevelUp("LEVEL UP!");
    }
}
