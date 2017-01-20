using UnityEngine;
using System.Collections;

public class EffectController : MonoBehaviour {
    public float LifeTime;

    protected Color m_OldColor;
	// Use this for initialization
	void Start () {
        DestroyObject(this.gameObject, LifeTime);    

        this.SetEffectInfo();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void SetEffectInfo()
    {
        ParticleSystem[] effects = this.GetComponentsInChildren<ParticleSystem>();

        for (int i = 0; i < effects.Length; i++)
        {
            ParticleSystem effect = effects[i];
            ParticleSystem.ShapeModule shape = effect.shape;
            ParticleSystem.EmissionModule emission = effect.emission;
        }

        Renderer[] renders = this.GetComponentsInChildren<Renderer>();
        for (int i = 0; i < renders.Length; i++)
            renders[i].sortingLayerName = "Frente";

    }
}
