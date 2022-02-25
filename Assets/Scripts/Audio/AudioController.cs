using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class AudioController : MonoBehaviour
{
    [SerializeField] List<AudioClip> _clips;
    [SerializeField] float _pitchMin, _pitchMax;
     
    AudioSource _source;
    void Start() 
        => _source = GetComponent<AudioSource>();


    public void PlaySound(float value)
    {    
        _source.clip = _clips[Random.Range(0, _clips.Count)];
        _source.pitch = value; 
        _source.Play();
    }

    public void PlaySound()
    {
        _source.clip = _clips[Random.Range(0, _clips.Count)];
        _source.pitch = Random.Range(_pitchMin, _pitchMax);
        _source.Play();
    }
}
