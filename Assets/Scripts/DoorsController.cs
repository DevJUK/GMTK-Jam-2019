using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class DoorsController : MonoBehaviour
{
	public Animator Anim;
	public Text ZeText;

    // Start is called before the first frame update
    void Start()
    {
		Anim = GetComponent<Animator>();
		ZeText = GetComponentInChildren<Text>();
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

	public void SetText(string Input)
	{
		StartCoroutine(WaitToUpdate(Input));
	}

	public void Menu()
	{
		StartCoroutine(WaitToMenu("Given Up?"));
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
}
