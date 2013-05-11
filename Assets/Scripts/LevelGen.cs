using UnityEngine;
using System.Collections;

public class LevelGen : MonoBehaviour
{
	public GameObject brick;
	private Material trinket_material;
	private Material brick_material;
	void Start()
	{
		GameObject trinket = (GameObject)GameObject.Find("tones_trinket");
		if(MainMenuController.G_item == "top hat"){
			trinket_material = (Material)Resources.Load("Texture/level/trinkets/Materials/tophat");
		} else
		if(MainMenuController.G_item == "telescope"){
			trinket_material = (Material)Resources.Load("Texture/level/trinkets/Materials/scope");
		} else
		if(MainMenuController.G_item == "satchel"){
			trinket_material = (Material)Resources.Load("Texture/level/trinkets/Materials/satchel");
		} else
		if(MainMenuController.G_item == "cane"){
			trinket_material = (Material)Resources.Load("Texture/level/trinkets/Materials/cane");
		} else
		if(MainMenuController.G_item == "corset"){
			trinket_material = (Material)Resources.Load("Texture/level/trinkets/Materials/corset");
		} else
		if(MainMenuController.G_item == "monocle"){
			trinket_material = (Material)Resources.Load("Texture/level/trinkets/Materials/monocle");
		} else { // pocket watch is default
			trinket_material = (Material)Resources.Load("Texture/level/trinkets/Materials/pocket-watch");
		}

		trinket.renderer.material = trinket_material;
		brick_material = (Material)Resources.Load("Texture/level/model_skin/Materials/brick");

		for(int i = 0; i < 18; i++)
		{
			for(int j = 0; j < 44; j++)
			{
				float a = (j % 2 == 0) ? 16.5f : 20.5f;
				GameObject clone = (GameObject)Instantiate(brick, new Vector3((i * 8.0F) - a, (j * 2.8f) - 90, 4.0f), Quaternion.identity);
				clone.transform.parent = transform;
				clone.renderer.material = brick_material;
			}
		}
	}
}
