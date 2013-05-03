using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour
{
	private float speed = 8;
	private bool grounded = true;
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
	private float halfPlayerHeight;
	private float halfPlayerWidth;
	private int raycastDistance = 2;

	void Start()
	{
		halfPlayerHeight = GetComponent<BoxCollider>().size.y / 2;
		halfPlayerWidth = GetComponent<BoxCollider>().size.x / 2;
		playerLifeTex = new Texture[]{(Texture)Resources.Load("Texture/gui/gear_life_empty"), (Texture)Resources.Load("Texture/gui/gear_life_full")};
	}

	void Update()
	{
		RaycastHit hit;
    if(Physics.Raycast(transform.position + new Vector3(1.0f,0,0), -Vector3.up * raycastDistance, out hit)){
    		grounded = (hit.distance <= halfPlayerHeight);
    }
    if(Physics.Raycast(transform.position - new Vector3(1.0f,0,0), -Vector3.up * raycastDistance, out hit)){
        grounded = grounded ? grounded : (hit.distance <= halfPlayerHeight);
    }
    if (Physics.Raycast(transform.position, -Vector3.up * raycastDistance, out hit)) {
        grounded = grounded ? grounded : (hit.distance <= halfPlayerHeight);
    }
    float distanceMoved = Input.GetAxis("Horizontal") * speed * Time.deltaTime;
    rigidbody.MovePosition(rigidbody.position + new Vector3(distanceMoved,0,0));

		if(Input.GetButtonDown("Jump")){
			if(grounded){
				grounded = false;
				rigidbody.AddForce(new Vector3(0,1000.0f,0));
				colNum = 12;
			}
		}
		if(!grounded){
			rigidbody.AddForce(new Vector3(0,-30.0f,0));
			colNum = 8;
		} else {
			colNum = 0;
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