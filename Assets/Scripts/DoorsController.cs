using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorsController : MonoBehaviour
{
	public Animator Anim;

    // Start is called before the first frame update
    void Start()
    {
		Anim = GetComponent<Animator>();
		OpenDoors();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.E))
		{
			OpenDoors();
		}

		if (Input.GetKey(KeyCode.Q))
		{
			CloseDoors();
		}
    }


	public void OpenDoors()
	{
		Anim.SetBool("OpenLevel", true);
		Anim.SetBool("CloseLevel", false);
	}

	public void CloseDoors()
	{
		Anim.SetBool("OpenLevel", false);
		Anim.SetBool("CloseLevel", true);
	}
}
