using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DistanceTracker : MonoBehaviour
{
	public Transform EndPos;

	public float AmountComplete;
	public float Max;

	public Text Display;

	private void Start()
	{
		Max = EndPos.position.z;
	}

	void Update()
    {
		AmountComplete = (EndPos.position.z - Camera.main.transform.position.z) / (Max / 100);
		//AmountComplete = AmountComplete + (EndPos.position.y - Camera.main.transform.position.y) / (Max / 100);
		AmountComplete = 100 - AmountComplete;
		if (AmountComplete < 0)
		{
			AmountComplete = 0;
		}
		if (AmountComplete > 100)
		{
			AmountComplete = 100;
		}
		SetAmount();
    }


	public void SetAmount()
	{
		Display.text = "Complete: " + Mathf.FloorToInt(AmountComplete) + "%";
	}

	public int GetDistance()
	{
		return Mathf.FloorToInt(AmountComplete);
	}
}
