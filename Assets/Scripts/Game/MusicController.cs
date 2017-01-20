using UnityEngine;
using System.Collections;

public class MusicController : MonoBehaviour {

    protected int m_Layer;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void PlayLayer(int index)
    {
        switch (index)
        {
            case 2:
                if(!transform.FindChild("Layer2").GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsName("FadeMusicOn"))
                    transform.FindChild("Layer2").GetComponent<Animator>().SetTrigger("On");
                break;
            case 3:
                if(!transform.FindChild("Layer3").GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsName("FadeMusicOn"))
                    transform.FindChild("Layer3").GetComponent<Animator>().SetTrigger("On");
                break;
        }

    }

    public void ResetSound(bool fullReset)
    {
        string trigger = fullReset ? "Off" : "On";
        transform.FindChild("Layer1").GetComponent<Animator>().SetTrigger("On");
        transform.FindChild("Layer2").GetComponent<Animator>().SetTrigger(trigger);
        transform.FindChild("Layer3").GetComponent<Animator>().SetTrigger(trigger);

        transform.FindChild("Bonus").GetComponent<Animator>().SetTrigger("Off");
    }


    public void PlayBonus()
    {
        transform.FindChild("Layer1").GetComponent<Animator>().SetTrigger("Off");
        transform.FindChild("Layer2").GetComponent<Animator>().SetTrigger("Off");
        transform.FindChild("Layer3").GetComponent<Animator>().SetTrigger("Off");

        transform.FindChild("Bonus").GetComponent<Animator>().SetTrigger("On");
    }
}
