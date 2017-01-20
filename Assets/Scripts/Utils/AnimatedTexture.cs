using UnityEngine;
using System.Collections;

public class AnimatedTexture : MonoBehaviour {
    public Vector2 Direction;
    public float Speed;

    float offsetVal;

    void Start()
    {
    }

    void Update()
    {
        offsetVal += Time.deltaTime *Speed;
        this.GetComponent<Renderer>().material.SetTextureOffset("_MainTex", Direction * offsetVal);
    }
}
