using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ApplicationPause : MonoBehaviour
{
    public static ApplicationPause instance { get; set; }

    public bool isPaused { get; set; }

    protected bool m_IsPaused = false;
    protected float m_Time = 0;

    public void Awake() {
        instance = this;
    }

    // Use this for initialization
    private void Start() {
        this.transform.FindChild("Image").GetComponent<Image>().enabled = false;
        this.transform.FindChild("Text1").gameObject.SetActive(false);
        this.transform.FindChild("Text2").gameObject.SetActive(false);
        this.transform.FindChild("Text3").gameObject.SetActive(false);
    }

    private void EndPause() {
        isPaused = false;
        m_IsPaused = false;
        this.transform.FindChild("Image").GetComponent<Image>().enabled = false;
        this.transform.FindChild("Text1").gameObject.SetActive(false);
        this.transform.FindChild("Text2").gameObject.SetActive(false);
        this.transform.FindChild("Text3").gameObject.SetActive(false);

        Time.timeScale = 1;
        Debug.LogWarning("Setting back the time scale to 1");
    }

    void OnApplicationPause(bool paused)
    {
        if (m_IsPaused && !paused)
            this.SetPause();

        m_IsPaused = paused;
    }

    public void SetPause()
    {
        this.GetComponent<Animator>().SetTrigger("Pause");
        this.transform.FindChild("Image").GetComponent<Image>().enabled = true;
        this.transform.FindChild("Text1").gameObject.SetActive(true);

        Time.timeScale = 0;
        isPaused = true;
    }
}
