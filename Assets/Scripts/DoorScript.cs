using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorScript : MonoBehaviour
{

	public float Delay;
	public bool DoorEnabled;
	public bool DoorIsUp;
	public bool IsCoRunning;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }


	private void DoorUp()
	{
		transform.position = new Vector3(transform.position.x, transform.position.y + 2, transform.position.z);
	}


	private void DoorDown()
	{
		transform.position = new Vector3(transform.position.x, transform.position.y - 2, transform.position.z);
	}


	private IEnumerator Sequence()
	{
		IsCoRunning = true;
		yield return new WaitForSeconds(Delay);
		IsCoRunning = false;
	}
}
