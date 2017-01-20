using UnityEngine;
using System.Collections;

[ExecuteInEditMode]
public class Colorizer : MonoBehaviour {

	public Color TintColor;
	public bool UseInstanceWhenNotEditorMode = true;

	private Color oldColor;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if(oldColor != TintColor) ChangeColor(gameObject, TintColor);
		oldColor = TintColor;
	}

	void ChangeColor(GameObject effect, Color color)
	{	
		var rend = effect.GetComponentsInChildren<Renderer>();
		foreach (var r in rend) {

            Material mat = null;

			#if UNITY_EDITOR 
            mat = r.sharedMaterial;
			#else
            if(UseInstanceWhenNotEditorMode) {mat = r.material;}
            else {mat = r.sharedMaterial;}
			#endif
		
			if(mat==null || !mat.HasProperty("_TintColor")) continue;
            Color oldColor = mat.GetColor("_TintColor");
			color.a = oldColor.a;
			mat.SetColor("_TintColor", color);
		}
        Light light = effect.GetComponentInChildren<Light>();
		if(light!=null) light.color = color;
	}
}
