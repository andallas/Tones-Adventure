using UnityEngine;
using System.Collections;

class SFX : MonoBehaviour
{
	private static SFX sfx_instance = null;
	public static SFX Instance { get { return sfx_instance; } }

	public AudioClip[] audioClips;
	private AudioSource[] audioSource;

	private float SFXVolume;

	void Awake()
	{
		if (sfx_instance != null && sfx_instance != this) {
			Destroy(sfx_instance.gameObject);
		} else {
			sfx_instance = this;
		}
		DontDestroyOnLoad(gameObject);
	}

	void Start()
	{
		audioSource = new AudioSource[audioClips.Length];
		for(int i = 0; i < audioSource.Length; i++)
		{
			audioSource[i] = gameObject.AddComponent<AudioSource>();
			audioSource[i].clip = audioClips[i];
		}
	}

	void Update()
	{
		if(SFXVolume != GameController.SFX_VOLUME * GameController.MASTER_VOLUME)
		{
			for(int i = 0; i < audioSource.Length; i++)
			{
				SFXVolume = GameController.SFX_VOLUME * GameController.MASTER_VOLUME;
				audioSource[i].volume = SFXVolume;
			}
		}
	}

	public void Play(int i)
	{
		audioSource[i].Play();
	}
}