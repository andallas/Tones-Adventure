using UnityEngine;
using System.Collections;

public class Logo : MonoBehaviour
{
	private Color color;
	private float fadeTime = 6;
	private bool changeScene = false;
	private bool fadeIn = true;
	private bool fadeOut = false;


	void Start()
	{
		color = renderer.material.color;
		StartCoroutine("startFade");
	}
	
	void Update()
	{
		if(changeScene)
		{
			StartCoroutine("LoadMainMenu");
			changeScene = false;
		}
		else
		{
			if(fadeIn)
			{
				color.a += Time.deltaTime / fadeTime;
				renderer.material.color = color;

				if(color.a >= 0.5f)
				{
					fadeIn = false;
					StartCoroutine("FadeOut");
				}
			}
			if(!fadeIn)
			{
				if(fadeOut)
				{
					color.a -= Time.deltaTime / fadeTime;
					renderer.material.color = color;

					if(color.a <= 0.0f)
					{
						changeScene = true;
						fadeOut = false;
					}
				}
			}
		}
	}

	IEnumerator startFade()
	{
		yield return new WaitForSeconds(0.85f);
		fadeTime = 2;
	}

	IEnumerator FadeOut()
	{
		yield return new WaitForSeconds(1.25f);
		fadeOut = true;
	}

	IEnumerator LoadMainMenu()
	{
		yield return new WaitForSeconds(1.5f);
		LoadGame();
	}

	void LoadGame()
	{
		Application.LoadLevel("main_menu");
	}
}
