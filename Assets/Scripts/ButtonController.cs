using UnityEngine;
using System.Collections;

public class ButtonController : MonoBehaviour
{
	private Button button;
	private bool inZone;
	public string action;
	public bool toggleable;

	void Start()
	{
		button = new Button(toggleable, action);
		inZone = false;
	}
	
	void Update()
	{
		if(inZone)
		{
			if(Input.GetButtonDown("Action"))
			{
				button.toggled = !button.toggled;
			}
		}
		button.Update();
	}

	void OnTriggerEnter(Collider collider)
	{
		if(collider.transform.gameObject.name == "Player")
		{
			inZone = true;
		}
	}

	void OnTriggerExit(Collider collider)
	{
		if(collider.transform.gameObject.name == "Player")
		{
			inZone = false;
		}
	}
}
