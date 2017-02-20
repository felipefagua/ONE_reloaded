using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
//using GooglePlayGames;
using UnityEngine.SocialPlatforms;
using System.Collections;

public class MainMenuController : IMainMenuController {

    public static MainMenuController instance;

    public float AngularSpeed;
    public float AnimationTime;
    Transform m_BtnSinglePlayer;
    Transform m_BtnMultiPlayer;
    Transform m_BtnOptions;
    Transform m_MenuSingle;
    Transform m_MenuMulti;
    Transform m_MenuOptions;
    Transform m_DialogScore;
    GameObject m_TouchScreen;

    protected Vector3 m_Scale;
    protected Vector3 m_CenterScale;
    protected bool m_IsEnabled;
    protected float m_Time;
    protected bool m_IsSingleScore;

    private void Awake() {
        instance = this;
    }

    private void Start () {

        m_BtnSinglePlayer = transform.FindChild("BtnSinglePlayer");
        m_BtnMultiPlayer = transform.FindChild("BtnMultiPlayer");
        m_BtnOptions = transform.FindChild("BtnOptions");
        m_MenuSingle = transform.FindChild("MenuSinglePlayer");
        m_MenuMulti = transform.FindChild("MenuMultiPlayer");
        m_MenuOptions = transform.FindChild("MenuOptions");
        m_DialogScore = transform.FindChild("DialogScore");
        m_TouchScreen = GameObject.Find("TouchScreen");

        m_BtnSinglePlayer.GetComponent<Animator>().enabled = false;
        m_BtnMultiPlayer.GetComponent<Animator>().enabled = false;
        m_BtnOptions.GetComponent<Animator>().enabled = false;

        m_Scale = this.transform.localScale;

        m_CenterScale = m_MenuSingle.localScale;
        m_MenuSingle.localScale = Vector3.zero;
        m_MenuMulti.localScale = Vector3.zero;
        m_DialogScore.localScale = Vector3.zero;


        this.transform.localScale = Vector3.zero;
	}
	
	public override void Show() {
        this.StartCoroutine(ShowCoroutine());
    }

    IEnumerator ShowCoroutine()
    {
        this.transform.localScale = m_Scale;
        m_MenuSingle.localScale = Vector3.zero;
        m_MenuMulti.localScale = Vector3.zero;
        m_MenuOptions.localScale = Vector3.zero;

        m_BtnSinglePlayer.FindChild("Text").transform.localScale = Vector3.zero;
        m_BtnMultiPlayer.FindChild("Text").transform.localScale = Vector3.zero;
        m_BtnOptions.FindChild("Text").transform.localScale = Vector3.zero;

        this.GetComponent<Animator>().SetTrigger("Intro");

        yield return new WaitForSeconds(3.0f);

        m_BtnSinglePlayer.GetComponent<Animator>().enabled = true;
        m_BtnMultiPlayer.GetComponent<Animator>().enabled = true;
        m_BtnOptions.GetComponent<Animator>().enabled = true;

        iTween.ScaleTo(m_BtnSinglePlayer.FindChild("Text").gameObject, Vector3.one, 0.5f);
        iTween.ScaleTo(m_BtnMultiPlayer.FindChild("Text").gameObject, Vector3.one, 0.5f);
        iTween.ScaleTo(m_BtnOptions.FindChild("Text").gameObject, Vector3.one, 0.5f);
    }


    public void OnSinglePlayer()
    {
        this.GetComponent<AudioSource>().Play();

        m_DialogScore.localScale = Vector3.zero;
        if(m_BtnOptions.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsName("Open"))
            m_BtnOptions.GetComponent<Animator>().SetTrigger("Close");

        if(m_BtnMultiPlayer.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsName("Open"))
            m_BtnMultiPlayer.GetComponent<Animator>().SetTrigger("Close");

        m_BtnSinglePlayer.GetComponent<Animator>().SetTrigger("Open");  
        iTween.ScaleTo(m_MenuSingle.gameObject, m_CenterScale, 0.5f);

        m_MenuMulti.localScale = Vector3.zero;
        m_MenuOptions.localScale = Vector3.zero;

        this.StartCoroutine(DisableEnableButtons());
    }

    public void OnMultiPlayer()
    {
        this.GetComponent<AudioSource>().Play();

        m_DialogScore.localScale = Vector3.zero;
        m_BtnSinglePlayer.GetComponent<Button>().enabled = false;
        m_BtnOptions.GetComponent<Button>().enabled = false;

        if(m_BtnOptions.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsName("Open"))
            m_BtnOptions.GetComponent<Animator>().SetTrigger("Close");

        if(m_BtnSinglePlayer.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsName("Open"))
            m_BtnSinglePlayer.GetComponent<Animator>().SetTrigger("Close");
        
        m_BtnMultiPlayer.GetComponent<Animator>().SetTrigger("Open"); 

        m_MenuSingle.localScale = Vector3.zero;
        m_MenuOptions.localScale = Vector3.zero;

        iTween.ScaleTo(m_MenuMulti.gameObject, m_CenterScale, 0.5f);

        this.StartCoroutine(DisableEnableButtons());
    }

    public void OnOptions()
    {
        this.GetComponent<AudioSource>().Play();

        m_DialogScore.localScale = Vector3.zero;
        m_BtnSinglePlayer.GetComponent<Button>().enabled = false;
        m_BtnMultiPlayer.GetComponent<Button>().enabled = false;

        if(m_BtnSinglePlayer.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsName("Open"))
            m_BtnSinglePlayer.GetComponent<Animator>().SetTrigger("Close");

        if(m_BtnMultiPlayer.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsName("Open"))
            m_BtnMultiPlayer.GetComponent<Animator>().SetTrigger("Close");
        
        m_BtnOptions.GetComponent<Animator>().SetTrigger("Open");  

        m_MenuSingle.localScale = Vector3.zero;
        m_MenuMulti.localScale = Vector3.zero;

        iTween.ScaleTo(m_MenuOptions.gameObject, m_CenterScale, 0.5f);

        this.StartCoroutine(DisableEnableButtons());
    }

    public void OnSingleScore()
    {
        m_IsSingleScore = true;
        this.StartCoroutine(OptionsCoroutine("Score"));
    }

    public void OnMultiScore()
    {
        m_IsSingleScore = false;
        this.StartCoroutine(OptionsCoroutine("Score"));    
    }

    public void OnScoreOk()
    {
        this.GetComponent<AudioSource>().Play();

        if (m_IsSingleScore)
            m_MenuSingle.localScale = Vector3.one;
        else
            m_MenuMulti.localScale = Vector3.one;
        
        m_DialogScore.transform.localScale = Vector3.zero;
    }

    public void OnMusic()
    {
        if (SoundManager.IsMusicOn)
            GameObject.Find("BtnMusic").GetComponentInChildren<Text>().text = "MUSIC OFF";
        else
            GameObject.Find("BtnMusic").GetComponentInChildren<Text>().text = "MUSIC ON";

        GameObject.Find("Sound").GetComponent<SoundManager>().SetMusic(!SoundManager.IsMusicOn);
    }

    public void OnSFX()
    {
        if (SoundManager.IsSFXOn)
            GameObject.Find("BtnSFX").GetComponentInChildren<Text>().text = "SFX OFF";
        else
            GameObject.Find("BtnSFX").GetComponentInChildren<Text>().text = "SFX ON";        

        GameObject.Find("Sound").GetComponent<SoundManager>().SetSFX(!SoundManager.IsSFXOn);
    }

    IEnumerator DisableEnableButtons()
    {
        m_BtnSinglePlayer.GetComponent<Button>().enabled = false;
        m_BtnOptions.GetComponent<Button>().enabled = false;
        m_BtnMultiPlayer.GetComponent<Button>().enabled = false;

        yield return new WaitForSeconds(0.5f);

        m_BtnSinglePlayer.GetComponent<Button>().enabled = true;
        m_BtnOptions.GetComponent<Button>().enabled = true;
        m_BtnMultiPlayer.GetComponent<Button>().enabled = true;
    }


    public void OnSinglePlay()
    {
        this.StartCoroutine(OptionsCoroutine("SinglePlay"));
    }

    public void OnMultiPlay()
    {
        this.StartCoroutine(OptionsCoroutine("MultiPlay"));
    }

    public void OnAchievements()
    {
        this.StartCoroutine(OptionsCoroutine("Achievements"));
    }

    public void OnLeaderBoard()
    {
        this.StartCoroutine(OptionsCoroutine("LeaderBorard"));
    }

    public void OnParameters()
    {
        this.StartCoroutine(OptionsCoroutine("Parameters"));
    }

    public void OnTutorial() {
        StartCoroutine(OptionsCoroutine("Tutorial"));
    }

    public IEnumerator OptionsCoroutine(string option)
    {
        this.GetComponent<AudioSource>().Play();

        iTween.ColorTo(m_TouchScreen, Color.white, 1.0f);

        yield return new WaitForSeconds(1.0f);

        switch (option)
        {
            case "Tutorial":
                Persistence.SelectedGameMode = "test_scene_tutorial";
                SceneManager.LoadScene("Loading", LoadSceneMode.Single);
                break;
            case "SinglePlay":
                Persistence.SelectedGameMode = "SimpleGame";
                SceneManager.LoadScene("Loading", LoadSceneMode.Single);
                break;
            case "MultiPlay":
                Persistence.SelectedGameMode = "MultiGame";
                SceneManager.LoadScene("Loading", LoadSceneMode.Single);
                break;
            case "LeaderBorard":
                //PlayGamesPlatform.Instance.ShowLeaderboardUI(GPGIds.leaderboard_top_one_gamers);
                break;
            case "Achievements":
                Social.ShowAchievementsUI();
                break;
            case "Score":
                m_MenuSingle.localScale = Vector3.zero;
                m_MenuMulti.localScale = Vector3.zero;
                m_DialogScore.localScale = Vector3.one;

                m_DialogScore.FindChild("Score").GetComponent<Text>().text = Persistence.Data.TopGame.Score.ToString();
                m_DialogScore.FindChild("Experience").GetComponent<Text>().text = Persistence.Data.TopGame.Experience.ToString("0.00");
                System.TimeSpan time = System.TimeSpan.FromSeconds(Persistence.Data.TopGame.Time);
                m_DialogScore.FindChild("Time").GetComponent<Text>().text = time.ToString();

                break;
            case "Parameters":
                SceneManager.LoadScene("SetupParameters", LoadSceneMode.Single);
                break;
        }
    }
}
