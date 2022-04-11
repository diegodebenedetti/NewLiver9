using UnityEngine;
using System;
using Core;



public class AudioManager : Singleton<AudioManager>
{
    public Sound[] sounds;
    void Awake()
    {
        foreach (Sound sound in sounds)
        {
            sound.source = gameObject.AddComponent<AudioSource>();
            sound.source.clip = sound.clip;

            sound.source.volume = sound.volume;
            sound.source.pitch = sound.pitch;
            sound.source.loop = sound.loop;
        }
    }

    void Start()
    {
        Play("bg_music");
    }
    public void Play(string name)
    {
        Sound snd = Array.Find(sounds, sound => sound.name == name);
        try
        {
            snd.source.Play();
        }
        catch (Exception e)
        {
            Debug.LogWarning("sound not found");
        }
        
    }

    public void PlayOneShot(string name,bool randomVolume,float randomVolumeMin,float randomVolumeMax)
    {
        Sound snd = Array.Find(sounds, sound => sound.name == name);
        try
        {
            if (randomVolume)
            {
                snd.volume = UnityEngine.Random.Range(randomVolumeMin, randomVolumeMax);
            }
            snd.source.PlayOneShot(snd.clip);
        }
        catch (Exception e)
        {
            Debug.LogWarning("sound not found");
        }

    }

    public void SetVolume(string name, float vol)
    {
        Sound snd = Array.Find(sounds, sound => sound.name == name);
        try
        {
            snd.source.volume = vol;
        }
        catch (Exception e)
        {
            Debug.LogWarning("sound not found");
        }
    }
    public void Stop(string name)
    {
        Sound snd = Array.Find(sounds, sound => sound.name == name);
        try
        {
            snd.source.Stop();
        }
        catch (Exception e)
        {
            Debug.LogWarning("sound not found");
        }
        
    }
}
