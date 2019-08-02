using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlayerScript : MonoBehaviour
{
	public float MoveSpeed;
	public bool PlayerDead;

	public Animator PlayerAmin;

	public int CurrentScene;

    void Start()
    {
        
    }


    void Update()
    {
		if (!PlayerDead)
		{
			GetComponent<Rigidbody>().velocity = Vector3.forward * MoveSpeed;
		}


		// Jump player to next scene in sequence
		if (Input.GetMouseButtonDown(0))
		{
			if (CurrentScene == 1)
			{
				CurrentScene++;
			}
			else if (CurrentScene == 2)
			{
				CurrentScene = 1;
			}

			PlayerAmin.SetTrigger("Jump");
		}
    }
}
