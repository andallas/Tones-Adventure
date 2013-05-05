using UnityEngine;
using System.Collections;

public class Button
{
	private bool _canToggle;
	private bool _toggled = false;
	private string _action;
	private int _objStatus = 0;
	private string target;
	private int soundClipNum;

	public Button()
	{
		canToggle = false;
		action = "default";
	}

	public Button(bool toggleAble, string act, string trgt, int clipNum)
	{
		canToggle = toggleAble;
		action = act;
		target = trgt;
		soundClipNum = clipNum;
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
		Player player = (Player)GameObject.Find("Player").GetComponent(typeof(Player));
		switch(action)
		{

			case "default":
				Debug.Log("This action is called: " + action);
			break;

			case "reset_player":

				player.Reset();
			break;

			case "move_verticle_platorm":
				SlidingPlatform sp = (SlidingPlatform)GameObject.Find(target).GetComponent(typeof(SlidingPlatform));
				sp.ToggleActive();
			break;

			case "puzzle_one":
				_objStatus = 1;
			break;

			default:
				Debug.LogError("No action specified");
			break;
		}
		player.PlayAudio(soundClipNum);
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

	public int objStatus
	{
		get{return _objStatus;}
		set{_objStatus = value;}
	}
}
