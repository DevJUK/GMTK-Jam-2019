using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletScript : MonoBehaviour
{
	public GameObject Player;
	public bool Hit;

	private void Awake()
	{
		Player = FindObjectOfType<PlayerScript>().gameObject;
	}

	void Update()
    {
		if (!Player.GetComponent<PlayerScript>().PlayerDead)
		{
			if (!Hit)
			{
				Vector3 Vec = (Player.transform.position - transform.position).normalized;
				GetComponent<Rigidbody>().MovePosition(transform.position + Vec * 4 * Time.deltaTime);
			}
		}
    }


	private void OnCollisionEnter(Collision collision)
	{
		Hit = true;
		GetComponent<MeshRenderer>().enabled = false;
		GetComponent<Light>().enabled = false;
		GetComponentInChildren<ParticleSystem>().Play();
		Destroy(gameObject, 1f);
	}
}
