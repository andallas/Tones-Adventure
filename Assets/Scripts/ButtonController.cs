using UnityEngine;
using System.Collections;

public class ButtonController : MonoBehaviour
{
	private Button button;
	private bool inZone;
	private bool didToggleOnEnter;
	public string action;
	public bool toggleable;
	public bool toggleOnEnter;
	public int clipNum;
	public string target;

	void Start()
	{
		button = new Button(toggleable, action, target, clipNum);
		inZone = false;
		didToggleOnEnter = false;
	}
	
	void Update()
	{
		if(inZone){
			if(toggleOnEnter){
				if(didToggleOnEnter != inZone){
					didToggleOnEnter = inZone;
					button.toggled = !button.toggled;
				}
			} else {
				if(Input.GetButtonDown("Action")){
					button.toggled = !button.toggled;
				}
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
			didToggleOnEnter = false;
		}
	}

	public int ButtonStatus()
	{
		return button.objStatus;
	}

	public void ButtonStatus(int status)
	{
		button.objStatus = status;
	}

	public void ActivateTarget()
	{
		button.ActivateTarget();
	}
}
