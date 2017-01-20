using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;
using System.Collections;

public class SetupParameters : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
        transform.FindChild("TxtLifes").GetComponent<InputField>().text = InitialParameters.Lifes.ToString();
        transform.FindChild("TxtFrecuency").GetComponent<InputField>().text = InitialParameters.Frecuency.ToString();
        transform.FindChild("TxtSpeed").GetComponent<InputField>().text = InitialParameters.Speed.ToString();
        transform.FindChild("TxtFrecuencyConstant").GetComponent<InputField>().text = InitialParameters.ConstantFrecuency.ToString();
        transform.FindChild("TxtInitialExp").GetComponent<InputField>().text = InitialParameters.Experience.ToString();
        transform.FindChild("TxtExpConstant").GetComponent<InputField>().text = InitialParameters.ConstantExperience.ToString();
        transform.FindChild("TxtLevelFrecuency").GetComponent<InputField>().text = InitialParameters.LevelFrecuencyFactor.ToString();
        transform.FindChild("TxtLevelSpeed").GetComponent<InputField>().text = InitialParameters.LevelSpeedFactor.ToString();
        transform.FindChild("TxtBonusExp").GetComponent<InputField>().text = InitialParameters.BonusExperience.ToString();

        transform.FindChild("TxtBonusExp").GetComponent<InputField>().text = InitialParameters.BonusExperience.ToString();
        transform.FindChild("TxtBonusDistance").GetComponent<InputField>().text = InitialParameters.BonusMoonDistance.ToString();
        transform.FindChild("TxtBonusAngle").GetComponent<InputField>().text = InitialParameters.BonusMoonAngle.ToString();
        transform.FindChild("TxtBonusAccel").GetComponent<InputField>().text = InitialParameters.BonusAcceleration.ToString();

        transform.FindChild("CmbControlMode").GetComponent<Dropdown>().value = InitialParameters.ControlMode == "Circles" ? 0 : 1;
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void OnPlay()
    {
        InitialParameters.Lifes = int.Parse(transform.FindChild("TxtLifes").GetComponent<InputField>().text);
        InitialParameters.Frecuency = float.Parse(transform.FindChild("TxtFrecuency").GetComponent<InputField>().text);
        InitialParameters.Speed = float.Parse(transform.FindChild("TxtSpeed").GetComponent<InputField>().text);
        InitialParameters.ConstantFrecuency = float.Parse(transform.FindChild("TxtFrecuencyConstant").GetComponent<InputField>().text);
        InitialParameters.Experience = float.Parse(transform.FindChild("TxtInitialExp").GetComponent<InputField>().text);
        InitialParameters.ConstantExperience = float.Parse(transform.FindChild("TxtExpConstant").GetComponent<InputField>().text);
        InitialParameters.LevelFrecuencyFactor = float.Parse(transform.FindChild("TxtLevelFrecuency").GetComponent<InputField>().text);
        InitialParameters.LevelSpeedFactor = float.Parse(transform.FindChild("TxtLevelSpeed").GetComponent<InputField>().text);

        InitialParameters.BonusExperience = float.Parse(transform.FindChild("TxtBonusExp").GetComponent<InputField>().text);
        InitialParameters.BonusMoonDistance = float.Parse(transform.FindChild("TxtBonusDistance").GetComponent<InputField>().text);
        InitialParameters.BonusMoonAngle = float.Parse(transform.FindChild("TxtBonusAngle").GetComponent<InputField>().text);
        InitialParameters.BonusAcceleration = float.Parse(transform.FindChild("TxtBonusAccel").GetComponent<InputField>().text);
       
        InitialParameters.ControlMode = transform.FindChild("CmbControlMode").GetComponent<Dropdown>().value == 0 ? "Circles" : "Sliders";

        SceneManager.LoadScene("TitleScreen", LoadSceneMode.Single);
    }
}
