using UnityEngine;
using UnityEngine.Advertisements;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;
using ChartboostSDK;

public class GameController : MonoBehaviour
{
	private IGameAnalytics analytics;

    protected PlayerController _player;
    protected MoonGenerator _generator;
    protected BackController Background;
    protected DeathDialogController DeathDialog;
    protected MusicController Music;

    public float InitialExperience;
    public float LevelFrecuency;
    public float LevelSpeed;
    public float ExperienceFactor;
    public float ExperienceSmoothConstant;
    public System.Action OnLevelUp;
    public GameObject BlackHole;


    protected int m_Level = 1;
    protected int m_LevelStep = 0;
    protected float m_NextLevelExperience;
    protected float m_PreviousExperience = 0;
    protected bool m_IsInBonusMode = false;
    protected bool m_IsGameOver = false;
    protected bool m_UseContinue = false;
    protected int m_PreviousLife;


    void Awake() {
        InitExternalComponents();

        DeathDialog.OnContinue += OnContinue;
        DeathDialog.OnGameOver += OnGameOver;

        _player.Lifes = InitialParameters.Lifes;
        _player.BonusExperience = InitialParameters.BonusExperience;

        InitialExperience = InitialParameters.Experience;
        LevelFrecuency = InitialParameters.Frecuency;
        LevelSpeed = InitialParameters.Speed;
        ExperienceFactor = InitialParameters.ConstantExperience;
        m_NextLevelExperience = InitialExperience;
        m_PreviousLife = _player.Lifes;
    }

    protected virtual void InitExternalComponents() {
        _player = GameObject.FindWithTag("Player").GetComponent<PlayerController>();
        _generator = GameObject.Find("MoonGenerator").GetComponent<MoonGenerator>();
        Background = GameObject.Find("Background").GetComponent<BackController>();
        DeathDialog = GameObject.Find("DeathDialog").GetComponent<DeathDialogController>();
        Music = GameObject.Find("Music").GetComponent<MusicController>();
		analytics = GameObject.Find("Analytics").GetComponent<IGameAnalytics>();
    }

    // Use this for initialization
    void Start() {
        InitGameController();        
    }

    protected virtual void InitGameController() {
        _generator.SetupNormalMode(LevelFrecuency, LevelSpeed);
        Chartboost.cacheRewardedVideo(CBLocation.Default);
    }

    string GetFPS() {
        int w = Screen.width, h = Screen.height;

        GUIStyle style = new GUIStyle();

        Rect rect = new Rect(0, 0, w, h * 2 / 100);
        style.alignment = TextAnchor.UpperLeft;
        style.fontSize = h * 2 / 100;
        style.normal.textColor = new Color (0.0f, 0.0f, 0.5f, 1.0f);
        float msec = Time.deltaTime * 1000.0f;
        float fps = 1.0f / Time.deltaTime;
        return string.Format("{0:0.0} ms ({1:0.} fps)", msec, fps);
    }

    private void Update() {
        UpdateGameController();
    }

    protected virtual void UpdateGameController() {
        if (!m_IsInBonusMode)
            UpdateNormalMode();
        else
            UpdateBonusMode();

        DebugGameInfo();       
    }

    
    protected virtual void CheckIfPlayerHasLoseALife() {
        if (_player.CurrentLife > 0 && m_PreviousLife != _player.CurrentLife) {
            _generator.DecreaseDifficulty();
            m_PreviousLife = _player.CurrentLife;
        } else if (!m_IsGameOver && _player.CurrentLife <= 0) {
            m_IsGameOver = true;
            _player.Death();

            _generator.DestroyMoons();
            _generator.enabled = false;
			/*
            if (!m_UseContinue && Application.internetReachability == NetworkReachability.ReachableViaLocalAreaNetwork) {
                this.StartCoroutine(DeathOrContinue());
                m_UseContinue = true;
            } else {
                this.StartCoroutine(GameOverCoroutine());
            }
            */
			this.StartCoroutine(GameOverCoroutine());
        }
    }

    protected virtual void UpdateNormalMode() {
        UpdateMusicInNormalMode();

        if (_player.CurrentExperience >= _player.BonusExperience) 
            StartBonusMode();
        else if (_player.TotalExperience >= m_NextLevelExperience) 
            SetupNextLevel();

        SetLevelStep();

        CheckIfPlayerHasLoseALife();
    }

    protected virtual void SetLevelStep() {
        for (int i = 1; i <= 5; i++) {
            if (i > m_LevelStep && 
                _player.TotalExperience - m_PreviousExperience > i * (m_NextLevelExperience - m_PreviousExperience) / 5) {
                this.SetLevelStep(i);
                m_LevelStep = i;
                break;
            }
        }
    }
    
    protected virtual void UpdateMusicInNormalMode() {
        if (_player.CurrentExperience > _player.BonusExperience / 3)
            Music.PlayLayer(2);

        if (_player.CurrentExperience > 2 * _player.BonusExperience / 3)
            Music.PlayLayer(3);

        if (_player.CurrentExperience >= _player.BonusExperience)
            Music.PlayBonus();
        else if (_player.TotalExperience >= m_NextLevelExperience)
            Music.ResetSound(true);

        if (_player.CurrentLife > 0 && m_PreviousLife != _player.CurrentLife)
            Music.ResetSound(true);
    }

    protected virtual void SetupNextLevel() {
        SetNextLevelExperience();
        UpdatePlayerLives();
        SetNextLevelSpeednFrecuency();
    }

    protected virtual void SetNextLevelExperience() {
        m_PreviousExperience = m_NextLevelExperience;
        m_NextLevelExperience = InitialExperience + m_PreviousExperience * (1 + ExperienceFactor / ExperienceSmoothConstant);
        m_Level++;
        m_LevelStep = 0;
    }

    protected virtual void UpdatePlayerLives() {
        if (_player.CurrentLife < _player.Lifes) {
            _player.CurrentLife++;
			analytics.GainedALife ();
            _player.OnUpdateLives();
        }
    }

    protected virtual void SetNextLevelSpeednFrecuency() {
        LevelFrecuency -= InitialParameters.LevelFrecuencyFactor;
        LevelSpeed += InitialParameters.LevelSpeedFactor;

        _generator.SetupNormalMode(LevelFrecuency, LevelSpeed);
        this.StartCoroutine(LevelUpCoroutine());
    }

    protected virtual void StartBonusMode() {
        _generator.SetupBonusMode();
        _player.InBonusMode = true;
        this.StartCoroutine(BeginBonusCoroutine());
        m_IsInBonusMode = true;
    }

    protected virtual void UpdateBonusMode() {
        if (_player.CurrentExperience == 0) {
            this.StartCoroutine(EndBonusCoroutine());
            _generator.DestroyMoons();
            _generator.SetupNormalMode();
            Music.ResetSound(false);
            m_IsInBonusMode = false;
            _player.InBonusMode = false;

            _player.BonusExperience = InitialParameters.BonusExperience + _player.BonusExperience * (1 + ExperienceFactor / ExperienceSmoothConstant);
        }
    }

    protected virtual void DebugGameInfo() {
        if (GameObject.Find("Info") != null)
            GameObject.Find("Info").GetComponent<Text>().text = string.Format("lifes={0},level{1}, frecuency:{2:F1}, speed:{3:F1}, experience:{4:F1},next_experience:{5:F1}, fps:{6}", _player.CurrentLife, m_Level, _generator.CurrentFrecuency, _generator.CurrentSpeed, _player.TotalExperience, m_NextLevelExperience, this.GetFPS());
    }

    public void SetLevelStep(int index)
    {
        switch (index)
        {
            case 0:
                _generator.EnableBlinking = false;
                _generator.EnableRotation = false;                    
                break;
            case 1:
                _generator.EnableBlinking = true;
                _generator.EnableRotation = false;
                break;
            case 2:
                _generator.EnableRotation = true;
                _generator.EnableBlinking = false;
                break;
            case 3:
                _generator.EnableBlinking = true;
                _generator.EnableRotation = true;
                break;            
        }    
    }

    public IEnumerator DeathOrContinue()
    {
        yield return new WaitForSeconds(2.0f);

        DeathDialog.Show();
    }

    public void OnContinue() {
        if (Chartboost.hasRewardedVideo(CBLocation.Default))
            Chartboost.showRewardedVideo(CBLocation.Default);
        else if (Advertisement.IsReady())
            Advertisement.Show();

        m_IsGameOver = false;
        _player.Alive();
        _player.CurrentLife = 1;
        _player.OnUpdateLives();

        _generator.enabled = true;
    }

    public void OnGameOver() {
        this.StartCoroutine(GameOverCoroutine());
    }

	IEnumerator GameOverCoroutine() {          
		analytics.GameOver();     
        GameObject black = GameObject.Instantiate(BlackHole, this.transform.position, Quaternion.identity) as GameObject;
        black.transform.localScale = Vector3.one * 3;
        black.GetComponent<SpriteRenderer>().sortingLayerName = "Frente";

        yield return new WaitForSeconds(2);

        DestroyObject(GameObject.Find("Canvas"));
        transform.FindChild("GameOverFade").GetComponent<Animator>().enabled = true;

        yield return new WaitForSeconds(1);

        Persistence.SaveSingleGame(_player.TotalScore, _player.TotalExperience, _player.GameTime);
        Persistence.Save();
        /*
        Social.ReportScore(_player.TotalScore, GPGIds.leaderboard_top_one_gamers, (bool sucess) =>
            {
            
            });
            */
        SceneManager.LoadScene("GameOver", LoadSceneMode.Single);
    }

    public IEnumerator BeginBonusCoroutine()
    {
        _player.transform.FindChild("Pulse").GetComponent<SpriteRenderer>().enabled = false;
        _player.transform.FindChild("Damage").GetComponent<SpriteRenderer>().enabled = false;

        GameObject.Find("WormHoleEffect").GetComponent<Animator>().SetTrigger("BeginBonus");
        Background.Fade();  
        Background.SetParticles(false);

        yield return new WaitForSeconds(3.0f);
    }

    public IEnumerator EndBonusCoroutine()
    {
        _player.transform.FindChild("Pulse").GetComponent<SpriteRenderer>().enabled = true;
        _player.transform.FindChild("Damage").GetComponent<SpriteRenderer>().enabled = true;

        GameObject.Find("WormHoleEffect").GetComponent<Animator>().SetTrigger("EndBonus");
        Background.UndoFade();    
        Background.SetParticles(true);

        yield return new WaitForSeconds(3.0f);
    }

    public IEnumerator LevelUpCoroutine()
    {
        _player.SetLevelUp();

        yield return new WaitForSeconds(1.0f);

        GameObject.Find("WormHoleEffect").GetComponent<Animator>().SetTrigger("LevelUp");

        Background.SetLevelUp();
    }
}
