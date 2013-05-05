using UnityEngine;
using System.Collections;
public class SlidingPlatform : MonoBehaviour {
	public float speed;
	public int direction;
	public float distanceToTravel;
	public bool elevator;
	private float distanceTraveled = 0;
	private GameObject child = null;
	private bool isActive = true;

	void Start(){
		//Ensure we don't start out of our expected dataset
		direction = direction <= -1 ? -1 : 1;
		isActive = !(elevator);
	}
	
	void Update(){
		if(isActive){
			float moveDistance = direction * speed * Time.deltaTime;
			distanceTraveled += Mathf.Abs(moveDistance);

			if(elevator){
				transform.Translate(0,moveDistance,0);
				if(child != null){
					child.transform.Translate(0,moveDistance,0);
				}
			} else {
				transform.Translate(moveDistance,0,0);
				if(child != null){
					child.transform.Translate(moveDistance,0,0);
				}
			}

			if(distanceTraveled >= distanceToTravel){
				direction = direction == 1 ? -1 : 1;
				distanceTraveled = 0;
				if(elevator){
					isActive = false;
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
