using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlayerScript : MonoBehaviour
{
	public float MoveSpeed;
	public bool PlayerDead;

	private bool IsCoRunning;
	private bool CanJump = true;

	public Animator PlayerAmin;
	private SceneController Scenes;

	public List<Material> PlayerMats;

    void Start()
    {
		Scenes = FindObjectOfType<SceneController>();
    }


    void Update()
    {
		if (!PlayerDead)
		{
			GetComponent<Rigidbody>().velocity = Vector3.forward * MoveSpeed;
		}


		// Jump player to next scene in sequence
		if ((Input.GetMouseButtonDown(0)) && (CanJump))
		{
			Scenes.NextScene();
			SwitchMat();
			PlayerAmin.SetTrigger("Jump");
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
	}


	private void SwitchMat()
	{
		if (GetComponentsInChildren<SkinnedMeshRenderer>()[1].material == PlayerMats[0])
		{
			GetComponentsInChildren<SkinnedMeshRenderer>()[1].material = PlayerMats[1];
		}
		else
		{
			//GetComponentsInChildren<SkinnedMeshRenderer>()[1].material = PlayerMats[0];
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
