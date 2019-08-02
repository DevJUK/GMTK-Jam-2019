using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneController : MonoBehaviour
{

	public int CurrentScene = 1;
	public int MaxScenes = 2;

	public GameObject Scene1;
	public GameObject Scene2;

	private bool IsCoRunning;

    void Update()
    {
        
    }


	public void NextScene()
	{
		if (CurrentScene == 1)
		{
			CurrentScene++;
			Scene1.SetActive(false);
			Scene2.SetActive(true);
		}
		else if (CurrentScene == MaxScenes)
		{
			CurrentScene = 1;
			Scene1.SetActive(true);
			Scene2.SetActive(false);
		}
	}
}
