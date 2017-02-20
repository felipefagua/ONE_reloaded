using UnityEngine;
using System.Collections;

public class LocalDataController {

    private const string KEY_TUTORIAL = "tutorial";

    private static LocalDataController _instance;

    public static LocalDataController instance {
        get {
            if (_instance == null)
                _instance = new LocalDataController();
            return _instance;
        }        
    }

    public bool hasPassTutorial {
        get { return (PlayerPrefs.GetInt(KEY_TUTORIAL, 0) == 1); }
        set {
            int tutorial = 0;
            if (value)
                tutorial = 1;
            PlayerPrefs.SetInt(KEY_TUTORIAL, tutorial);
        }
    }

    private LocalDataController() {
        
    }

    private void CleanLocalData() {
        hasPassTutorial = false;
    }
}
