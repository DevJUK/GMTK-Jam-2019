using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class PlayerScript : MonoBehaviour
{
	public float MoveSpeed;
	public bool PlayerDead;
	public bool GravityFlipped = true;
	public bool GravChanged;
	public bool GameStarted = false;

	internal bool IsCoRunning;
	internal bool CanJump = true;
	public bool CanGrav = false;

	public ParticleSystem TeleportParticles;
	public ParticleSystem GravityParticles;

	public Animator PlayerAmin;
	private SceneController Scenes;

	public List<Material> PlayerMats;

	public List<GameObject> Body;

	private Tracking GM;
	private DoorsController DM;
	private AudioManager AM;

	void Start()
	{
		Scenes = FindObjectOfType<SceneController>();
		GM = FindObjectOfType<Tracking>();
		DM = FindObjectOfType<DoorsController>();
		AM = FindObjectOfType<AudioManager>();
		Physics.gravity = new Vector3(0, -9.8f, 0);
		transform.localScale = new Vector3(transform.localScale.x, transform.localScale.y, transform.localScale.z);
		StartCoroutine(DelayStart());
	}


	void Update()
	{
		if (GameStarted)
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
				AM.PlayClip("Jump");
				AM.PlayClip("Teleport", Volume: 2f, Pitch: 2f);

				if (!IsCoRunning)
				{
					StartCoroutine(Cooldown());
				}
			}
			else if ((Input.GetMouseButtonDown(0)) && (CanGrav))
			{
				Debug.Log("Flip");
				FlipGravity();
				GravityParticles.Play();
				AM.PlayClip("Pop");
				AM.PlayClip("Teleport", Volume: 2f, Pitch: 2f);
			}

			if ((CanGrav) && (!GravChanged))
			{
				Physics.gravity = Physics.gravity * 2;
				GravChanged = true;
			}
		}
		else if (Input.GetButtonDown("Space"))
		{
			GameStarted = true;

			if (!IsCoRunning)
			{
				StartCoroutine(DelayStart());
			}

			PlayerAmin.SetBool("GameStarted", true);
			DataUpdate();
		}
		else
		{
			Debug.Log("else");

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
			AM.PlayClip("WallHit");
			PlayerAmin.SetBool("HitWall", true);
			PlayerAmin.SetBool("Falling", false);
			PlayerDead = true;

			if (GetComponent<BreakScript>().enabled)
			{
				GetComponent<BreakScript>().StopAllCoroutines();
				GetComponent<BreakScript>().enabled = false;
			}

			for (int i = 0; i < GameObject.FindGameObjectsWithTag("Ex").Length; i++)
			{
				GameObject.FindGameObjectsWithTag("Ex")[i].SetActive(false);
			}

			DataUpdate();
			StartCoroutine(DelayDeath());

		}

		if (collision.gameObject.tag == "Ex")
		{
			PlayerAmin.SetBool("HitWall", true);
	
			for (int i = 0; i < Body.Count; i++)
			{
				Body[i].SetActive(false);
			}

			PlayerDead = true;

			if (GetComponent<BreakScript>().enabled)
			{
				GetComponent<BreakScript>().StopAllCoroutines();
				GetComponent<BreakScript>().enabled = false;
			}

			DataUpdate();
			StartCoroutine(DelayDeath());
		}
	}


	private void OnTriggerEnter(Collider other)
	{
		if (other.gameObject.tag == "End")
		{
			// End Level
			PlayerDead = true;
			PlayerAmin.SetBool("Falling", false);
			CanGrav = false;
			CanJump = false;
			GameObject.FindGameObjectWithTag("DriveText").GetComponent<Text>().text = "Drive: Destroyed";
			GameObject.FindGameObjectWithTag("DriveText").GetComponent<Text>().color = Color.red;
		}


		if (other.gameObject.tag == "Speed")
		{
			MoveSpeed += 2;

			if (!GetComponent<BreakScript>().enabled)
			{
				GetComponent<BreakScript>().StopAllCoroutines();
				GetComponent<BreakScript>().enabled = true;
			}

			GameObject.FindGameObjectWithTag("DriveText").GetComponent<Text>().text = "Drive: Gravity";
			GameObject.FindGameObjectWithTag("DriveText").GetComponent<Text>().color = Color.cyan;
		}

		if (other.gameObject.tag == "Fall")
		{
			MoveSpeed -= 1;
			if (!GetComponent<BreakScript>().enabled)
			{
				GetComponent<BreakScript>().StopAllCoroutines();
				GetComponent<BreakScript>().enabled = true;
			}
			Physics.gravity = Physics.gravity / 4;
			PlayerAmin.SetBool("Falling", true);
			CanGrav = false;
			CanJump = true;
			GameObject.FindGameObjectWithTag("DriveText").GetComponent<Text>().text = "Drive: Jump";
			GameObject.FindGameObjectWithTag("DriveText").GetComponent<Text>().color = Color.green;
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

	private IEnumerator DelayDeath()
	{
		yield return new WaitForSeconds(1.5f);
		DM.CloseDoors();
		DM.SetText();
	}

	private IEnumerator ReduceSpeed()
	{
		MoveSpeed -= .1f;
		yield return new WaitForSeconds(.25f);
	}


	private IEnumerator DelayStart()
	{
		IsCoRunning = true;
		float Speed = MoveSpeed;
		MoveSpeed = 0;
		yield return new WaitForSeconds(.75f);
		MoveSpeed = Speed;
		IsCoRunning = false;
	}


	private IEnumerator End()
	{
		// Sound
		AM.PlayClip("Yay");

		yield return new WaitForSeconds(2);
		// Particles


	}

	private void DataUpdate()
	{
		// New PB
		if (GM.PB < Scenes.gameObject.GetComponent<DistanceTracker>().GetDistance())
		{
			GM.PB = Scenes.gameObject.GetComponent<DistanceTracker>().GetDistance();
			GM.Last = Scenes.gameObject.GetComponent<DistanceTracker>().GetDistance();
			Debug.Log("NEW PB & LAST");
		}
		else if (GM.PB > Scenes.gameObject.GetComponent<DistanceTracker>().GetDistance())
		{
			GM.Last = Scenes.gameObject.GetComponent<DistanceTracker>().GetDistance();
			Debug.Log("NEW LAST ONLY");
		}

		GM.GetComponent<SaveScript>().SaveData();
	}


	public void StopGame()
	{
		PlayerAmin.SetBool("HitWall", true);
		PlayerAmin.SetBool("Falling", false);
		PlayerDead = true;
		if (GetComponent<BreakScript>().enabled)
		{
			GetComponent<BreakScript>().enabled = false;
		}
	}

	public void PlayFootStep()
	{
		AM.PlayClip("FootStep", Volume: .5f);
	}
}
