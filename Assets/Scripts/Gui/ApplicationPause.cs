using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ApplicationPause : MonoBehaviour
{

    protected bool m_IsPaused = false;
    protected float m_Time = 0;

    // Use this for initialization
    void Start()
    {
        this.transform.FindChild("Image").GetComponent<Image>().enabled = false;
        this.transform.FindChild("Text1").gameObject.SetActive(false);
        this.transform.FindChild("Text2").gameObject.SetActive(false);
        this.transform.FindChild("Text3").gameObject.SetActive(false);
    }

    void EndPause()
    {
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
    }
}
