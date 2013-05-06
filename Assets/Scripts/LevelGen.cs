using UnityEngine;
using System.Collections;

public class LevelGen : MonoBehaviour
{
	public GameObject background;

	void Start()
	{
		for(int i = 0; i < 10; i++)
		{
			for(int j = 0; j < 5; j++)
			{
				Instantiate(prefab, new Vector3(i * 2.0F, 0, 0), Quaternion.identity);
			}
		}
	}
}
