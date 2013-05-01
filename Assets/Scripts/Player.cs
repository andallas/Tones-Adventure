using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour
{
	private float speed = 8;
	private float jumpSpeed = 0.325f;
	private bool grounded = true;
	private bool jumping = false;
	private int life = 3;
	private int maxLife = 3;
	private float startX = 0.0f;
	private float startY = 1.7f;
	private Texture[] playerLifeTex;

	void Start()
	{
		playerLifeTex = new Texture[]{(Texture)Resources.Load("Texture/gui/gear_life_empty"), (Texture)Resources.Load("Texture/gui/gear_life_full")};
	}
	
	void Update()
	{
		transform.Translate(Input.GetAxis("Horizontal") * speed * Time.deltaTime, 0, 0);

		if(Input.GetButtonDown("Jump"))
		{
			if(grounded)
			{
				grounded = false;
				jumping = true;
			}
		}
		if(jumping)
		{
			jumpSpeed -= Time.deltaTime;
			transform.Translate(0, jumpSpeed, 0);
			if(jumpSpeed <= 0)
			{
				jumping = false;
				jumpSpeed = 0.325f;
			}
		}
	}

	void OnGUI()
	{
		for(int i = 0; i < maxLife; i++)
		{
			if(i < life)
			{
				GUI.DrawTexture(new Rect(10 + (i * 40),10,50 + (i * 40),50), playerLifeTex[1], ScaleMode.ScaleToFit, true);
			}
			else 
			{
				GUI.DrawTexture(new Rect(10 + (i * 40),10,50 + (i * 40),50), playerLifeTex[0], ScaleMode.ScaleToFit, true);
			}
		}
	}

	void OnCollisionEnter(Collision collision)
	{
		if(!grounded)
		{
			foreach(ContactPoint contact in collision.contacts)
			{
				Debug.DrawRay(contact.point, contact.normal, Color.red);
				grounded = true;
			}
		}
	}

	bool IsAlive()
	{
		return (life <= 0);
	}

	void DamagePlayer()
	{
		life--;
		if(IsAlive())
		{
			ResetPlayer();
		}
	}

	void ResetPlayer()
	{
		life = maxLife;
		transform.position = new Vector3(startX, startY, 0);
	}

	void SetPlayerStart(float posX, float posY)
	{
		startX = posX;
		startY = posY;
	}
}