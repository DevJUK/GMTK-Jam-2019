using System.Collections.Generic;
using UnityEngine;


/*
 * 
 *								Audio Manager Script
 *			
 *			Script written by: Jonathan Carter (https://jonathan.carter.games)
 *									Version: 1
 *							  Last Updated: 22/06/19
 * 
 * 
*/


public class AudioManager : MonoBehaviour
{

    public GameObject Sound_Prefab = null;       // Holds the prefab that plays the sound requested
	[HideInInspector]
	public List<string> Sound_Names = new List<string>();       // A list to hold the audioclip names
	[HideInInspector]
	public List<AudioClip> Sound_Clips = new List<AudioClip>();									// A list to hold the audioclips themselves
    public Dictionary<string, AudioClip> Sound_Lib = new Dictionary<string, AudioClip>();       // Dictionary that holds the audio names and clips

	
    private void Start()
    {
		if (Sound_Prefab == null)
		{
			Debug.LogWarning("(*Audio Manager*): Prefab has not been assigned! Please assign a prefab in the inspector before using the manager.");
		}

		GetComponent<AudioSource>().hideFlags = HideFlags.HideInInspector;

		for (int i = 0; i < Sound_Names.Count; i++)         // For loop that populates the dictionary with all the sound assets in the lists
        {
            Sound_Lib.Add(Sound_Names[i], Sound_Clips[i]);
        }
    }

	
	// Fuction to select and play a sound asset from the start with options
    public void PlayClip(string Request, float Volume = 1, float Pitch = 1)                   
    {
        if (Sound_Lib.ContainsKey(Request))												// If the sound is in the library
        {
            GameObject clip = Instantiate(Sound_Prefab);									// Instantiate a Sound prefab
            clip.GetComponent<AudioSource>().clip = Sound_Lib[Request];						// Get the prefab and add the requested clip to it

			clip.GetComponent<AudioSource>().volume = Volume;	// changes the volume if a it is inputted
			clip.GetComponent<AudioSource>().pitch = Pitch;      // changes the pitch if a change is inputted

			clip.GetComponent<AudioSource>().Play();										// play the audio from the prefab
			Destroy(clip, clip.GetComponent<AudioSource>().clip.length);					// Destroy the prefab once the clip has finished playing
        }
		else
		{
			Debug.LogWarning("(*Audio Manager*): Could not find clip. Please ensure the clip is scanned and the string you entered is correct (Note the input is CaSe SeNsItIvE).");
		}
    }


	// Function to select and play a sound asset from a selected time with options
	public void PlayClipFromTime(string Request, float Time, float Volume = 1, float Pitch = 1)
	{
		if (Sound_Lib.ContainsKey(Request))
		{
			GameObject clip = Instantiate(Sound_Prefab);
			clip.GetComponent<AudioSource>().clip = Sound_Lib[Request];
			clip.GetComponent<AudioSource>().time = Time;

			clip.GetComponent<AudioSource>().volume = Volume;
			clip.GetComponent<AudioSource>().pitch = Pitch;

			clip.GetComponent<AudioSource>().Play();
			Destroy(clip, clip.GetComponent<AudioSource>().clip.length);
		}
		else
		{
			Debug.LogWarning("(*Audio Manager*): Could not find clip. Please ensure the clip is scanned and the string you entered is correct (Note the input is CaSe SeNsItIvE).");
		}
	}


	// Function to select and play a sound asset with a delay and options
	public void PlayClipWithDelay(string Request, float Delay, float Volume = 1, float Pitch = 1)
    {
        if (Sound_Lib.ContainsKey(Request))
        {
            GameObject clip = Instantiate(Sound_Prefab);
            clip.GetComponent<AudioSource>().clip = Sound_Lib[Request];

			clip.GetComponent<AudioSource>().volume = Volume;
			clip.GetComponent<AudioSource>().pitch = Pitch;

			clip.GetComponent<AudioSource>().PlayDelayed(Delay);							// Only difference, played with a delay rather that right away
            Destroy(clip, clip.GetComponent<AudioSource>().clip.length);
        }
		else
		{
			Debug.LogWarning("(*Audio Manager*): Could not find clip. Please ensure the clip is scanned and the string you entered is correct (Note the input is CaSe SeNsItIvE).");
		}
    }

	// Used in the editor script, to update the library with a fresh input, don't call this, it doesn't play audio
	public void UpdateLibrary()
	{
		for (int i = 0; i < Sound_Names.Count; i++)         // For loop that populates the dictionary with all the sound assets in the lists
		{
			Sound_Lib.Clear();
			Sound_Lib.Add(Sound_Names[i], Sound_Clips[i]);
		}
	}
}