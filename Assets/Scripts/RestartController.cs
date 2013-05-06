using UnityEngine;
using System.Collections;
public class RestartController : MonoBehaviour {
	private Ray ray;
	private RaycastHit hit;
	
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
