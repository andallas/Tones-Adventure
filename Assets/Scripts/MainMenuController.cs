using UnityEngine;
using System.Collections;
public class MainMenuController : MonoBehaviour {
	private Ray ray;
	private RaycastHit hit;
	public string item;
	public string villain;

	void Start(){
		//Random
		string[] items = new string[]{"pocket watch", "monocle", "corset", "cane", "satchel", "telescope", "top hat"};
		string[] villains = new string[]{
			"Prof. Dr. Jerome L. MacAndrew II",
			"Doctor Z. Bingley Day",
			"Inquisitor Willie Gennings Kane",
			"Professor Lester S. Hollaway III",
			"Prof. Dr. Mikas Owen Havisham",
			"Dr. Algernon W. McBride",
			"Doctor Thomas C. Rosenthal",
			"Dr. Abraham W. Broughan",
			"Prof. Miles Plouden Mulready II",
			"Professor Poter Mullins Whiteman Esq.",
			"Dr. Calvin Von-Brothenheimer",
			"Dr. Edwina Maister Benzie",
			"Professor Nettie Clavers McLachlan II",
			"Prof. Dr. B. Louise Heffernan",
			"Doctor Ursula S. Warburton",
			"Dr. I. Landers McFall",
			"Professor A. Shickleton Goodwyn",
			"Professor B. Flannagan Morrisay III",
			"Prof. Beale Sheppard Shanaghan II",
			"Doctor Isabel McKenzie Merekaun",
			"Professor Arrah T. Hertpool"
		};
		item = items[Random.Range(0,7)];
		GameController.ITEM = item;
		villain = villains[Random.Range(0,21)];
		GameController.VILLAIN = villain;
		GameObject.Find("Objective").GetComponent<TextMesh>().text = "Tone needs to obtain her <color=#a59057>"+item+"</color> which was stolen by <color=#a59057>\n"+villain+"</color>, so she heads to the laboratory to find it.";
	}
	
	void Update(){
		if(Input.GetMouseButtonDown(0)){
			ray = Camera.main.ScreenPointToRay(Input.mousePosition);
			if(Physics.Raycast(ray, out hit)){
				if(hit.transform.name == "Play Text"){
					audio.Play();
					Invoke("LoadLevel", 0.4f);
				}
			}
		}
	}

	void LoadLevel(){
		//Application.LoadLevel("game_scene");
		Application.LoadLevel("shawn_game_scene");
	}
}
