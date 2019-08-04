using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class DoorsController : MonoBehaviour
{
	public Animator Anim;
	public Text ZeText;

	private AudioManager Am;
	public List<string> MenuText;
	public List<string> RestartText;

	private PlayerScript Player;

    // Start is called before the first frame update
    void Start()
    {
		Am = FindObjectOfType<AudioManager>();
		Anim = GetComponent<Animator>();
		ZeText = GetComponentInChildren<Text>();
		if (FindObjectOfType<PlayerScript>())
		{
			Player = FindObjectOfType<PlayerScript>();
		}
		OpenDoors();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.E))
		{
			OpenDoors();
		}

		if (Input.GetKey(KeyCode.Q))
		{
			CloseDoors();
		}
    }


	public void OpenDoors()
	{
		Anim.SetBool("OpenLevel", true);
		Anim.SetBool("CloseLevel", false);
	}

	public void CloseDoors()
	{
		Anim.SetBool("OpenLevel", false);
		Anim.SetBool("CloseLevel", true);
	}

	public void SetText()
	{
		StartCoroutine(WaitToUpdate(RestartText[Random.Range(0, MenuText.Count)]));
	}

	public void SetText(string Input)
	{
		StartCoroutine(WaitToUpdate(Input));
	}

	public void Menu()
	{
		StartCoroutine(WaitToMenu(MenuText[Random.Range(0, RestartText.Count)]));
	}

	public void Win()
	{
		Player.DataUpdate();
		StartCoroutine(WaitToMenu("Congrats!"));
	}

	private IEnumerator WaitToUpdate(string Input)
	{
		yield return new WaitForSeconds(1.25f);
		ZeText.text = Input;
		yield return new WaitForSeconds(1f);
		SceneManager.LoadSceneAsync("Level1");
	}


	private IEnumerator WaitToMenu(string Input)
	{
		yield return new WaitForSeconds(1.25f);
		ZeText.text = Input;
		yield return new WaitForSeconds(1f);
		SceneManager.LoadSceneAsync("Menu");
	}

	public void PlayClick()
	{
		Am.PlayClip("Click");
	}
}
