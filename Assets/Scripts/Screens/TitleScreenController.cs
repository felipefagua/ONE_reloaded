using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class TitleScreenController : MonoBehaviour {
    public float FadeTime;
    public bool ResetData;
    [SerializeField]
    private IMainMenuController _menuController;

    protected Transform m_TouchScreen;
    protected Transform m_Info;
    protected float m_FadeValue;
    protected bool m_IsFading = false;

	// Use this for initialization
	void Start () {
        m_TouchScreen = GameObject.Find("TouchScreen").transform;
        m_Info = GameObject.Find("Info").transform;

        m_TouchScreen.GetComponent<Button>().enabled = false;
        m_Info.GetComponent<Text>().enabled = false;
        this.Invoke("EnableInfo", 4);

        Time.timeScale = 1;

        if (ResetData)
            Persistence.Reset();
	}
	
	// Update is called once per frame
	void Update () {
        if (m_IsFading && m_FadeValue < FadeTime)
        {
            m_FadeValue += FadeTime * Time.deltaTime;
            m_TouchScreen.GetComponent<Image>().color = new Color(1, 1, 1, m_FadeValue);
        }
	}

    public void StartMenu() {
		/*
        if (LocalDataController.instance.hasPassTutorial)
            GotoMainMenu();
        else
            GotoTutorialScene();
		*/
		GotoMainMenu ();
	}

    public void GotoMainMenu() {
        iTween.ColorTo(GameObject.Find("logoon"), new Color(0, 0, 0, 0), 1.0f);
        iTween.ColorTo(GameObject.Find("logooff"), new Color(0, 0, 0, 0), 1.0f);
        iTween.ColorTo(GameObject.Find("halo"), new Color(0, 0, 0, 0), 1.0f);
        iTween.ColorTo(GameObject.Find("destello"), new Color(0, 0, 0, 0), 1.0f);

        m_Info.GetComponent<Text>().enabled = false;
        m_TouchScreen.GetComponent<Image>().enabled = false;
        m_TouchScreen.GetComponent<Button>().enabled = false;

        _menuController.Show();
    }

    private void GotoTutorialScene() {
        _menuController.OnTutorial();
    }

    public void EnableInfo()
    {
        m_Info.GetComponent<Text>().enabled = true;
        m_TouchScreen.GetComponent<Button>().enabled = true;
    }
}

