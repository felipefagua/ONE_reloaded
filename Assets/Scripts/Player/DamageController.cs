using UnityEngine;
using System.Collections;

public class DamageController : MonoBehaviour {
    public Sprite[] DamageSprites;
    public int DamageIndex;
    public float Speed;

    protected int m_PreviousIndex;

	// Use this for initialization
	void Start () {
        this.GetComponent<SpriteRenderer>().enabled = false;
        this.GetComponent<Animator>().enabled = false;
    }
	
	// Update is called once per frame
	void Update () {

        if (m_PreviousIndex != DamageIndex && DamageIndex >= 0)
        {
            this.GetComponent<SpriteRenderer>().enabled = true;
            this.GetComponent<Animator>().enabled = true;
            this.GetComponent<SpriteRenderer>().sprite = DamageSprites[DamageIndex];
            this.GetComponent<Animator>().speed = Speed;

            m_PreviousIndex = DamageIndex;
        }
    }
}