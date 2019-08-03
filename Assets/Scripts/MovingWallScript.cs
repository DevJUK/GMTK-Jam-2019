using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingWallScript : MonoBehaviour
{

	public float MoveSpeed;
	public List<int> Points;
	public bool MoveRight;


    void Update()
    {

		if (transform.localPosition.z > Points[1])
		{
			MoveRight = false;
		}
		else if (transform.localPosition.z < Points[0])
		{
			MoveRight = true;
		}

		if (MoveRight)
		{
			Debug.Log("For");
			GetComponent<Rigidbody>().velocity = Vector3.forward * MoveSpeed;
		}
		else
		{
			Debug.Log("Aga");
			GetComponent<Rigidbody>().velocity = -Vector3.forward * MoveSpeed;
		}
    }
}
