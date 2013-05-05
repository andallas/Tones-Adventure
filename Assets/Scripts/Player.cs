using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour
{
	private float speed = 8;
	private float jumpSpeed = 1300.0f;
	private bool grounded = true;
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
	private bool jump;
	private bool canPhase = false;
	private bool canDoubleJump = false;
	private bool didDoubleJump = false;
	private float distanceMoved;

	public AudioClip[] audioClips;
	private AudioSource[] audioSource;

	void Start()
	{
		halfPlayerHeight = GetComponent<BoxCollider>().size.y / 2;
		halfPlayerWidth = GetComponent<BoxCollider>().size.x / 2;
		playerLifeTex = new Texture[]{(Texture)Resources.Load("Texture/gui/gear_life_empty"), (Texture)Resources.Load("Texture/gui/gear_life_full")};
		
		audioSource = new AudioSource[audioClips.Length];
		for(int i = 0; i < audioSource.Length; i++){
			audioSource[i] = gameObject.AddComponent<AudioSource>();
			audioSource[i].clip = audioClips[i];
		}
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
    if(Physics.Raycast(transform.position, -Vector3.up * raycastDistance, out hit)) {
        grounded = grounded ? grounded : (hit.distance <= halfPlayerHeight);
    }
    distanceMoved = Input.GetAxis("Horizontal") * speed * Time.deltaTime;
    rigidbody.MovePosition(rigidbody.position + new Vector3(distanceMoved,0,0));

    //Reset Double Jump
    if(didDoubleJump && grounded){
    	didDoubleJump = false;
    }

		if(Input.GetButtonDown("Jump")){
			if(grounded){
				Jump();
			} else
			if(canDoubleJump && !didDoubleJump){
				DoubleJump();
			}
		} else {
			if(!grounded){
				rigidbody.AddForce(new Vector3(0,-2500.0f,0) * Time.deltaTime);
				if(rigidbody.velocity.y <= 0){
					colNum = 0;
					rowNum = 1;
				} else {
					colNum = 12;
					rowNum = 0;
				}
			} else {
				switch((int)Input.GetAxis("Horizontal")){
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

		//Phasing
		if(canPhase){
			Physics.IgnoreLayerCollision(0,8, Input.GetButton("Phase"));
		}
	}

	void OnGUI()
	{
		for(int i = 0; i < maxLife; i++){
			if(i < life){
				GUI.DrawTexture(new Rect(10 + (i * 40),10,50 + (i * 40),50), playerLifeTex[1], ScaleMode.ScaleToFit, true);
			} else {
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

	void DoubleJump()
	{
		didDoubleJump = true;
		Jump();
	}

	void Jump()
	{
		rigidbody.AddForce(new Vector3(0,jumpSpeed,0));
		colNum = 12;
		rowNum = 0;
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
			Reset();
		}
	}

	public void Reset()
	{
		life = maxLife;
		transform.position = new Vector3(startX, startY, 0);
	}

	void SetPlayerStart(float posX, float posY)
	{
		startX = posX;
		startY = posY;
	}

	public void PlayAudio(int clipNum)
	{
		audioSource[clipNum].Play();
	}

	public void EnablePhase()
	{
		canPhase = true;
	}
	public void EnableDoubleJump()
	{
		canDoubleJump = true;
	}

	public void PlaySuccessTones()
	{
		StartCoroutine(SuccessToneOne());
	}

	private IEnumerator SuccessToneOne()
	{
		PlayAudio(2);
		yield return new WaitForSeconds(1);
		PlayAudio(1);
		yield return new WaitForSeconds(1);
		PlayAudio(3);
	}

	public float DistanceMoved()
	{
		return distanceMoved;
	}
}
