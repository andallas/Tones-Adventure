using UnityEngine;
using System.Collections;

class BGM : MonoBehaviour
{
	private static BGM instance = null;
	public static BGM Instance { get { return instance; } }

	public AudioClip[] audioClips;
	private AudioSource[] audioSource;

	private int curTrack = 2;
	private bool loadSong = true;
	private float BGMVolume;

	private bool paused = false;

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
		if(BGMVolume != GameController.BGM_VOLUME * GameController.MASTER_VOLUME)
		{
			BGMVolume = GameController.BGM_VOLUME * GameController.MASTER_VOLUME;
			audioSource[curTrack].volume = BGMVolume;
		}

		if(GameController.PAUSED)
		{
			if(!paused)
			{
				PauseSong();
				paused = true;
			}
		}
		else
		{
			if(paused)
			{
				PlaySong();
				paused = false;
			}
		}

		if(!paused)
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
	}

	public void PlayNext()
	{
		curTrack = Random.Range(0, audioClips.Length);
		StopSong();
		audioSource[curTrack].Play();
		audioSource[curTrack].volume = BGMVolume;
		loadSong = true;
	}

	public void StopSong()
	{
		audioSource[curTrack].Stop();
	}

	public void PauseSong()
	{
		audioSource[curTrack].Pause();
	}

	public void PlaySong()
	{
		audioSource[curTrack].Play();
	}
}