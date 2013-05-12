using UnityEngine;
using System.Collections;

public class Enemy : MonoBehaviour
{
	public Waypoint[] waypoints;

	private int _curState;
	private int _curWP;
	private float _speed;
	private float halfWidth;
	private int _health;

	enum States
	{
		Patrol,
		Dead
	}

	void Start()
	{
		curState = (int)States.Patrol;
		curWP = 0;
		speed = 5.0f;
		halfWidth = GetComponent<BoxCollider>().size.x / 2;
		health = 1;
	}
	
	void Update()
	{
		if(!GameController.PAUSED)
		{
			if(!IsAlive())
				curState = (int)States.Dead;

			switch(curState)
			{
				case (int)States.Patrol:
					for(int i = 0; i < waypoints.Length; i++)
					{
						if(i == curWP)
							Debug.DrawLine(transform.position, waypoints[curWP].transform.position, Color.green);
						else
							Debug.DrawLine(transform.position, waypoints[i].transform.position, Color.red);
					}
					Patrol();
				break;
				case (int)States.Dead:
					// Death code
					Kill();
				break;
				default:
				break;
			}
		}
	}

	private bool IsAlive()
	{
		return health > 0;
	}

	private void Patrol()
	{
		float buffer = 0.5f;
		if(transform.position.x + halfWidth < waypoints[curWP].transform.position.x)
		{
			rigidbody.MovePosition(rigidbody.position + new Vector3(speed * Time.deltaTime, 0, 0));
		}
		if(transform.position.x - halfWidth> waypoints[curWP].transform.position.x)
		{
			rigidbody.MovePosition(rigidbody.position + new Vector3(-speed * Time.deltaTime, 0, 0));
		}
		if((transform.position.x + halfWidth >= waypoints[curWP].transform.position.x - buffer && transform.position.x + halfWidth <= waypoints[curWP].transform.position.x + buffer) ||
		   (transform.position.x - halfWidth >= waypoints[curWP].transform.position.x - buffer && transform.position.x - halfWidth <= waypoints[curWP].transform.position.x + buffer))
		{
			curWP = (curWP >= waypoints.Length - 1) ? 0 : curWP + 1;
		}
	}

	public void DamageSelf(int damage)
	{
		health -= damage;
	}

	private void Kill()
	{
		Destroy(gameObject);
	}

	// Getters/Setters
	public int curState
	{
		get{return _curState;}
		set{_curState = value;}
	}

	public int curWP
	{
		get{return _curWP;}
		set{_curWP = value;}
	}

	public float speed
	{
		get{return _speed;}
		set{_speed = value;}
	}

	public int health
	{
		get{return _health;}
		set{_health = value;}
	}
}
