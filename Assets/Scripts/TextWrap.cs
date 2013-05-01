/*************************
* Author: Sohpie Houlden *
*************************/
using UnityEngine;
using System.Collections;

public class TextWrap : MonoBehaviour
{
	public string theText = "Hello world blah-dy-blah!";
	public float theWidth = 2f;

	private TextMesh texty;

	void Start()
	{
		texty = GetComponent<TextMesh>();
	}

	void Update()
	{
		texty.text = "";
		string[] words = theText.Split(' ');
		for(int i=0; i<words.Length; i++)
		{
			string workingString = texty.text;
			texty.text = workingString + " " + words[i];
			if(i==0)
				texty.text = workingString + words[i];
			if (texty.renderer.bounds.size.x > theWidth && i>0)
				texty.text = workingString + "\n" + words[i];
		}
	}
}