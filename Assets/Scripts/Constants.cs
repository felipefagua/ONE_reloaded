using UnityEngine;
using System.IO;
using System.Collections;

public class Constants {
    public const float AFINITY_LORD = 0.98f;
    public const float AFINITY_EPIC = 0.95f;
    public const float AFINITY_AWESOME = 0.90f;
    public const float AFINITY_GREAT = 0.80f;
    public const float AFINITY_GOOD = 0.70f;
    public const float MAX_LEVELFRECUENCY = 1.0f;
    public const float MAX_LEVELSPEED = 10.0f;
    public const int MAX_LEVELS = 15;
}


public static class InitialParameters
{
    public static int Lifes = 3; // 3;
    public static float Frecuency = 5.0f;//2.9f;
    public static float ConstantFrecuency = 0.02f;
    public static float Speed = 1.0f; //1.8f;
    public static float Experience = 7;
    public static float ConstantExperience = 2; //2;
    public static float LevelFrecuencyFactor = 0.7f; //7;
    public static float LevelSpeedFactor = 0.14f; //7;
    public static float BonusExperience = 10; // 15;
    public static float BonusMoonDistance = 0.5f;
    public static float BonusMoonAngle = 15.0f;
    public static float BonusAcceleration = 0.01f;
    public static string ControlMode = "Circles";
}

public enum EAfinityType
{
    LORD,
    EPIC,
    AWESOME,
    GREAT,
    GOOD,
    BAD
}





  