using UnityEngine;
using System.Collections;

public class ButtonController : MonoBehaviour
{
	private Button button;
	private bool inZone = false;
	private bool didToggleOnEnter = false;
	public string action = "default";
	public bool toggleable = true;
	public bool toggleOnEnter = false;
	public int clipNum = 0;
	public string target = "default";
	private float emissionPower = 0.0f;

	void Awake()
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
		if(button.LED)
		{
			if(button.toggled)
			{
				emissionPower = 1.0f;
			}
			else
			{
				emissionPower = 0.0f;
			}
			if(transform.parent.renderer)
			{
				if(emissionPower != transform.parent.renderer.material.GetFloat("_EmissionPower"))
					transform.parent.renderer.material.SetFloat("_EmissionPower", emissionPower);
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

	public void ToggleButton(bool status)
	{
		button.toggled = status;
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
