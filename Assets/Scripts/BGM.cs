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

	void Awake()
	{
		instance = this;
		DontDestroyOnLoad(gameObject);
	}

	void Start()
	{
		audioSource = new AudioSource[audioClips.Length];
		for(int i = 0; i < audioSource.Length; i++)
		{
			audioSource[i] = gameObject.AddComponent<AudioSource>();
			audioSource[i].clip = audioClips[i];
			audioSource[i].volume = 0.25f;
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
		audioSource[curTrack].Play();
		loadSong = true;
	}

	public void Stop()
	{
		audio.Stop();
	}
}