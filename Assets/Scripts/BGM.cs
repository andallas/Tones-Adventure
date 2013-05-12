using UnityEngine;
using System.Collections;

class BGM : MonoBehaviour
{
	private static BGM instance = null;
	public static BGM Instance { get { return instance; } }

	public AudioClip[] audioClips;
	private AudioSource[] audioSource;

	private int curTrack;
	private bool loadSong = true;
	private float BGMVolume = 0.15f;

	void Awake()
	{
		if (instance != null && instance != this) 
		{
			Destroy(instance.gameObject);
			return;
		}
		else
		{
			instance = this;
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
		if(!audioSource[curTrack].isPlaying)
		{
			if(loadSong)
			{
				loadSong = false;
				Invoke("PlayNext", 2);
			}
		}
	}

	public void PlayNext()
	{
		curTrack = Random.Range(0, audioClips.Length);
		Stop();
		audioSource[curTrack].Play();
		audioSource[curTrack].volume = BGMVolume;
		loadSong = true;
	}

	public void Stop()
	{
		audio.Stop();
	}
}