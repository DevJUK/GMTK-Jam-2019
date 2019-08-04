using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreakScript : MonoBehaviour
{
	public int Delay;
	private bool IsCoRunning;
	private PlayerScript Player;
	private SceneController Scenes;
	private AudioManager AM;

	private void Start()
	{
		Player = GetComponent<PlayerScript>();
		Scenes = FindObjectOfType<SceneController>();
		AM = FindObjectOfType<AudioManager>();

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
		yield return new WaitForSeconds(Delay);
		Scenes.NextScene();
		Player.SwitchMat();
		Player.PlayerAmin.SetTrigger("Jump");
		Player.TeleportParticles.Play();
		AM.PlayClip("Jump");
		AM.PlayClip("Teleport", Volume: 2f, Pitch: 2f);
		IsCoRunning = false;
	}

}
