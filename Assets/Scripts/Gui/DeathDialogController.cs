using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using ChartboostSDK;

public class DeathDialogController : MonoBehaviour {

    public System.Action OnContinue;
    public System.Action OnGameOver;

    Vector3 m_Scale;
	void Awake()
	{
		m_Scale = this.transform.localScale;
	}
	// Use this for initialization
	void Start () {
        this.transform.localScale = Vector3.zero;
	}
	
	// Update is called once per frame
	void Update () {

	}

    public void Show()
    {
        Time.timeScale = 0; 
		this.transform.localScale = m_Scale;
        GameObject.Find("Fade").GetComponent<Image>().color = new Color(0, 0, 0, 0.75f);
    }

    public void OnYes()
    {
        Time.timeScale = 1;
        this.transform.localScale = Vector3.zero;
        GameObject.Find("Fade").GetComponent<Image>().color = new Color(0, 0, 0, 0);
        OnContinue();
    }

    public void OnNo()
    {
        Time.timeScale = 1;
        this.transform.localScale = Vector3.zero;
        GameObject.Find("Fade").GetComponent<Image>().color = new Color(0, 0, 0, 0);
        OnGameOver();
    }
}
