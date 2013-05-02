using UnityEngine;
using System.Collections;

public class Button
{
	private bool _canToggle;
	private bool _toggled = false;
	private string _action;

	public Button()
	{
		canToggle = false;
		action = "default";
	}

	public Button(bool toggleAble, string act)
	{
		canToggle = toggleAble;
		action = act;
	}

	public void Update()
	{
		if(toggled)
		{
			Action();
		}
		if(!canToggle)
		{
			toggled = false;
		}
	}

	public void Action()
	{
		switch(action)
		{
			case "default":
				Debug.Log("This action is called: " + action);
			break;
			default:
				Debug.LogError("No action specified");
			break;
		}
	}

	// Setters & Getters
	public bool canToggle
	{
		get{return _canToggle;}
		set{_canToggle = value;}
	}

	public bool toggled
	{
		get{return _toggled;}
		set{_toggled = value;}
	}

	public string action
	{
		get{return _action;}
		set{_action = value;}
	}
}
