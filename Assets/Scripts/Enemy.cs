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

	private int col = 8;
	private int row = 8;
	private int rowNum = 0;
	private int colNum = 0;
	private int total = 4;
	private int animSpeed = 10;
	private float illumFade = 0.0f;
	private bool rising = true;

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
					Patrol();
				break;
				case (int)States.Dead:
					// Death code
					Kill();
				break;
				default:
				break;
			}

			if(illumFade >= 1.0f)
			{
				rising = false;
			}
			else if(illumFade <= 0.0f)
			{
				rising = true;
			}

			illumFade = (rising) ? illumFade + Time.deltaTime : illumFade - Time.deltaTime;
			SetSpriteAnimation(col, row, colNum, rowNum, total, animSpeed, illumFade);
		}
	}

	void SetSpriteAnimation(int colCount, int rowCount, int colNumber, int rowNumber, int totalCells, int _animSpeed, float _illumFade)
	{
	    int index  = (int)(Time.time * _animSpeed);
	    index = index % totalCells;
	 
	    float sizeX = 1.0f / colCount;
	    float sizeY = 1.0f / rowCount;
	    Vector2 size =  new Vector2(sizeX,sizeY);
	 
	    var uIndex = index % colCount;
	    var vIndex = index / rowCount;
	 
	    float offsetX = (uIndex+colNumber) * size.x;
	    float offsetY = (1.0f - size.y) - (vIndex + rowNumber) * size.y;
	    Vector2 offset = new Vector2(offsetX,offsetY);
	 
	    renderer.material.SetTextureOffset ("_MainTex", offset);
	    renderer.material.SetTextureScale  ("_MainTex", size);

	    renderer.material.SetTextureOffset ("_Illum", offset);
	    renderer.material.SetTextureScale  ("_Illum", size);

	    renderer.material.SetFloat("_EmissionPower", _illumFade);
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
			// Move Right
			rigidbody.MovePosition(rigidbody.position + new Vector3(speed * Time.deltaTime, 0, 0));
			colNum = 0;
			rowNum = 1;
			total = 8;
		}
		else if(transform.position.x - halfWidth > waypoints[curWP].transform.position.x)
		{
			// Move Left
			rigidbody.MovePosition(rigidbody.position + new Vector3(-speed * Time.deltaTime, 0, 0));
			colNum = 0;
			rowNum = 0;
			total = 8;
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
