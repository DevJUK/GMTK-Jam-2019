using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;

/*
 * 
 *							Audio Manager Editor Script
 *			
 *			Script written by: Jonathan Carter (https://jonathan.carter.games)
 *									Version: 1
 *							  Last Updated: 22/06/19						
 * 
 * 
*/

[CustomEditor(typeof(AudioManager))]
public class AudioManagerEditor : Editor
{
	// Colours for the Editor Buttons
	private Color32 ScanCol = new Color32(41, 176, 97, 255);
	private Color32 ScannedCol = new Color32(189, 191, 60, 255);
	private Color32 RedCol = new Color32(190, 42, 42, 255);
	private Color32 CyanCol = new Color32(100, 215, 175, 255);

	private bool SortedAudio;			// Bool for if the audio has been sorted
	private bool HasScannedOnce;		// Bool for if the scan button has been pressed before
	private string ScanButtonString;	// String for the value of the scan button text

	private List<AudioClip> AudioList;	// List of Audioclips used to add the audio to the library in the Audio Manager Script
	private List<string> AudioStrings;	// List of Strings used to add the names of the audio clips to the library in the Audio Manager Script

	private AudioManager Script;        // Reference to the Audio Manager Script that this script overrides the inspector for


	private GameObject InputtedPrefab;
	private bool ChangePrefab;
	private string ChangePrefabString;

	// When the script first enables - do this stuff!
	private void OnEnable()
	{
		// References the Audio Manager Script
		Script = target as AudioManager;		

		// Adds an Audio Source to the gameobject this script is on if its not already there (used for previewing audio only) 
		// * Hide flags hides it from the inspector so you don't notice it there *
		if (Script.gameObject.GetComponent<AudioSource>())
		{
			Script.gameObject.GetComponent<AudioSource>().hideFlags = HideFlags.HideInInspector;
			Script.GetComponent<AudioSource>().playOnAwake = false;
		}
		else
		{
			Script.gameObject.AddComponent<AudioSource>();
			Script.gameObject.GetComponent<AudioSource>().hideFlags = HideFlags.HideInInspector;
			Script.GetComponent<AudioSource>().playOnAwake = false;
		}
	}


	// Overrides the Inspector of the Audio Manager Script with this stuff...
	public override void OnInspectorGUI()
	{
		// The Path to the audio folder in your project
		string Dir = Application.dataPath + "/audio";

		// Makes the audio directoy if it doesn't exist in your project
		// * This will not create a new folder if you already have an audio folder *
		if (!Directory.Exists(Application.dataPath + "/audio"))
		{
			AssetDatabase.CreateFolder("Assets", "Audio");
		}

		// Checks to see if the Audio Manager Library is not empty
		// * If its not empty then you've pressed scan before, therefore it won't show the scan button again *
		if (Script.Sound_Clips.Count != 0)
		{
			HasScannedOnce = true;
		}

		// Changes the text & colour of the first button based on if you've pressed it before or not
		if (HasScannedOnce) { ScanButtonString = "Re-Scan"; GUI.color = ScannedCol; }
		else { ScanButtonString = "Scan"; GUI.color = ScanCol; }


		// Begins a grouping for the buttons to stay on one line
		EditorGUILayout.BeginHorizontal();


		// The actual Scan button - Runs functions to get the audio from the directory and add it to the library on the Audio Manager Script
		if (GUILayout.Button(ScanButtonString))
		{
			// Init Lists
			AudioList = new List<AudioClip>();
			AudioStrings = new List<string>();

			// Auto filling the lists 
			AddAudioClips();
			AddStrings();

			// Updates the lists 
			Script.Sound_Clips = AudioList;
			Script.Sound_Names = AudioStrings;
			Script.UpdateLibrary();
		}

		// Resets the color of the GUI
		GUI.color = Color.white;


		// The actual Clear button - This just clear the Lists and Library in the Audio Manager Script and resets the Has Scanned bool for the Scan button reverts to green and "Scan"
		if (GUILayout.Button("Clear"))
		{
			Script.Sound_Lib.Clear();
			Script.Sound_Clips.Clear();
			Script.Sound_Names.Clear();
			HasScannedOnce = false;
		}


		if (InputtedPrefab == null) { ChangePrefabString = "Assign Prefab"; GUI.color = ScanCol; }
		else if (InputtedPrefab) { ChangePrefabString = "Change Prefab"; GUI.color = CyanCol; }


		if (GUILayout.Button(ChangePrefabString))
		{
			ChangePrefab = !ChangePrefab;
		}

		GUI.color = Color.white;

		// Ends the grouping for the buttons
		EditorGUILayout.EndHorizontal();


		if (ChangePrefab)
		{
			if (InputtedPrefab == null)
			{
				EditorGUILayout.HelpBox("Please assign a prefab that will play the audio. The prefab needs to be an empty gameobject with only an audiosoruce component on it.", MessageType.Info);
				InputtedPrefab = (GameObject)EditorGUILayout.ObjectField("Prefab: ", InputtedPrefab, typeof (GameObject), false);
				Script.Sound_Prefab = InputtedPrefab;
			}
			else
			{
				EditorGUILayout.HelpBox("Prefab has been assigned, you may change it below if you need to.", MessageType.Info);
				InputtedPrefab = (GameObject)EditorGUILayout.ObjectField("Prefab: ", InputtedPrefab, typeof(GameObject), false);
				Script.Sound_Prefab = InputtedPrefab;
			}
		}


		// *** Labels ***
		EditorGUILayout.HelpBox("Items Scanned: " + Script.Sound_Clips.Count + "\n" + "Items in Directory: " + CheckNumberOfFiles(), MessageType.None);

		if (HasScannedOnce)
		{
			DisplayNames();
		}

		Repaint();

		// Shows anything that would normally be on the inspector - unless it has the Hide From Inspector
		// * If uncommented this will show the normal inspector for the script as the custom editor, not recommended and shouldn't be needed *
		//base.OnInspectorGUI();
	}


	// Adds all the audio clips to the list
	private void AddAudioClips()
	{
		// Makes a new lsit the size of the amount of objects in the path
		List<string> AllFiles = new List<string>(Directory.GetFiles(Application.dataPath + "/audio"));

		// Checks to see if there is anything in the path, if its empty it will not run the rest of the code and instead put a message in the console
		if (AllFiles.Count > 0)
		{
			HasScannedOnce = true;  // Sets the has scanned once to true so the scan button turns into the re-scan button

			AudioClip Source = null;

			foreach (string Thingy in AllFiles)
			{
				string Path = "Assets" + Thingy.Replace(Application.dataPath, "").Replace('\\', '/');

				if (AssetDatabase.LoadAssetAtPath(Path, typeof(AudioClip)))
				{
					Source = (AudioClip)AssetDatabase.LoadAssetAtPath(Path, typeof(AudioClip));
					AudioList.Add(Source);
				}
			}
		}
		else
		{
			// !Warning Message - shown in the console should there be no audio in the directory been scanned
			Debug.LogWarning("(*Audio Manager*): Please ensure there are Audio files in the directory: " + Application.dataPath + "/Audio");
		}
	}


	// Adds all the strings for the clip names to its list
	private void AddStrings()
	{
		for (int i = 0; i < AudioList.Count; i++)
		{
			if (AudioList[i] == null)
			{
				AudioList.Remove(AudioList[i]);
			}
		}

		int Ignored = 0;

		for (int i = 0; i < AudioList.Count; i++)
		{
			Debug.Log(AudioList[i].ToString());

			if (AudioList[i].ToString().Contains("(UnityEngine.AudioClip)"))
			{
				AudioStrings.Add(AudioList[i].ToString().Replace(" (UnityEngine.AudioClip)", ""));
			}
			else
			{
				Ignored++;
			}
		}

		if (Ignored > 0)
		{
			// This message should never show up, but its here just incase
			Debug.LogAssertion("(*Audio Manager*): " + Ignored + " entries ignored, this is due to the files either been in sub directories or other files that are not Unity AudioClips.");
		}
	}


	// Returns the number of files in the audio directory
	private int CheckNumberOfFiles()
	{
		int FinalCount;
		List<AudioClip> ClipCount = new List<AudioClip>();
		List<string> AllFiles = new List<string>(Directory.GetFiles(Application.dataPath + "/audio"));

		foreach (string Thingy in AllFiles)
		{
			string Path = "Assets" + Thingy.Replace(Application.dataPath, "").Replace('\\', '/');

			AudioClip Source = (AudioClip)AssetDatabase.LoadAssetAtPath(Path, typeof(AudioClip));

			ClipCount.Add(Source);
		}

		FinalCount = ClipCount.Count;

		// divides the final result by 2 as it includes the .meta files in this count which we don't use
		return FinalCount / 2;
	}


	// Displayes all the audio clips with the name and a button to preview said clips
	private void DisplayNames()
	{
		// Used as a placeholder for the clip name each loop
		string Elements = "";

		// Going through all the audio clips and making an element in the Inspector for them
		for (int i = 0; i < Script.Sound_Clips.Count; i++)
		{
			Elements = Script.Sound_Names[i];

			// Starts the ordering
			EditorGUILayout.BeginHorizontal();

			// Adds the text for the clip 
			EditorGUILayout.TextArea(Elements, GUILayout.MaxWidth(200));

			// Changes the GUI colour to green for the buttons
			GUI.color = ScanCol;

			// If there are no clips playing it will show "preview clip" buttons for all elements
			if (!Script.GetComponent<AudioSource>().isPlaying)
			{
				if (GUILayout.Button("Preview Clip"))
				{
					Script.GetComponent<AudioSource>().clip = Script.Sound_Clips[i];
					Script.GetComponent<AudioSource>().PlayOneShot(Script.GetComponent<AudioSource>().clip);
				}
			}
			// if a clip is playing, the clip that is playing will have a "stop clip" button instead of "preview clip" 
			else if (Script.GetComponent<AudioSource>().clip == Script.Sound_Clips[i])
			{
				GUI.color = RedCol;

				if (GUILayout.Button("Stop Clip"))
				{
					Script.GetComponent<AudioSource>().Stop();
				}
			}
			// This just ensures the rest of the elements keep a button next to them
			else
			{
				if (GUILayout.Button("Preview Clip"))
				{
					Script.GetComponent<AudioSource>().clip = Script.Sound_Clips[i];
					Script.GetComponent<AudioSource>().PlayOneShot(Script.GetComponent<AudioSource>().clip);
				}
			}

			// Resets the GUI colour
			GUI.color = Color.white;

			// Ends the GUI ordering
			EditorGUILayout.EndHorizontal();
		}
	}
}
