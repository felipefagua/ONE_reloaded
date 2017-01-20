using UnityEngine;
using System.Collections;
using GAF.Core;

public class BackgroundController : MonoBehaviour
{
    public string ClipIntro;
    public string ClipLoop;
    protected int m_StateIndex = 0;
    protected GAFMovieClip m_Clip;
	
    // Use this for initialization
    void Start()
    {
         m_Clip = transform.FindChild("Clip").GetComponent<GAFMovieClip>();
         m_Clip.setSequence(ClipIntro, false);
         this.Invoke("BeginIntro", 3);
    }
	
    // Update is called once per frame
    void Update()
    {
        
       
    }

    public void BeginIntro()
    {       
        m_Clip.setSequence(ClipIntro, true);
        m_Clip.setAnimationWrapMode(GAFInternal.Core.GAFWrapMode.Once);
        m_Clip.play();

        this.Invoke("BeginLoop", m_Clip.duration());
    }

    public void BeginLoop()
    {
        m_Clip.setSequence(ClipLoop, true);
        m_Clip.setAnimationWrapMode(GAFInternal.Core.GAFWrapMode.Loop);
        m_Clip.play();
    }

    public void Fade(float time)
    {
        for (int i = 0; i < this.transform.childCount; i++)
        {
            GameObject back = this.transform.GetChild(i).gameObject;
            if(back.GetComponent<SpriteRenderer>() != null)
                iTween.ColorTo(back, new Color(0, 0, 0, 0), time); 
        }   

        iTween.ValueTo(this.gameObject, iTween.Hash("from", 1, "to", 0, "time", time, "onupdate","FadeClip"));            
    }

    public void UndoFade(float time)
    {
        for (int i = 0; i < this.transform.childCount; i++)
        {
            GameObject back = this.transform.GetChild(i).gameObject;
            if(back.GetComponent<SpriteRenderer>() != null)
                iTween.ColorTo(back, new Color(1, 1, 1, 1), time); 

            iTween.ValueTo(this.gameObject, iTween.Hash("from", 0, "to", 1, "time", time, "onupdate","FadeClip"));
        }   
    }

    public void FadeClip(float value)    
    {
        for (int i = 0; i < m_Clip.transform.childCount; i++)
        {
            GameObject back = m_Clip.transform.GetChild(i).gameObject;                 
            back.GetComponent<MeshRenderer>().material.SetColor("_CustomColorMultiplier", new Color(1,1,1,value));
        }
    }
}
