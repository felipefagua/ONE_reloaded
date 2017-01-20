using UnityEngine;
using System.Collections;

public class BackController : MonoBehaviour {

    public GameObject[] BackgroundPrefabs;

    protected PlayerController m_Player;
    protected int m_BackgroundIndex = 0;

    protected GameObject m_CurrentBackground;
    protected GameObject m_NextBackground;
    protected ParticleSystem m_IdleParticles;

	// Use this for initialization
	void Start () {
        m_Player = GameObject.FindObjectOfType<PlayerController>();

        m_CurrentBackground = GameObject.Instantiate(BackgroundPrefabs[m_BackgroundIndex], this.transform.position, Quaternion.identity) as GameObject;
        m_CurrentBackground.transform.parent = this.transform;

        m_IdleParticles = transform.FindChild("IdleParticles").GetComponent<ParticleSystem>();
	}
	
	// Update is called once per frame
	void Update () {

    }
       
    public void SetLevelUp()
    {
        m_BackgroundIndex++;

        if (m_BackgroundIndex == BackgroundPrefabs.Length)
            m_BackgroundIndex = 0;
        
        m_NextBackground = GameObject.Instantiate(BackgroundPrefabs[m_BackgroundIndex], this.transform.position, Quaternion.identity) as GameObject;
        m_NextBackground.transform.parent = this.transform;

        this.StartCoroutine(OnChangeBackground());
    }

    IEnumerator OnChangeBackground()
    {
        m_CurrentBackground.GetComponent<BackgroundController>().Fade(1.5f); 

        yield return new WaitForSeconds(1.5f);

        DestroyObject(m_CurrentBackground);
        m_CurrentBackground = null;
        m_CurrentBackground = m_NextBackground;
    }

    public void Fade()
    {
        m_CurrentBackground.GetComponent<BackgroundController>().Fade(3.0f); 
    }

    public void UndoFade()
    {
        m_CurrentBackground.GetComponent<BackgroundController>().UndoFade(3.0f); 
    }

    public void SetParticles(bool status)
    {
        m_IdleParticles.enableEmission = status;
    }
}
