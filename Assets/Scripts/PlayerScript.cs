using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlayerScript : MonoBehaviour
{
	public float MoveSpeed;
	public bool PlayerDead;
	public bool GravityFlipped;
	public bool GravChanged;

	internal bool IsCoRunning;
	internal bool CanJump = true;
	public bool CanGrav = false;

	public ParticleSystem TeleportParticles;

	public Animator PlayerAmin;
	private SceneController Scenes;

	public List<Material> PlayerMats;

	public List<GameObject> Body;

	private Tracking GM;
	private DoorsController DM;

	void Start()
	{
		Scenes = FindObjectOfType<SceneController>();
		GM = FindObjectOfType<Tracking>();
		DM = FindObjectOfType<DoorsController>();
		StartCoroutine(DelayStart());
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

		if ((CanGrav) && (!GravChanged))
		{
			Physics.gravity = Physics.gravity * 2;
			GravChanged = true;
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

	// Deaths
	private void OnCollisionEnter(Collision collision)
	{
		if (collision.gameObject.tag == "Death")
		{
			Debug.Log("Hit Wall");
			PlayerAmin.SetBool("HitWall", true);
			PlayerAmin.SetBool("Falling", false);
			PlayerDead = true;

			if (GetComponent<BreakScript>().enabled)
			{
				GetComponent<BreakScript>().enabled = false;
			}

			DataUpdate();

		}

		if (collision.gameObject.tag == "Ex")
		{
			for (int i = 0; i < Body.Count; i++)
			{
				Body[i].SetActive(false);
			}

			PlayerDead = true;

			if (GetComponent<BreakScript>().enabled)
			{
				GetComponent<BreakScript>().enabled = false;
			}

			DataUpdate();
		}
	}


	private void OnTriggerEnter(Collider other)
	{
		if (other.gameObject.tag == "End")
		{
			// End Level
			PlayerDead = true;
			PlayerAmin.SetBool("Falling", false);
		}


		if (other.gameObject.tag == "Speed")
		{
			MoveSpeed += 2;

			if (!GetComponent<BreakScript>().enabled)
			{
				GetComponent<BreakScript>().enabled = true;
			}
		}

		if (other.gameObject.tag == "Fall")
		{
			MoveSpeed -= 1;
			Physics.gravity = Physics.gravity / 4;
			PlayerAmin.SetBool("Falling", true);
			CanGrav = false;
			CanJump = true;
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


	private IEnumerator ReduceSpeed()
	{
		MoveSpeed -= .1f;
		yield return new WaitForSeconds(.25f);
	}


	private IEnumerator DelayStart()
	{
		float Speed = MoveSpeed;
		MoveSpeed = 0;
		yield return new WaitForSeconds(1.5f);
		MoveSpeed = Speed;
	}

	private void DataUpdate()
	{
		// New PB
		if (GM.PB < Scenes.gameObject.GetComponent<DistanceTracker>().GetDistance())
		{
			GM.PB = Scenes.gameObject.GetComponent<DistanceTracker>().GetDistance();
			GM.Last = Scenes.gameObject.GetComponent<DistanceTracker>().GetDistance();
		}
		else if (GM.PB > Scenes.gameObject.GetComponent<DistanceTracker>().GetDistance())
		{
			GM.Last = Scenes.gameObject.GetComponent<DistanceTracker>().GetDistance();
		}

		GM.GetComponent<SaveScript>().SaveData();
	}
}
