using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DistanceTracker : MonoBehaviour
{

	public Transform Player;
	public Transform EndPos;

	public float AmountComplete;
	public float Max;

	public Text Display;

	private void Start()
	{
		Max = EndPos.position.z;
		Player = FindObjectOfType<PlayerScript>().transform;
	}

	void Update()
    {
		AmountComplete = (EndPos.position.z - Camera.main.transform.position.z) / (Max / 100);
		AmountComplete = 100 - AmountComplete;
		SetAmount();
    }


	public void SetAmount()
	{
		Display.text = "Complete: " + Mathf.FloorToInt(AmountComplete);
	}
}
