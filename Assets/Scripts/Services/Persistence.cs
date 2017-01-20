using UnityEngine;
using System;
using System.Xml.Serialization;
using System.Collections;

public abstract class Persistence
{
    private static GameData m_Data;
    public static string SelectedGameMode;

    public static GameData Data
    {
        get
        {
            if (m_Data == null)
            {
                if (PlayerPrefs.HasKey("GameData"))
                {                    

                    string data = PlayerPrefs.GetString("GameData");
                    Debug.Log(data);
                    System.IO.StringReader str = new System.IO.StringReader(data);

                    XmlSerializer serializer = new XmlSerializer(typeof(GameData));
                    m_Data = serializer.Deserialize(str) as GameData;
                }
                else
                {
                    m_Data = new GameData();
                    m_Data.LastGame = new PlayData();
                    m_Data.TopGame = new PlayData();
                }
            }
 
            return m_Data;
        }
    }


    public static void SaveSingleGame(int score, float exp, float time)
    {
        Data.LastGame = new PlayData(){ Score = score, Experience = exp, Time = time };  

        if (Data.TopGame == null || Data.LastGame.Score > Data.TopGame.Score)
        {
            Data.LastGame.IsHighScore = true;
            Data.TopGame = Data.LastGame;
        }
    }

    public static void Reset()
    {
        PlayerPrefs.DeleteAll();
    }

    public static void Save()
    {
        System.IO.StringWriter str = new System.IO.StringWriter();

        XmlSerializer serializer = new XmlSerializer(typeof(GameData));
        serializer.Serialize(str, Data);

        Debug.Log(str.ToString());

        PlayerPrefs.SetString("GameData", str.ToString());
        PlayerPrefs.Save();
    }
}

[Serializable]
public class GameData
{
    [SerializeField]
    public PlayData LastGame;
    [SerializeField]
    public PlayData TopGame;
}

[Serializable]
public class PlayData
{
    [SerializeField]
    public int Score;
    [SerializeField]
    public float Experience;
    [SerializeField]
    public float Time;
    [SerializeField]
    public bool IsHighScore;
}
