using UnityEngine;
using System.Collections;

public class CameraFollow : MonoBehaviour
{
	public GameObject player;
	
	void Update()
	{
		Vector3 position = player.transform.position;
		position.y += 4;
		position.z -= 20;
		this.transform.position = position;
	}
}
