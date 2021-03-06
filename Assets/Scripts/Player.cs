using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour
{
	private float speed = 8;
	private float jumpSpeed = 1300.0f;
	private bool grounded = true;
	private int lives = 3;
	private int life = 3;
	private int maxLife = 3;
	private float startX = -12.0f;
	private float startY = 2.55f;
	private Texture[] guiTextures;

	private int col = 8;
	private int row = 8;
	private int rowNum = 0;
	private int colNum = 0;
	private int total = 4;
	private int animSpeed = 10;
	private float illumFade = 1.0f;

	private bool faceRight = true;
	private float halfPlayerHeight;
	//private float halfPlayerWidth;
	private int raycastDistance = 2;
	private bool jump;
	private bool canPhase = false;
	private bool canDoubleJump = false;
	private bool didDoubleJump = false;
	private float distanceMoved;
	private bool[] keys;
	private bool win = false;
	private bool lose = false;
	private float alphaFadeValue = 0;
	private bool invokedLevelChange = false;

	public GameObject new_key;

	public AudioClip[] audioClips;
	private AudioSource[] audioSource;

	public int curState;
	private bool stunned = false;
	private Color stunColor;
	private Color normColor;
	private float SFXVolume;
	private float Gamma;
	private bool justLanded = true;

	private string hintNum = "two";
	private float hintTimer = 14.0f;

	private float winTime = 0.0f;

	enum States
	{
		Normal,
		Invulnerable
	}

	void Start()
	{
		Gamma = GameController.GAMMA.r;
		RenderSettings.ambientLight = GameController.GAMMA;
		stunColor = renderer.material.color;
		normColor = renderer.material.color;
		curState = (int)States.Normal;
		halfPlayerHeight = GetComponent<BoxCollider>().size.y / 2;
		//halfPlayerWidth = GetComponent<BoxCollider>().size.x / 2;
		guiTextures = new Texture[]{
			(Texture)Resources.Load("Texture/gui/gear_life_empty"),
			(Texture)Resources.Load("Texture/gui/gear_life_full"),
			(Texture)Resources.Load("Texture/gui/key_blank"),
			(Texture)Resources.Load("Texture/gui/key_full"),
			(Texture)Resources.Load("Texture/gui/black"),
			(Texture)Resources.Load("Texture/gui/tone_lives")
		};

		audioSource = new AudioSource[audioClips.Length];
		for(int i = 0; i < audioSource.Length; i++){
			audioSource[i] = gameObject.AddComponent<AudioSource>();
			audioSource[i].clip = audioClips[i];
			audioSource[i].volume = GameController.SFX_VOLUME * GameController.MASTER_VOLUME;
		}

		keys = new bool[3];
		keys[0] = false;
		keys[1] = false;
		keys[2] = false;
	}

	void Update()
	{
		hintTimer = hintTimer > 0 ? hintTimer - Time.deltaTime : 0;

		if(GameController.GAMMA.r != Gamma)
		{
			Color ambient = new Color(Gamma, Gamma, Gamma, 1.0f);
			GameController.GAMMA = ambient;
			if(RenderSettings.ambientLight != GameController.GAMMA)
				RenderSettings.ambientLight = GameController.GAMMA;
		}

		if(SFXVolume != GameController.SFX_VOLUME * GameController.MASTER_VOLUME)
		{
			for(int i = 0; i < audioSource.Length; i++)
			{
				SFXVolume = GameController.SFX_VOLUME * GameController.MASTER_VOLUME;
				audioSource[i].volume = SFXVolume;
			}
		}

		if(Input.GetButtonDown("Pause"))
		{
			GameController.PAUSED = !GameController.PAUSED;

			if(GameController.PAUSED)
			{
				Time.timeScale = 0.0f;
				Time.fixedDeltaTime = 0.0f;
			}
			else
			{
				Time.timeScale = 1.0f;
				Time.fixedDeltaTime = 0.02f;
			}
		}

		if(!GameController.PAUSED)
		{
			if(curState == (int)States.Invulnerable)
			{
				stunColor = Color.Lerp(stunColor, Color.red, Time.deltaTime);
				renderer.material.color = stunColor;
			}
			if(curState == (int)States.Normal)
			{
				stunColor = Color.Lerp(stunColor, normColor, Time.deltaTime);
				renderer.material.color = stunColor;
			}

			switch((int)Input.GetAxis("Horizontal"))
			{
				case 0:
				break;
				case 1:
					faceRight = true;
				break;
				case -1:
					faceRight = false;
				break;
				default:
				break;
			}
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

		    if(grounded)
		    {
		    	if(justLanded)
		    		//PlayAudio(12);
	    		justLanded = false;
		    }
		    //Reset Double Jump
		    if(didDoubleJump && grounded){
		    	didDoubleJump = false;
		    }

		    if(!stunned)
		    {
		    	distanceMoved = Input.GetAxis("Horizontal") * speed * Time.deltaTime;
		    	rigidbody.MovePosition(rigidbody.position + new Vector3(distanceMoved,0,0));

				if(Input.GetButtonDown("Jump")){
					if(grounded){
						justLanded = true;
						Jump();
					} else
					if(canDoubleJump && !didDoubleJump){
						DoubleJump();
					}
				} else {
					if(!grounded){
						rigidbody.AddForce(new Vector3(0,-2500.0f,0) * Time.deltaTime);
						if(rigidbody.velocity.y <= 0){
							// Falling
							if(faceRight)
							{
								colNum = 0;
								rowNum = 5;
								total = 4;
							}
							else
							{
								colNum = 4;
								rowNum = 5;
								total = 4;
							}
						} else {
							// Jumping
							if(faceRight)
							{
								colNum = 0;
								rowNum = 4;
								total = 4;
							}
							else
							{
								colNum = 4;
								rowNum = 4;
								total = 4;
							}
						}
					} else {
						switch((int)Input.GetAxis("Horizontal")){
							case 0:
								if(faceRight)
								{
									colNum = 0;
									rowNum = 2;
									total = 6;
								}
								else
								{
									colNum = 0;
									rowNum = 3;
									total = 6;
								}
							break;
							case -1:
								colNum = 0;
								rowNum = 1;
								total = 8;
							break;
							case 1:
								colNum = 0;
								rowNum = 0;
								total = 8;
							break;
							default:
							break;
						}
					}
				}
			}
			SetSpriteAnimation(col, row, colNum, rowNum, total, animSpeed, illumFade);
		}

		//Phasing
		if(canPhase){
			Physics.IgnoreLayerCollision(0,8, Input.GetButton("Phase"));
		}

		//Winning
		if(win){
			if(alphaFadeValue >= 1 && !invokedLevelChange){
				invokedLevelChange = true;
				Invoke("LoadWinScreen", 1.0f);
			}
		}
		else
		{
			if(!GameController.PAUSED)
			{
				winTime += Time.time;
				Debug.Log("Time: " + winTime);
			}
		}
		//Lost
		if(lose){
			if(alphaFadeValue >= 1 && !invokedLevelChange){
				invokedLevelChange = true;
				Invoke("LoadLoseScreen", 1.0f);
			}
		}
	}

	void OnGUI()
	{
		float width = Screen.width / 2;
		float height = Screen.height / 4;

		if(GameController.PAUSED)
		{
			GUI.Label(new Rect(width - 50, height - 50, 100, 20), "Master Volume");
			GameController.MASTER_VOLUME = GUI.HorizontalSlider(new Rect(width - 50, height - 25, 100, 20), GameController.MASTER_VOLUME, 0.0F, 1.0F);
			GUI.Label(new Rect(width + 75, height - 25, 20, 20), "" + GameController.MASTER_VOLUME);

			GUI.Label(new Rect(width - 50, height, 100, 20), "BGM Volume");
			GameController.BGM_VOLUME = GUI.HorizontalSlider(new Rect(width - 50, height + 25, 100, 20), GameController.BGM_VOLUME, 0.0F, 1.0F);
			GUI.Label(new Rect(width + 75, height + 25, 20, 20), "" + GameController.BGM_VOLUME);

			GUI.Label(new Rect(width - 50, height + 50, 100, 20), "SFX Volume");
			GameController.SFX_VOLUME = GUI.HorizontalSlider(new Rect(width - 50, height + 75, 100, 20), GameController.SFX_VOLUME, 0.0F, 1.0F);
			GUI.Label(new Rect(width + 75, height + 75, 20, 20), "" + GameController.SFX_VOLUME);

			GUI.Label(new Rect(width - 50, height + 100, 100, 20), "Gamma");
			Gamma = GUI.HorizontalSlider(new Rect(width - 50, height + 125, 100, 20), Gamma, 0.0F, 0.2F);
			GUI.Label(new Rect(width + 75, height + 125, 20, 20), "" + Gamma);

			GUI.Label(new Rect(width - 75, height + 250, 400, 200),"Controls:\nLeft:   'A'  |  Left Arrow\nRight:  'D'  |  Right Arrow\nJump:   'Space'\nDouble Jump:     'Tap Space Twice Rapidly'\nAction: 'LShift'\nPhase:  'X'\nPause:  'P'");
		}

		for(int i = 0; i < maxLife; i++){
			if(i < life){
				GUI.DrawTexture(new Rect(10 + (i * 40),10,50 + (i * 40),50), guiTextures[1], ScaleMode.ScaleToFit, true);
			} else {
				GUI.DrawTexture(new Rect(10 + (i * 40),10,50 + (i * 40),50), guiTextures[0], ScaleMode.ScaleToFit, true);
			}
		}
		float w = Screen.width - 60;
		for(int i = 0; i < keys.Length; i++){
			if(keys[i]){
				GUI.DrawTexture(new Rect(w - (i * 50),10,50,50), guiTextures[3], ScaleMode.ScaleToFit, true);
			} else {
				GUI.DrawTexture(new Rect(w - (i * 50),10,50,50), guiTextures[2], ScaleMode.ScaleToFit, true);
			}
		}
		for(int i = 0; i < lives; i++){
			GUI.DrawTexture(new Rect(w - (i * 50),Screen.height - 60,50,50), guiTextures[5], ScaleMode.ScaleToFit, true);
		}
		if(hintTimer > 0){
			switch(hintNum){
				case "one":
					GUI.Label(new Rect(width - 150, height, 300, 200),"<color=#a59057>Tone:</color> Last I was here I solved this riddle on the <color=#a59057>whiteboard</color>. I need to sing into the microphones in a specific order. <color=#a59057>[L-SHIFT]</color> while standing in front of a microphone to sing into it. <color=#a59057>Listen</color> for Tones voice.");
				break;
				case "two":
					GUI.Label(new Rect(width - 150, height, 300, 200),"<color=#a59057>Tone:</color> It's dark! I can hit <color=#a59057>[P]</color> to adjust Gamma settings.");
				break;
				case "three":
					GUI.Label(new Rect(width - 150, height, 300, 200),"<color=#a59057>Tone:</color> That <color=#a59057>whiteboard</color>, I almost solved that riddle last time I was here.");
				break;
				case "four":
					GUI.Label(new Rect(width - 150, height, 300, 200),"<color=#a59057>Tone:</color> I remember this riddle. I can't remember where, but I wrote the answer on a <color=#a59057>whiteboard</color> somewhere in the lab...");
				break;
				case "five":
					GUI.Label(new Rect(width - 150, height, 300, 200),"<color=#a59057>Tone:</color> My golem powers have increased. I can <color=#a59057>double jump</color> by pressing <color=#a59057>[SPACE]</color> rapidly.");
				break;
				case "six":
					GUI.Label(new Rect(width - 150, height, 300, 200),"<color=#a59057>Tone:</color> My golem powers have increased. I can <color=#a59057>phase</color> through <color=#c652e7>purple walls</color> by holding down <color=#a59057>[X]</color>.");
				break;
				case "seven":
					GUI.Label(new Rect(width - 150, height, 300, 200),"<color=#a59057>Tone:</color> Finally this golem can rest. Devyn... Please come back soon...");
				break;
			}
		}
		if(win || lose){
			alphaFadeValue += Mathf.Clamp01(Time.deltaTime / 10);
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
				Vector3 normal = new Vector3(Mathf.Round(contact.normal.x), Mathf.Round(contact.normal.y), Mathf.Round(contact.normal.z));
				if(normal == Vector3.up)
				{
					collision.gameObject.SendMessage("DamageSelf", 1);
					PlayAudio(13);
				}
				else
				{
					if(curState == (int)States.Normal)
					{
						DamagePlayer();
					}
					rigidbody.AddForce(normal * 250);
				}
			}
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
		PlayAudio(11);
	}
	
	bool IsAlive()
	{
		return life > 0;
	}

	void DamagePlayer()
	{
		life--;
		stunned = true;
		curState = (int)States.Invulnerable;
		Invoke("invulnerableTimeout", 1.0f);
		if(!IsAlive())
		{
			PlayAudio(10);
			lives--;
			if(lives < 0)
				lose = true;
			else
				Reset();
		} else {
			PlayAudio(09);
		}
	}

	private void invulnerableTimeout()
	{
		curState = (int)States.Normal;
		unStun();
	}

	private void unStun()
	{
		stunned = false;
	}

	public void Kill()
	{
		life -= life;
		stunned = true;
		curState = (int)States.Invulnerable;
		Invoke("invulnerableTimeout", 1.0f);
		if(!IsAlive())
		{
			PlayAudio(10);
			lives--;
			if(lives < 0)
				lose = true;
			else
				Reset();
		} else {
			PlayAudio(09);
		}
	}

	public void Reset()
	{
		Heal();
		transform.position = new Vector3(startX, startY, 0);
	}

	public void Heal()
	{
		if(life < maxLife){
			PlayAudio(8);
		}
		life = maxLife;
	}

	void SetPlayerStart(float posX, float posY)
	{
		startX = posX;
		startY = posY;
	}

	public void PlayAudio(int clipNum)
	{
		if(clipNum > -1){
			//audioSource[clipNum].Play();
			SFX.Instance.Play(clipNum);
			//BGM.Instance.StopSong();
		}
	}

	public void EnablePhase()
	{
		if(canPhase == false){
			PlaySuccessTones();
			ShowHint("six");
			Destroy(GameObject.Find("Phase_Powerup"));
		}
		canPhase = true;
	}

	public void EnableDoubleJump()
	{
		if(canDoubleJump == false){
			PlaySuccessTones();
			ShowHint("five");
			Destroy(GameObject.Find("Double_Jump_Powerup"));
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
			Destroy(GameObject.Find("key_"+key));
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
			if(key == "one"){
				Instantiate(new_key, new Vector3(111.5f, -76.0f, 0.0f), Quaternion.identity);
				PlayAudio(0);
			} else
			if(key == "two"){
				Instantiate(new_key, new Vector3(106.0f, -72.0f, 0.0f), Quaternion.identity);
				PlayAudio(1);
			} else
			if(key == "three"){
				Instantiate(new_key, new Vector3(100.5f, -76.0f, 0.0f), Quaternion.identity);
				PlayAudio(2);
			}
		} else {
			PlayAudio(7);
		}
		return playSound;
	}

	public void ShowHint(string i)
	{
		hintNum = i;
		hintTimer = 12.0f;
	}

	public void WinConditionMet()
	{
		win = true;
		ShowHint("seven");
		Destroy(GameObject.Find("tones_trinket"));
	}

	private void LoadWinScreen()
	{
		Application.LoadLevel("game_win");
	}

	public void LoseConditionMet()
	{
		lose = true;
	}

	private void LoadLoseScreen()
	{
		Application.LoadLevel("game_over");
	}
}