using UnityEngine;
using System.Collections;
public class Puzzle : MonoBehaviour {
	public ButtonController[] btn;
	public SlidingPlatform[] platform;
	public string puzzleName;
	private bool[] status;
	private bool complete = false;

	void Start(){
		status = new bool[btn.Length];
		for(int i = 0; i < status.Length; i++){
	    status[i] = false;
	  }
	}
	
	void Update(){
		if(!complete){
			switch(puzzleName){
				case "default":
					Debug.Log("This action is called: " + puzzleName);
				break;
				case "puzzle_one":
					if(!status[1]){
						status[1] = (btn[0].ButtonStatus() == 0 && btn[1].ButtonStatus() == 1 && btn[2].ButtonStatus() == 0);
					} else
					if(!status[0]){
						status[0] = (btn[0].ButtonStatus() == 1 && btn[1].ButtonStatus() == 1 && btn[2].ButtonStatus() == 0);
					} else
					if(!status[2]){
						status[2] = (btn[0].ButtonStatus() == 1 && btn[1].ButtonStatus() == 1 && btn[2].ButtonStatus() == 1);
					}
					if(status[2]){
						complete = true;
						platform[0].ToggleActive();
					} else
					if(!status[2] && (btn[0].ButtonStatus() == 1 && btn[1].ButtonStatus() == 1 && btn[2].ButtonStatus() == 1)){
						ResetPuzzle();
					}
				break;
				default:
					Debug.LogError("No Puzzle Name Set.");
				break;
			}
		}
	}

	void ResetPuzzle(){
		for(int i = 0; i < status.Length; i++){
	    status[i] = false;
	  }
	  for(int i = 0; i < btn.Length; i++){
	    btn[i].ButtonStatus(0);
	  }
	}
}
