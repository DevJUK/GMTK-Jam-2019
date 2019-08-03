using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreakScript : MonoBehaviour
{
	public int Delay;
	private bool IsCoRunning;
	private PlayerScript Player;
	private SceneController Scenes;


	private void Start()
	{
		Player = GetComponent<PlayerScript>();
		Scenes = FindObjectOfType<SceneController>();

		Player.CanJump = false;
		Player.CanGrav = true;
	}

	private void Update()
	{
		if (!IsCoRunning)
		{
			StartCoroutine(Check());
		}
	}


	private IEnumerator Check()
	{
		IsCoRunning = true;
		Scenes.NextScene();
		Player.SwitchMat();
		Player.PlayerAmin.SetTrigger("Jump");
		Player.TeleportParticles.Play();
		yield return new WaitForSeconds(Delay);
		IsCoRunning = false;
	}

}
