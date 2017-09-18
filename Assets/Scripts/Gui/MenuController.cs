using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;

public class MenuController : MonoBehaviour
{
    [SerializeField]
    private string _mainMenuSceneName;
	private IGameAnalytics analytics;
    protected Vector3 m_Scale;
    protected Vector3 m_DialogScale;
    protected GameObject m_PauseButton1;
    protected GameObject m_PauseButton2;
    protected GameObject m_Dialog;
    protected GameObject m_Fade;
    protected string m_Action;

    public string mainMenuSceneName { get { return _mainMenuSceneName; } }

    private void Awake() {
		m_Scale = this.transform.localScale;
		this.transform.localScale = Vector3.zero;

		m_PauseButton1 = GameObject.Find ("BtnPause");
		m_PauseButton2 = GameObject.Find ("BtnPause2");

		analytics = GameObject.Find("Analytics").GetComponent<IGameAnalytics>();

        m_Dialog = GameObject.Find("Dialog");
        m_DialogScale = m_Dialog.transform.localScale;
        m_Dialog.transform.localScale = Vector3.zero;

        m_Fade = GameObject.Find("Fade");
    }
	
    public void OnPause() {   
        if (Time.timeScale != 0) {            
            GetComponent<AudioSource>().Play();
            transform.localScale = m_Scale;
            m_Dialog.transform.localScale = Vector3.zero;
            this.EnablePauseButton(false);

            Time.timeScale = 0.0f;
            m_Fade.GetComponent<Image>().color = new Color(0, 0, 0, 0.75f);
        }
    }    

    public void OnMainMenu()
    {
        this.GetComponent<AudioSource>().Play();
        this.transform.localScale = Vector3.zero;
        m_Dialog.transform.localScale = m_DialogScale;
        m_Dialog.transform.FindChild("Message").GetComponent<Text>().text = "Are you sure you want to return to the main menu?".ToUpper();
        m_Dialog.GetComponent<DeathDialogController>().Show();
        m_Action = "MainMenu";
    }

    public void OnRetry()
    {
        this.GetComponent<AudioSource>().Play();
        this.transform.localScale = Vector3.zero;
        m_Dialog.transform.localScale = m_DialogScale;
        m_Dialog.transform.FindChild("Message").GetComponent<Text>().text = "Do you want to restart the game again?".ToUpper();
        m_Dialog.GetComponent<DeathDialogController>().Show();
        m_Action = "Retry";
    }

    public void OnResume() {
        HidePauseMenu();
        GetComponent<AudioSource>().Play();
        GameObject.Find("AppPause").GetComponent<ApplicationPause>().SetPause();
        EnablePauseButton(true);
    }

    public void HidePauseMenu() {
        transform.localScale = Vector3.zero;
        m_Dialog.transform.localScale = Vector3.zero;
        m_Fade.GetComponent<Image>().color = new Color(0, 0, 0, 0);
    }

    private void EnablePauseButton(bool enable)
    {
        if (enable)
        {
            m_PauseButton1.GetComponent<Animator>().SetTrigger("Go");

            if (m_PauseButton2 != null)
                m_PauseButton2.GetComponent<Animator>().SetTrigger("Go");
        }
        else
        {
            m_PauseButton1.GetComponent<Animator>().SetTrigger("Pause");

            if (m_PauseButton2 != null)
                m_PauseButton2.GetComponent<Animator>().SetTrigger("Pause");
        }
    }

    public void OnYes() {
        this.GetComponent<AudioSource>().Play();
        
        switch (m_Action) {
			case "MainMenu":
				SceneManager.UnloadScene (SceneManager.GetActiveScene ().name);
				SceneManager.LoadScene (_mainMenuSceneName, LoadSceneMode.Single);
				analytics.ExitSession ();
                break;
            case "Retry":
                Persistence.SelectedGameMode = SceneManager.GetActiveScene().name;
                SceneManager.LoadScene("Loading", LoadSceneMode.Single);
                break;
        }
    }

    public void OnNo()
    {
        this.GetComponent<AudioSource>().Play();
        this.transform.localScale = m_Scale;
        m_Dialog.transform.localScale = Vector3.zero;
    }
}
