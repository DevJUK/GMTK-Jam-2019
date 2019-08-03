using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretScript : MonoBehaviour
{

	public GameObject Player;

	public bool InRange;
	public float Delay;
	public bool IsCoRunning;

	public GameObject TorpPrefab;
	private SceneController Scenes;


	private void Start()
	{
		Scenes = FindObjectOfType<SceneController>();
	}


	void Update()
    {
		// Player In Range
		if ((InRange) && (!IsCoRunning))
		{
			// Shoot Player
			StartCoroutine(Shoot());
		}
    }



	private IEnumerator Shoot()
	{
		IsCoRunning = true;
		GameObject Go = Instantiate(TorpPrefab, transform.position, transform.rotation);
		Go.transform.SetParent(GameObject.FindGameObjectWithTag("Scene" + Scenes.GetCurrentScene()).transform);
		yield return new WaitForSeconds(Delay);
		IsCoRunning = false;
	}


	private void OnTriggerEnter(Collider other)
	{
		if (other.gameObject.name == "Player")
		{
			InRange = true;
		}
	}


	private void OnTriggerExit(Collider other)
	{
		if (InRange)
		{
			InRange = false;
		}
	}
}
