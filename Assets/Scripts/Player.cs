using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour
{
	private float speed = 8;
	private float jumpSpeed = 0;
	private float baseJumpSpeed = 0.125f;
	private bool grounded = true;
	private bool jumping = false;
	private int life = 3;
	private int maxLife = 3;
	private float startX = 0.0f;
	private float startY = 1.5f;
	private Texture[] playerLifeTex;

	private int col = 16;
	private int row = 8;
	private int rowNum = 0;
	private int colNum = 0;
	private int total = 4;
	private int animDur = 10;

	void Start()
	{
		playerLifeTex = new Texture[]{(Texture)Resources.Load("Texture/gui/gear_life_empty"), (Texture)Resources.Load("Texture/gui/gear_life_full")};
		jumpSpeed = baseJumpSpeed;
	}

	void FixedUpdate()
	{
		float force = (Input.GetAxis("Horizontal") * speed * Time.deltaTime);
		//rigidbody.AddForce(new Vector3(force,0,0));
		//transform.Translate(force, 0, 0);
		rigidbody.MovePosition(rigidbody.position + new Vector3(force,0,0));

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
			jumping = false;
			rigidbody.AddForce(new Vector3(0,1000.0f,0));
			/*jumpSpeed -= Time.deltaTime;
			//transform.Translate(0, jumpSpeed, 0);
			//rigidbody.AddForce(new Vector3(0,jumpSpeed * 10000 * Time.deltaTime,0));
			rigidbody.AddForce(new Vector3(0, 40.0f, 0));
			//rigidbody.AddExplosionForce(20, rigidbody.position, 10, 3.0f);
			if(jumpSpeed <= 0)
			{
				jumping = false;
				jumpSpeed = baseJumpSpeed;
			}*/
			colNum = 12;
		}
		else
		{
			if(!grounded)
			{
				rigidbody.AddForce(new Vector3(0,-30.0f,0));
				colNum = 8;
			}
			else
			{
				colNum = 0;
			}
		}

		SetSpriteAnimation(col, row, colNum, rowNum, total, animDur);
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
				if(contact.normal == Vector3.up)
				{
					grounded = true;
				}
				Debug.DrawRay(contact.point, contact.normal, Color.red);
			}
		}
	}

	void SetSpriteAnimation(int colCount, int rowCount, int colNumber, int rowNumber, int totalCells, int duration)
	{
	    int index  = (int)(Time.time * duration);
	    index = index % totalCells;
	 
	    float sizeX = 1.0f / colCount;
	    float sizeY = 1.0f / rowCount;
	    Vector2 size =  new Vector2(sizeX,sizeY);
	 
	    var uIndex = index % colCount;
	    var vIndex = index / colCount;
	 
	    float offsetX = (uIndex+colNumber) * size.x;
	    float offsetY = (1.0f - size.y) - (vIndex + rowNumber) * size.y;
	    Vector2 offset = new Vector2(offsetX,offsetY);
	 
	    renderer.material.SetTextureOffset ("_MainTex", offset);
	    renderer.material.SetTextureScale  ("_MainTex", size);
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