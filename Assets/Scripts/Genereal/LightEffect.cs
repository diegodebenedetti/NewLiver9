using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Light))]
public class LightEffect : MonoBehaviour
{
    [SerializeField] float _maxTime, _minTime, _intensityMax, _intensityMin, _flickerdurationmin, _flickerdurationmax;   

    Light _light;
    float _timer, _originalIntesity, _enemyScare;
    void Start() 
    { 
        EnemyAI.OnEnemyScareChange += EnemyScare;
        TryGetComponent<Light>(out _light);
        _originalIntesity = _light.intensity;
        _timer = Random.Range(_minTime, _maxTime);
    }

    void EnemyScare(float scare) => _enemyScare = scare;
    void OnEnable() => StartCoroutine(Flicker());
    void OnDisable() => StopAllCoroutines();
     
    void Update()
    {
       if(_timer > 0) 
           _timer-= Time.deltaTime; 
       else
       {
           _timer = Random.Range(_minTime, _maxTime);
           StartCoroutine(Flicker());
       }
    }
    IEnumerator Flicker()
    {
        var flickertime = Random.Range(_flickerdurationmin, _flickerdurationmax);
        while(flickertime > 0)
        { 
            flickertime -= Time.deltaTime;
            _light.intensity = Random.Range(_intensityMin, _intensityMax);
            yield return null;
        }
        _light.intensity = _originalIntesity;
        
    }
  
}
