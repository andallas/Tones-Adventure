using UnityEngine;
using System.Collections;

public class ButtonController : MonoBehaviour
{
	private Button button;
	private bool inZone;

	void Start()
	{
		button = new Button();
		inZone = false;
	}
	
	void Update()
	{
		if(inZone)
		{
			if(Input.GetButtonDown("Action"))
			{
				button.Action();
			}
		}
	}

	void OnTriggerEnter(Collider collider)
	{
		if(collider.transform.gameObject.name == "Player")
		{
			inZone = true;
		}
		else
		{
			inZone = false;
		}
	}
}
