using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// Universal Rotation Script


public class RotateScript : MonoBehaviour
{

	[Header("Choose an axis to rotate on")]
	public bool XAxis;		// Bool for the XAxis
	public bool YAxis;		// Bool for the YAxis
	public bool ZAxis;		// Bool for the ZAxis

	[Header("Speed of the rotation")]
	public float Speed;     // Float for the speed of the rotation

	[Header("Local Rotation?")]
	public bool LocalRot = false;


	// Update is called once per display frame
	void FixedUpdate ()
	{
		if (!LocalRot)
		{
			// Roates the object with whatever rotation selected at the desired speed (note there is not time.deltatime here so its small changes
			transform.Rotate(ConvertBool(XAxis) * Speed, ConvertBool(YAxis) * Speed, ConvertBool(ZAxis) * Speed);
		}
		else
		{
			transform.Rotate(ConvertBool(XAxis) * Speed, ConvertBool(YAxis) * Speed, ConvertBool(ZAxis) * Speed, Space.Self);
		}
	}


	// Function to convert boolean to int for use in the ratation
	int ConvertBool(bool input)
	{
		int convert;
		convert = input ? 1 : 0;
		return (convert);
	}
}
