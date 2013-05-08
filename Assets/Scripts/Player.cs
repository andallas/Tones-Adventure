using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour
{
	private float speed = 8;
	private float jumpSpeed = 1300.0f;
	private bool grounded = true;
	private int life = 3;
	private int maxLife = 3;
	private float startX = -12.0f;
	private float startY = 2.55f;
	private Texture[] guiTextures;

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
	private bool[] keys;
	private bool win = false;
	private float alphaFadeValue = 0;
	private bool invokedLevelChange = false;

	public AudioClip[] audioClips;
	private AudioSource[] audioSource;

	void Start()
	{
		halfPlayerHeight = GetComponent<BoxCollider>().size.y / 2;
		halfPlayerWidth = GetComponent<BoxCollider>().size.x / 2;
		guiTextures = new Texture[]{
			(Texture)Resources.Load("Texture/gui/gear_life_empty"),
			(Texture)Resources.Load("Texture/gui/gear_life_full"),
			(Texture)Resources.Load("Texture/gui/key_blank"),
			(Texture)Resources.Load("Texture/gui/key_full"),
			(Texture)Resources.Load("Texture/gui/black")
		};
		
		audioSource = new AudioSource[audioClips.Length];
		for(int i = 0; i < audioSource.Length; i++){
			audioSource[i] = gameObject.AddComponent<AudioSource>();
			audioSource[i].clip = audioClips[i];
		}

		keys = new bool[3];
		keys[0] = false;
		keys[1] = false;
		keys[2] = false;
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

		//Winning
		if(win){
			if(alphaFadeValue >= 1 && !invokedLevelChange){
				invokedLevelChange = true;
				Invoke("LoadWinScreen", 2.0f);
			}
		}
	}

	void OnGUI()
	{
		for(int i = 0; i < maxLife; i++){
			if(i < life){
				GUI.DrawTexture(new Rect(10 + (i * 40),10,50 + (i * 40),50), guiTextures[1], ScaleMode.ScaleToFit, true);
			} else {
				GUI.DrawTexture(new Rect(10 + (i * 40),10,50 + (i * 40),50), guiTextures[0], ScaleMode.ScaleToFit, true);
			}
		}
		float w = Screen.width - 60;
		float w_2 = w + 40;
		for(int i = 0; i < keys.Length; i++){
			if(keys[i]){
				GUI.DrawTexture(new Rect(w - (i * 50),10,50,50), guiTextures[3], ScaleMode.ScaleToFit, true);
			} else {
				GUI.DrawTexture(new Rect(w - (i * 50),10,50,50), guiTextures[2], ScaleMode.ScaleToFit, true);
			}
		}
		if(win){
			alphaFadeValue += Mathf.Clamp01(Time.deltaTime / 5);
			GUI.color = new Color(0, 0, 0, alphaFadeValue);
			GUI.DrawTexture( new Rect(0, 0, Screen.width, Screen.height ), guiTextures[4] );
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

	public void Heal()
	{
		if(life < maxLife){
			PlayAudio(8);
		}
		life = maxLife;
	}

	public void Reset()
	{
		Heal();
		transform.position = new Vector3(startX, startY, 0);
	}

	void SetPlayerStart(float posX, float posY)
	{
		startX = posX;
		startY = posY;
	}

	public void PlayAudio(int clipNum)
	{
		if(clipNum > -1){
			audioSource[clipNum].Play();
		}
	}

	public void EnablePhase()
	{
		if(canPhase == false){
			PlaySuccessTones();
		}
		canPhase = true;
	}
	public void EnableDoubleJump()
	{
		if(canDoubleJump == false){
			PlaySuccessTones();
		}
		canDoubleJump = true;
	}

	public void PlaySuccessTones()
	{
		StartCoroutine(SuccessToneOne());
	}

	private IEnumerator SuccessToneOne()
	{
		yield return new WaitForSeconds(2f);
		PlayAudio(1);
		yield return new WaitForSeconds(1f);
		PlayAudio(2);
		yield return new WaitForSeconds(1f);
		PlayAudio(3);
	}

	public void PlaySuccessTonesTwo()
	{
		StartCoroutine(SuccessToneTwo());
	}

	private IEnumerator SuccessToneTwo()
	{
		yield return new WaitForSeconds(2f);
		PlayAudio(1);
		yield return new WaitForSeconds(1f);
		PlayAudio(3);
		yield return new WaitForSeconds(1f);
		PlayAudio(2);
	}

	public float DistanceMoved()
	{
		return distanceMoved;
	}

	public void PickupKey(string key)
	{
		bool playSound = false;
		if(key == "one"){
			playSound = !keys[0];
			keys[0] = true;
		} else
		if(key == "two"){
			playSound = !keys[1];
			keys[1] = true;
		} else
		if(key == "three"){
			playSound = !keys[2];
			keys[2] = true;
		}
		if(playSound){
			PlayAudio(6);
		}
	}

	public bool PlaceKey(string key)
	{
		bool playSound = false;
		if(key == "one"){
			playSound = keys[0];
			keys[0] = false;
		} else
		if(key == "two"){
			playSound = keys[1];
			keys[1] = false;
		} else
		if(key == "three"){
			playSound = keys[2];
			keys[2] = false;
		}
		if(playSound){
			PlayAudio(6);
		} else {
			PlayAudio(7);
		}
		return playSound;
	}

	public void WinConditionMet()
	{
		win = true;
	}

	private void LoadWinScreen()
	{
		Application.LoadLevel("game_win");
	}
}
