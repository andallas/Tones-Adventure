using UnityEngine;
using System.Collections;
public class SlidingPlatform : MonoBehaviour {
	public float speed;
	public int direction;
	public float distanceToTravel;
	public bool elevator;
	public bool forceStayActive;
	private float distanceTraveled = 0;
	private GameObject child = null;
	public bool isActive;

	void Start(){
		//Ensure we don't start out of our expected dataset
		direction = direction <= -1 ? -1 : 1;
	}
	
	void Update(){
		if(!GameController.PAUSED)
		{
			if(isActive){
				float moveDistance = direction * speed * Time.deltaTime;
				distanceTraveled += Mathf.Abs(moveDistance);

				if(elevator){
					transform.Translate(0,moveDistance,0);
					if(child != null){
						Player player = (Player)child.GetComponent(typeof(Player));
						player.transform.Translate(player.DistanceMoved(),moveDistance,0);
					}
				} else {
					transform.Translate(moveDistance,0,0);
					if(child != null){
						Player player = (Player)child.GetComponent(typeof(Player));
						player.transform.Translate(moveDistance + player.DistanceMoved(),0,0);
					}
				}

				if(distanceTraveled >= distanceToTravel){
					direction = direction == 1 ? -1 : 1;
					distanceTraveled = 0;
					if(elevator && !forceStayActive){
						isActive = false;
					}
				}
			}
		}
	}

	void OnCollisionEnter(Collision collision)
	{
		if(collision.gameObject.tag == "Player"){
			child = collision.gameObject;
		}
	}

	void OnCollisionExit(Collision collision)
	{
		child = null;
	}

	public void ToggleActive()
	{
		//Ensure we don't turn off verticle platforms if they are active.
		isActive = (elevator && isActive) ? true : !isActive;
	}
}
