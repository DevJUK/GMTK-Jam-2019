using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlayerScript : MonoBehaviour
{
	public float MoveSpeed;
	public bool PlayerDead;

	private bool IsCoRunning;
	private bool CanJump = true;

	public ParticleSystem TeleportParticles;

	public Animator PlayerAmin;
	private SceneController Scenes;

	public List<Material> PlayerMats;

	public GameObject Body;

    void Start()
    {
		Scenes = FindObjectOfType<SceneController>();
    }


    void Update()
    {
		if (!PlayerDead)
		{
			GetComponent<Rigidbody>().velocity = new Vector3(0, 0, MoveSpeed);
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
    }


	private void OnCollisionEnter(Collision collision)
	{
		if (collision.gameObject.tag == "Death")
		{
			Debug.Log("Hit Wall");
			PlayerAmin.SetBool("HitWall", true);
		}

		if (collision.gameObject.tag == "Ex")
		{
			Body.SetActive(false);
		}
	}


	private void OnTriggerEnter(Collider other)
	{
		if (other.gameObject.tag == "End")
		{
			// End Level
			PlayerDead = true;
		}
	}


	private void SwitchMat()
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

	private IEnumerator Cooldown()
	{
		IsCoRunning = true;
		CanJump = false;
		yield return new WaitForSeconds(1);
		CanJump = true;
		IsCoRunning = false;

	}
}
