using UnityEngine;
using System.Collections;
public class WinController : MonoBehaviour {
	private Ray ray;
	private RaycastHit hit;

	void Start(){
		string item = GameController.ITEM == null ? "pocket watch" : GameController.ITEM;
		string villain = GameController.VILLAIN == null ? "Dr. Calvin Von-Brothenheimer" : GameController.VILLAIN;
		GameObject.Find("Blurb").GetComponent<TextMesh>().text = "The curious golem Tone has \nobtained her <color=#a59057>" + item + "</color> from \n<color=#a59057>" + villain + "</color>.";
		GameObject.Find("Thanks").GetComponent<TextMesh>().text = "Thank you so much for playing \n<color=#a59057>Tone's Adventure</color>!";
		GameObject.Find("Credits").GetComponent<TextMesh>().text = "Lead Developers \n<color=#a59057>Justin Hammond</color> & <color=#a59057>Shawn Deprey</color>\n\n" + 
		"Sprites, Animations & Poster\n<color=#a59057>Zachery Madere</color>\n\n" + "Music & Sound\n<color=#a59057>David van Laar-Veth</color>\n\n" + 
		"3D Assets\n<color=#a59057>Justin Hammond</color>\n\n" + "Level Design & Textures\n<color=#a59057>Shawn Deprey</color>\n\n";
	}
	
	void Update(){
		if(Input.GetMouseButtonDown(0)){
			ray = Camera.main.ScreenPointToRay(Input.mousePosition);
			if(Physics.Raycast(ray, out hit)){
				if(hit.transform.name == "Restart"){
					GameObject.Find("Main Camera").audio.Play();
					Invoke("LoadLevel", 0.4f);
				}
			}
		}
	}

	void LoadLevel(){
		Application.LoadLevel("main_menu");
	}
}
