using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class SoundManager : MonoSingleton<SoundManager> {

	public AudioMixer audioMixer;
	public AudioSource musicAudioSource; 
	public AudioSource soundAudioSource;

	const string musicPath = "Music/";
	const string soundPath = "Sound/";

	private bool musicOn;
	public bool MusicOn {
		get { return this.musicOn; }
		set {
			this.musicOn = value;
			this.MusicMute(!musicOn);
		}
	}



    private bool soundOn;
	public bool SoundOn
	{
		get { return this.soundOn; }
		set
		{
			this.soundOn = value;
			this.SoundMute(!soundOn);
		}
	}

 

    private int musicVolume;
	public int MusicVolume
	{
		get { return this.musicVolume; }
		set
		{
            if (this.musicVolume != value)
            {
				this.musicVolume = value;
                if (musicOn)
                {
					this.SetVolume("MusicVolume", musicVolume);
                }
			}
		}
	}
	private int soundVolume;
	public int SoundVolume
	{
		get { return this.soundVolume; }
		set
		{
			if (this.soundVolume != value)
			{
				this.soundVolume = value;
				if (soundOn)
				{
					this.SetVolume("SoundVolume", soundVolume);
				}
			}
		}
	}
	// Use this for initialization
	void Start () {
		this.MusicVolume = Config.MusicVolume ;
		this.SoundVolume = Config.SoundVolume;
		this.MusicOn = Config.MusicOn;
		this.soundOn = Config.SoundOn;
	}
	
	// Update is called once per frame
	void Update () {
		
	}
	public void MusicMute(bool mute)
	{
		this.SetVolume("MusicVolume", mute ? 0 : musicVolume);
	}
	public void SoundMute(bool mute)
	{
		this.SetVolume("SoundVolume", mute ? 0 : soundVolume);
	}

  
	

	private void SetVolume(string name, int value)
	{
		float volume = value * 0.5f - 50f;
		this.audioMixer.SetFloat(name , volume);
	}

	public void PlayMusic(string name)
    {
		AudioClip clip = Resloader.Load<AudioClip>(musicPath + name);
        if (clip == null)
        {
			Debug.LogWarningFormat("PlayMusic :{0} is not existed", name);
			return;
        }
        if (musicAudioSource.isPlaying)
        {
			musicAudioSource.Stop();

		}
		musicAudioSource.clip = clip;
		musicAudioSource.Play();

	}

	public void PlaySound(string name)
	{
		AudioClip clip = Resloader.Load<AudioClip>(soundPath + name);
		if (clip == null)
		{
			Debug.LogWarningFormat("PlaySound :{0} is not existed", name);
			return;
		}
		soundAudioSource.PlayOneShot(clip);

	}


}
