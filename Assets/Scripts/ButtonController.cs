using UnityEngine;
using System.Collections;

public class ButtonController : MonoBehaviour
{
	Button button;

	void Start()
	{
		button = new Button();
	}
	
	void OnTriggerEnter(Collider collider)
	{
		//if(collider.transform.gameObject)
	}
}
