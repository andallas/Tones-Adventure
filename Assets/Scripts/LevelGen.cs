using UnityEngine;
using System.Collections;

public class LevelGen : MonoBehaviour
{
	public GameObject brick;

	void Start()
	{
		for(int i = 0; i < 18; i++)
		{
			for(int j = 0; j < 44; j++)
			{
				float a = (j % 2 == 0) ? 19.5f : 20.5f;
				GameObject clone = (GameObject)Instantiate(brick, new Vector3((i * 8.0F) - a, (j * 2.8f) - 90, 4.0f), Quaternion.identity);
				clone.transform.parent = this.transform;
			}
		}
	}
}
