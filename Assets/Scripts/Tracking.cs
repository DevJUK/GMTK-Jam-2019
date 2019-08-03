using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class Tracking : MonoBehaviour
{

	public int PB;
	public int Last;


	private void Awake()
	{
		DontDestroyOnLoad(this);
	}

	private void Start()
	{
		GetComponent<SaveScript>().LoadData();

		if (SceneManager.GetActiveScene().name == "Menu")
		{
			GameObject.Find("BestDistance").GetComponent<Text>().text = "Furthest Traversed: " + PB + "%";
			GameObject.Find("LastDistance").GetComponent<Text>().text = "Last Attempt: " + Last + "%";
		}
	}
}
