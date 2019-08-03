﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlayerScript : MonoBehaviour
{
	public float MoveSpeed;
	public bool PlayerDead;
	public bool GravityFlipped;

	internal bool IsCoRunning;
	internal bool CanJump = true;
	public bool CanGrav = false;

	public ParticleSystem TeleportParticles;

	public Animator PlayerAmin;
	private SceneController Scenes;

	public List<Material> PlayerMats;

	public List<GameObject> Body;

    void Start()
    {
		Scenes = FindObjectOfType<SceneController>();
    }


    void Update()
    {
		if (!PlayerDead)
		{
			Vector3 Dir = new Vector3(0, 0, MoveSpeed);
			Dir.y = GetComponent<Rigidbody>().velocity.y;
			GetComponent<Rigidbody>().velocity = Dir;

		}


		// Jump player to next scene in sequence
		if ((Input.GetMouseButtonDown(0)) && (CanJump))
		{
			Scenes.NextScene();
			SwitchMat();
			PlayerAmin.SetTrigger("Jump");
			TeleportParticles.Play();

			if (!IsCoRunning)
			{
				StartCoroutine(Cooldown());
			}
		}
		else if ((Input.GetMouseButtonDown(0)) && (CanGrav))
		{
			Debug.Log("Flip");
			FlipGravity();
			TeleportParticles.Play();
		}
    }


	internal void FlipGravity()
	{
		if (!GravityFlipped)
		{
			Physics.gravity *= -1;
			Debug.Log(Physics.gravity);
			transform.localScale = new Vector3(transform.localScale.x, -transform.localScale.y, transform.localScale.z);
		}
		else if (GravityFlipped)
		{
			Physics.gravity *= 1;
			transform.localScale = new Vector3(transform.localScale.x, transform.localScale.y, transform.localScale.z);
		}
	}


	private void OnCollisionEnter(Collision collision)
	{
		if (collision.gameObject.tag == "Death")
		{
			Debug.Log("Hit Wall");
			PlayerAmin.SetBool("HitWall", true);
			PlayerDead = true;

			if (GetComponent<BreakScript>().enabled)
			{
				GetComponent<BreakScript>().enabled = false;
			}
		}

		if (collision.gameObject.tag == "Ex")
		{
			for (int i = 0; i < Body.Count; i++)
			{
				Body[i].SetActive(false);
			}

			PlayerDead = true;
		}
	}


	private void OnTriggerEnter(Collider other)
	{
		if (other.gameObject.tag == "End")
		{
			// End Level
			PlayerDead = true;
		}


		if (other.gameObject.tag == "Speed")
		{
			MoveSpeed += 2;

			if (!GetComponent<BreakScript>().enabled)
			{
				GetComponent<BreakScript>().enabled = true;
			}
		}
	}


	internal void SwitchMat()
	{
		Debug.Log(GetComponentsInChildren<SkinnedMeshRenderer>()[1].gameObject.name);
		Debug.Log(GetComponentsInChildren<SkinnedMeshRenderer>()[1].material.name);
		Debug.Log(PlayerMats[0].name);

		if (GetComponentsInChildren<SkinnedMeshRenderer>()[1].material.name == PlayerMats[0].name + " (Instance)")
		{
			GetComponentsInChildren<SkinnedMeshRenderer>()[1].material = PlayerMats[1];
		}
		else if (GetComponentsInChildren<SkinnedMeshRenderer>()[1].material.name == PlayerMats[1].name + " (Instance)")
		{
			GetComponentsInChildren<SkinnedMeshRenderer>()[1].material = PlayerMats[0];
		}
	}

	public IEnumerator Cooldown()
	{
		IsCoRunning = true;
		CanJump = false;
		yield return new WaitForSeconds(1);
		CanJump = true;
		IsCoRunning = false;

	}
}