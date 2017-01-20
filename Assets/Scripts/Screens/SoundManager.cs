using UnityEngine;
using System.Collections;

public class SoundManager : MonoBehaviour {
    public static bool IsMusicOn = true;
    public static bool IsSFXOn = true;
 
	// Use this for initialization
	void Awake () {
	
        SetSound();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void SetMusic(bool status)
    {
        IsMusicOn = status;
        SetSound();
    }

    public void SetSFX(bool status)
    {
        IsSFXOn = status;
        SetSound();
    }

    void SetSound()
    {
        AudioSource[] sounds = GameObject.FindObjectsOfType<AudioSource>();

        for(int i=0; i < sounds.Length; i++)
        {
            if (sounds[i].gameObject == Camera.main.gameObject)
                sounds[i].enabled = IsMusicOn;
            else
                sounds[i].enabled = IsSFXOn;
        }        

        if (GameObject.Find("Music") != null)
        {
            AudioSource[] music = GameObject.Find("Music").GetComponentsInChildren<AudioSource>();
            for(int i=0; i < music.Length; i++)
                music[i].enabled = IsMusicOn;
        }
    }
}
