using System.Collections; 
using UnityEngine;
using UnityEngine.Events;

public class EnemyDeathEvent : MonoBehaviour
{  
    [SerializeField] AudioSource _source;
    [SerializeField] AudioClip _clip;
    [SerializeField] Light _sunLight;
    [SerializeField] Color _color;
    [SerializeField] float _sunIntesntiy, _time;
    [SerializeField] Material _newSkybox;
    [SerializeField] UnityEvent _event;

    void Start() => EnemyAI.OnStateChange += Ending;

    void Ending(EnemyState state)
    {  
        if(state == EnemyState.Dead)
        { 
            _event.Invoke();
            _source.clip = _clip;
            _source.Play();
            StartCoroutine(Light()); 
        }
     
    }

    IEnumerator Light()
    { 
        RenderSettings.skybox = _newSkybox; 
        _sunLight.color = _color;
        var timer = 0f;
        while(timer < _time)
        {
            timer += Time.deltaTime;
            _sunLight.intensity = _sunIntesntiy * timer/_time;
        }
        yield return null;
    }
}
