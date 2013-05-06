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
					if(!status[0]){
						status[0] = (btn[0].ButtonStatus() == 1 && btn[1].ButtonStatus() == 0 && btn[2].ButtonStatus() == 0);
					} else
					if(!status[2]){
						status[2] = (btn[0].ButtonStatus() == 1 && btn[1].ButtonStatus() == 0 && btn[2].ButtonStatus() == 1);
					} else
					if(!status[1]){
						status[1] = (btn[0].ButtonStatus() == 1 && btn[1].ButtonStatus() == 1 && btn[2].ButtonStatus() == 1);
					}
					if(status[1]){
						complete = true;
						platform[0].ToggleActive();
						((Player)GameObject.Find("Player").GetComponent(typeof(Player))).PlaySuccessTonesTwo();
					} else
					if(!status[1] && (btn[0].ButtonStatus() == 1 && btn[1].ButtonStatus() == 1 && btn[2].ButtonStatus() == 1)){
						ResetPuzzle();
					}
				break;

				case "puzzle_two":
					if(btn[6].ButtonStatus() == 1 || btn[7].ButtonStatus() == 1 || btn[8].ButtonStatus() == 1){
						if( (btn[1].ButtonStatus() == 1 && btn[5].ButtonStatus() == 1 && btn[6].ButtonStatus() == 1) &&
								(btn[0].ButtonStatus() == 0 && btn[2].ButtonStatus() == 0 && btn[3].ButtonStatus() == 0) &&
								(btn[4].ButtonStatus() == 0 && btn[7].ButtonStatus() == 0 && btn[8].ButtonStatus() == 0)
							){
								((Player)GameObject.Find("Player").GetComponent(typeof(Player))).PlaySuccessTones();
								btn[6].ActivateTarget();
								complete = true;
							} else {
								ResetPuzzle();
								Player player = (Player)GameObject.Find("Player").GetComponent(typeof(Player));
								player.PlayAudio(5);
								player.transform.position = new Vector3(-1.780525f, -87.31756f, 0);
							}
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
