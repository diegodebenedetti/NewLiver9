using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RainTrigger : MonoBehaviour
{
    [SerializeField] LayerMask _playerLayer;
    [SerializeField] string _soundName;
    [SerializeField] float _maxVolume, _minVolume, _time, timer;
    void OnTriggerEnter(Collider other)
    {
        if((1 << other.gameObject.layer & _playerLayer) != 0) 
            StartCoroutine(VolumeDownCoroutine());   
    }

    IEnumerator VolumeDownCoroutine()
    {
        timer = 0; 
        Sound sounds = Array.Find(AudioManager.Instance.sounds, sound => sound.name == _soundName);
        bool volumeUp = sounds.source.volume == _minVolume;
        
        while(timer < _time)
        {
            timer += Time.deltaTime;
            var lerp = timer/_time;
            AudioManager.Instance.SetVolume(_soundName,Mathf.Lerp(sounds.source.volume, volumeUp ? _maxVolume : _minVolume, lerp));
            yield return null;
        }
 
        AudioManager.Instance.SetVolume(_soundName, volumeUp ? _maxVolume : _minVolume);
        
    }
}
