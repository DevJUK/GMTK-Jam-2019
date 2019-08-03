using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletScript : MonoBehaviour
{
	public GameObject Player;

	private void Awake()
	{
		Player = FindObjectOfType<PlayerScript>().gameObject;
	}

	void Update()
    {
		Vector3 Vec = (Player.transform.position - transform.position).normalized;
		GetComponent<Rigidbody>().MovePosition(transform.position + Vec * 4 * Time.deltaTime);
    }


	private void OnCollisionEnter(Collision collision)
	{
		GetComponentInChildren<ParticleSystem>().Play();
		Destroy(this, 1);
	}
}
