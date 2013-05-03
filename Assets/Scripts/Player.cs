using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour
{
	private float speed = 8;
	private float jumpSpeed = 1000.0f;
	private bool grounded = true;
	private bool jumping = false;
	private int life = 3;
	private int maxLife = 3;
	private float startX = 0.0f;
	private float startY = 2.6f;
	private Texture[] playerLifeTex;

	private int col = 16;
	private int row = 8;
	private int rowNum = 0;
	private int colNum = 0;
	private int total = 4;
	private int animSpeed = 10;

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
    		//Debug.Log(hit.distance);
    		grounded = (hit.distance <= halfPlayerHeight);
    	}
    	if(Physics.Raycast(transform.position - new Vector3(1.0f,0,0), -Vector3.up * raycastDistance, out hit)){
        	grounded = grounded ? grounded : (hit.distance <= halfPlayerHeight);
    	}
    	if (Physics.Raycast(transform.position, -Vector3.up * raycastDistance, out hit)) {
    	    grounded = grounded ? grounded : (hit.distance <= halfPlayerHeight);
    	}

	    float distanceMoved = Input.GetAxis("Horizontal") * speed * Time.deltaTime;
	    bool allowMoveLeft = true;
	    bool allowMoveRight = true;

	    Debug.DrawRay(transform.position + new Vector3(-halfPlayerWidth,halfPlayerHeight,0), -Vector3.right * raycastDistance, Color.red);
	    Debug.DrawRay(transform.position + new Vector3(-halfPlayerWidth,0,0), -Vector3.right * raycastDistance, Color.red);
	  	Debug.DrawRay(transform.position + new Vector3(-halfPlayerWidth,-halfPlayerHeight + 0.25f,0), -Vector3.right * raycastDistance, Color.red);

	    if(Physics.Raycast(transform.position + new Vector3(-halfPlayerWidth,halfPlayerHeight,0), -Vector3.right * raycastDistance, out hit)) {
	    	//distanceMoved = (hit.distance <= Mathf.Abs(distanceMoved)) ? (hit.distance - 0.24f) * Input.GetAxis("Horizontal") : distanceMoved;
	    	allowMoveLeft = !(Mathf.Abs(distanceMoved) <= hit.distance + 0.25f);
	    }
	    if(Physics.Raycast(transform.position + new Vector3(-halfPlayerWidth,0,0), -Vector3.right * raycastDistance, out hit)) {
	      //distanceMoved = (hit.distance <= Mathf.Abs(distanceMoved)) ? (hit.distance - 0.24f) * Input.GetAxis("Horizontal") : distanceMoved;
	      allowMoveLeft = !(Mathf.Abs(distanceMoved) <= hit.distance + 0.25f);
	    }
	    if(Physics.Raycast(transform.position + new Vector3(-halfPlayerWidth,-halfPlayerHeight + 0.25f,0), -Vector3.right * raycastDistance, out hit)) {
	    	//distanceMoved = (hit.distance <= Mathf.Abs(distanceMoved)) ? (hit.distance - 0.24f) * Input.GetAxis("Horizontal") : distanceMoved;
	    	allowMoveLeft = !(Mathf.Abs(distanceMoved) <= hit.distance + 0.25f);
	    }

	    Debug.DrawRay(transform.position + new Vector3(halfPlayerWidth,halfPlayerHeight,0), Vector3.right * raycastDistance, Color.red);
	    Debug.DrawRay(transform.position + new Vector3(halfPlayerWidth,0,0), Vector3.right * raycastDistance, Color.red);
	  	Debug.DrawRay(transform.position + new Vector3(halfPlayerWidth,-halfPlayerHeight + 0.25f,0), Vector3.right * raycastDistance, Color.red);

			if(Physics.Raycast(transform.position + new Vector3(halfPlayerWidth,halfPlayerHeight,0), Vector3.right * raycastDistance, out hit)) {
				//distanceMoved = (hit.distance <= Mathf.Abs(distanceMoved)) ? (hit.distance - 0.24f) * Input.GetAxis("Horizontal") : distanceMoved;
				allowMoveRight = !(Mathf.Abs(distanceMoved) <= hit.distance + 0.25f);
	    }
	    if(Physics.Raycast(transform.position + new Vector3(halfPlayerWidth,0,0), Vector3.right * raycastDistance, out hit)) {
	      //distanceMoved = (hit.distance <= Mathf.Abs(distanceMoved)) ? (hit.distance - 0.24f) * Input.GetAxis("Horizontal") : distanceMoved;
	      allowMoveRight = !(Mathf.Abs(distanceMoved) <= hit.distance + 0.25f);
	    }
	    if(Physics.Raycast(transform.position + new Vector3(halfPlayerWidth,-halfPlayerHeight + 0.25f,0), Vector3.right * raycastDistance, out hit)) {
	    	//distanceMoved = (hit.distance <= Mathf.Abs(distanceMoved)) ? (hit.distance - 0.24f) * Input.GetAxis("Horizontal") : distanceMoved;
	    	allowMoveRight = !(Mathf.Abs(distanceMoved) <= hit.distance + 0.25f);
	    }

	    //if(allowMove){
	    //	rigidbody.MovePosition(rigidbody.position + new Vector3(distanceMoved,0,0));
	    //}
	    if(Input.GetAxis("Horizontal") == -1 && allowMoveLeft){
	    	rigidbody.MovePosition(rigidbody.position + new Vector3(distanceMoved,0,0));
	  	} else
	  	if(Input.GetAxis("Horizontal") == 1 && allowMoveRight){
	  		rigidbody.MovePosition(rigidbody.position + new Vector3(distanceMoved,0,0));
	  	}
	  	//rigidbody.MovePosition(rigidbody.position + new Vector3(distanceMoved,0,0));


	  	/*if(rigidbody.position != distanceMoved){
	  		rigidbody.MovePosition(rigidbody.position + distanceMoved);
	  	} else {
	  		rigidbody.MovePosition(rigidbody.position - distanceMoved);
	  	}*/

	  	/*if(Input.GetAxis("Horizontal") == 1){
	  		
	  	} else
	  	if(Input.GetAxis("Horizontal") == -1){

	  	}*/

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
			rigidbody.AddForce(new Vector3(0, jumpSpeed, 0));
			jumping = false;
			colNum = 12;
			rowNum = 0;
		}
		else
		{
			if(!grounded)
			{
				rigidbody.AddForce(new Vector3(0,-30.0f,0));
				if(rigidbody.velocity.y <=0)
				{
					colNum = 0;
					rowNum = 1;
				}
				else
				{
					colNum = 12;
					rowNum = 0;
				}
			}
			else
			{
				switch((int)Input.GetAxis("Horizontal"))
				{
					case 0:
						colNum = 0;
						rowNum = 0;
					break;
					case -1:
						colNum = 4;
						rowNum = 0;
					break;
					case 1:
						colNum = 8;
						rowNum = 0;
					break;
					default:
						colNum = 0;
						rowNum = 0;
					break;
				}
			}
		}
		SetSpriteAnimation(col, row, colNum, rowNum, total, animSpeed);
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
		foreach(ContactPoint contact in collision.contacts)
		{
			if(collision.gameObject.tag == "Enemy")
			{
				
				if(contact.normal == Vector3.up)
				{
					collision.gameObject.SendMessage("DamageSelf", 1);
				}
				else
				{
					DamagePlayer();
				}
				Debug.DrawRay(contact.point, contact.normal, Color.red);
			}
		}
		if(!grounded)
		{
			foreach(ContactPoint contact in collision.contacts)
			{
				Debug.Log(contact.normal == Vector3.up);
				if(contact.normal == Vector3.up)
				{
					grounded = true;
				}
				Debug.DrawRay(contact.point, contact.normal, Color.red);
			}
		}
	}

	void SetSpriteAnimation(int colCount, int rowCount, int colNumber, int rowNumber, int totalCells, int _animSpeed)
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
	}
	
	bool IsAlive()
	{
		return life > 0;
	}

	void DamagePlayer()
	{
		life--;
		if(!IsAlive())
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