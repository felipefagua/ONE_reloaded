using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System;

public class PerformanceController : MonoBehaviour {

    public GameObject LifePrefab;
    public float LifesInterSpace;
    public float LifesScale;
    protected PlayerController Player;
    protected Transform m_TextPerformance;
    protected Transform m_TextScore;
    protected Transform m_TextLevelUp;
    protected Transform m_TextBonusMode;
    protected Transform m_ProgressBar;
    protected Transform m_Lifes;
    
    protected int m_PreviousScore;
    
    [SerializeField]
    private Image[] _lifeIndicators;

    [SerializeField]
    private GameObject _lifeExplosionEffect;

    [SerializeField]
    private GameObject _fullChargedEffect;

    [SerializeField]
    private Image _xpProgressBar;

    [SerializeField]
    private Image _xpProgressBarEffectPosition;

    [SerializeField]
    private Camera _camera;

    // Use this for initialization
    void Start () {

        Player = GameObject.FindWithTag("Player").GetComponent<PlayerController>();
        Player.OnMoon += OnMoon;
        Player.OnLevelUp += OnLevelUp;
        Player.OnUpdateLives += OnUpdateLives;

        m_TextPerformance = transform.FindChild("Performance");
        m_ProgressBar = transform.FindChild("ProgressBar");
        m_TextPerformance.GetComponent<Text>().text = string.Empty;
        m_TextScore = transform.FindChild("Score");
        m_TextLevelUp = transform.FindChild("LevelUp");
        m_TextBonusMode = transform.FindChild("BonusMode");
        m_Lifes = transform.FindChild("Lifes");
       
        m_TextLevelUp.GetComponent<Text>().enabled = false;

        _xpProgressBar.fillAmount = 0;
        for(int i=0; i < Player.Lifes; i++)
        {
            GameObject life = GameObject.Instantiate(LifePrefab, m_Lifes.transform.position, Quaternion.identity) as GameObject;
            life.transform.parent = m_Lifes.transform;
            life.transform.localPosition -= i * Vector3.right * LifesInterSpace;
            life.transform.localScale = Vector3.one * LifesScale;
        }

	}
	
    // Update is called once per frame
	void Update () {
        
	}

    void OnMoon(EAfinityType type, int count, int score)
    {
        string message = string.Empty;
        if (count == 1)
            message = this.GetAfinityText(type);
        else
            message = string.Format("COMBO x {0}", count);

        this.SetPerformance(score, message);
    }

    private void OnUpdateLives() {
        for (int i = 0; i < _lifeIndicators.Length; i++) {
            UpdateLife(_lifeIndicators[i], i, Player.CurrentLife);
        }
    }

    private void UpdateLife(Image lifeIndicator, int lifeIndicatorIndex, int playerLives) {
        int lastIndexAlive = playerLives - 1;
        if (lastIndexAlive >= lifeIndicatorIndex)
            TurnOnLifeindicator(lifeIndicator);
        else
            TurnOffLifeindicator(lifeIndicator);
    }

    private void TurnOnLifeindicator(Image lifeIndicator) {
        if (lifeIndicator.color != Color.white) {
            lifeIndicator.color = Color.white;        
        }
    }

    private void TurnOffLifeindicator(Image lifeIndicator) {
        if (lifeIndicator.color.a != 0) {
            Color color = lifeIndicator.color;
            color.a = 0;
            lifeIndicator.color = color;
            ShowExplosionLifeEffect(lifeIndicator);
        }
    }

    private void ShowExplosionLifeEffect(Image lifeIndicator) {

        Vector3 particles3DPosition = _camera.ScreenToWorldPoint(lifeIndicator.transform.position);

        particles3DPosition.z = _lifeExplosionEffect.transform.position.z;
        //particles3DPosition.x -= _camera.transform.position.x;

        Debug.LogWarning("ShowExplosionLifeEffect particles3DPosition: " + particles3DPosition);

        _lifeExplosionEffect.transform.position = particles3DPosition;
        _lifeExplosionEffect.SetActive(false);
        _lifeExplosionEffect.SetActive(true);
    }

    void OnCombo(EAfinityType type, int count, int score)
    {
        string message = string.Format("{0} x {1}", this.GetAfinityText(type), count);
        this.SetPerformance(score, message);
    }

    string GetAfinityText(EAfinityType type)
    {
        string message = string.Empty;

        switch (type)
        {
            case EAfinityType.LORD:
                message = "LORD OF LIGHT!";
                break;
            case EAfinityType.EPIC:
                message = "EPIC!";
                break;
            case EAfinityType.AWESOME:
                message = "AWESOME!";
                break;
            case EAfinityType.GREAT:
                message = "GREAT!";
                break;
            case EAfinityType.GOOD:
                message = "GOOD!";
                break;
            case EAfinityType.BAD:
                message = "BAD!";
                break;
        }

        return message;
    }

    void SetPerformance(int score, string message) {        
        m_TextPerformance.GetComponent<Text>().text = message;
        m_TextPerformance.GetComponent<Animator>().SetTrigger("Show");

        float alpha = (Player.CurrentExperience / Player.BonusExperience);
        alpha = Mathf.Clamp(alpha, 0, 1);

        float percent = alpha * 100;

        _xpProgressBar.fillAmount = alpha;

        if (alpha >= 1)
        //if (true)
            ShowFullyChargedEffect();
        else
            HideFullyChargedEffect();

        this.StartCoroutine(OnScoreCoroutine(score));
    }

    private void ShowFullyChargedEffect() {
        Vector3 particles3DPosition = _camera.ScreenToWorldPoint(_xpProgressBarEffectPosition.transform.position);

        particles3DPosition.z = _fullChargedEffect.transform.position.z;
        //particles3DPosition.x -= _camera.transform.position.x;

        Debug.LogWarning("ShowExplosionLifeEffect particles3DPosition: " + particles3DPosition);

        _fullChargedEffect.transform.position = particles3DPosition;
        _fullChargedEffect.SetActive(false);
        _fullChargedEffect.SetActive(true);

        CancelInvoke("TurnOnChargedEffect");
        InvokeRepeating("TurnOnChargedEffect", 2, 2);
    }

    private void HideFullyChargedEffect() {
        _fullChargedEffect.SetActive(false);
        CancelInvoke("TurnOnChargedEffect");
    }

    private void TurnOnChargedEffect() {
        _fullChargedEffect.SetActive(false);
        _fullChargedEffect.SetActive(true);
    }

    IEnumerator OnScoreCoroutine(int score)
    {
        iTween.ScaleTo(m_TextScore.gameObject, iTween.Hash(
            "scale", Vector3.one * 1.05f, 
            "time", 0.5f));

        int step = (score - m_PreviousScore) / 5;
        for (int i = 0; i < 5; i++)
        {
            m_TextScore.GetComponent<Text>().text = (m_PreviousScore + i * step).ToString();
            yield return new WaitForSeconds(0.1f);
        }

        iTween.ScaleTo(m_TextScore.gameObject, iTween.Hash(
            "scale", Vector3.one, 
            "time", 0.5f));        

        m_PreviousScore = score;

        m_TextScore.GetComponent<Text>().text = score.ToString();
    }

    void OnLevelUp(string message)
    {
        m_TextLevelUp.GetComponent<Text>().enabled = true;
        m_TextLevelUp.GetComponent<Text>().text = message;
        m_TextLevelUp.GetComponent<Animator>().SetTrigger("Show");
    }
}

