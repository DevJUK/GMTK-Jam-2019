using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class Tracking : MonoBehaviour
{

	public int PB;
	public int Last;


	private void Start()
	{
		if (SceneManager.GetActiveScene().name == "Menu")
		{
			GetComponent<SaveScript>().LoadData();
			GameObject.Find("BestDistance").GetComponent<Text>().text = "Furthest Traversed: " + PB + "%";
			GameObject.Find("LastDistance").GetComponent<Text>().text = "Last Attempt: " + Last + "%";
		}
	}
}
