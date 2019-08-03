using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretScript : MonoBehaviour
{

	public GameObject Barrel;
	public GameObject Player;


    // Start is called before the first frame update
    void Start()
    {
        
    }


    void Update()
    {
		// Follow Player
		Barrel.transform.LookAt(Player.transform);
    }


	private void OnTriggerEnter(Collider other)
	{
		if (other.gameObject.name == "Player")
		{

		}
	}
}
