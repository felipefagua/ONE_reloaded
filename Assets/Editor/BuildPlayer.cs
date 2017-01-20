using UnityEngine;
using UnityEditor;
using UnityEditor.Callbacks;
using UnityEditor.iOS.Xcode;
using System.Collections;
using System.IO;

public class BuildPlayer : MonoBehaviour 
{
	private const string DISABLE_GOOGLE_PLAY_IOS = "NO_GPGS";
	private string destinationPath;

	[MenuItem("Madbricks/Build/Build iOS")]
	static void BuildForiOS() 
	{ 
		EditorBuildSettingsScene[] scenes = EditorBuildSettings.scenes;
		string[] scenesPath = new string[scenes.Length];
		for (int i = 0; i < scenesPath.Length; i++) 
			scenesPath[i] = scenes[i].path;

		BuildOptions buildOptions = BuildOptions.ShowBuiltPlayer;

		string destinationPath = EditorUtility.SaveFilePanel("Choose a destination","", EditorPrefs.GetString("BuildForiOS.Name",""), "");
		string previousPath = EditorPrefs.GetString ("BuildForiOS.PreviousPath", Application.persistentDataPath);

		int lastSlash = destinationPath.LastIndexOf("/");
		string path = destinationPath.Substring(0, lastSlash), name = destinationPath.Substring(lastSlash + 1);
		EditorPrefs.SetString("BuildForiOS.PreviousPath", path);
		EditorPrefs.SetString("BuildForiOS.Name", name);

		PlayerSettings.SetScriptingDefineSymbolsForGroup(BuildTargetGroup.iOS, DISABLE_GOOGLE_PLAY_IOS);

		BuildPipeline.BuildPlayer (scenesPath, destinationPath, BuildTarget.iOS, buildOptions);
	}

	[PostProcessBuildAttribute(100)]
	public static void OnPostProcessBuild(BuildTarget target, string pathToBuiltProject)
	{
		if (target != BuildTarget.iOS)
			return;

		string projPath = pathToBuiltProject + "/Unity-iPhone.xcodeproj/project.pbxproj";
		PBXProject proj = new PBXProject();
		proj.ReadFromString(File.ReadAllText(projPath));
		string targetGUID = proj.TargetGuidByName("Unity-iPhone");
		proj.AddBuildProperty(targetGUID, "OTHER_LDFLAGS", "-ObjC"); 
		File.WriteAllText(projPath, proj.WriteToString());
	}

}
