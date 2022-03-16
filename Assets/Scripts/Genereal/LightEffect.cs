using System.Collections;
using Enemy;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Light))]
public class LightEffect : MonoBehaviour
{
    [Header("Flicker")]
    [SerializeField] float _maxTime;
    [SerializeField] float _minTime, _intensityMax, _intensityMin, _flickerdurationmin, _flickerdurationmax, _scareFactor;
    [Header("Flash")]
    [SerializeField] float _flashInstenity;
    [SerializeField] float _flashTime, _flashDiminshFactor;
    [SerializeField] UnityEvent _flashEvent;   
    [SerializeField] Animator anim;

    Light _light;
    float _timer, _originalIntesity, _enemyScare;
    private void Awake() 
    { 
        EnemyAI.OnEnemyScareChange += EnemyScare;
        TryGetComponent<Light>(out _light);
    }
    void Start() 
    { 
        _originalIntesity = _light.intensity;
        _timer = Random.Range(_minTime, _maxTime);
    }

    bool _doingFlash;
    void EnemyScare(float scare) => _enemyScare = scare;
    void OnEnable() => StartCoroutine(Flicker());
    void OnDisable() => StopAllCoroutines();
     
    void Update()
    {
        if(!_doingFlash)
        {
            if(_timer > 0) 
                _timer-= Time.deltaTime; 
            else
            {
                _timer = Random.Range(_minTime, _maxTime) - (_enemyScare/100f * _scareFactor);
                StartCoroutine(Flicker());
            }
        }
       
    }

    public void DoFlash()
    {
        StartCoroutine(Flash());
        _flashEvent.Invoke();
        anim.SetTrigger("PressButton");
    } 
    IEnumerator Flicker()
    {

        var flickertime = Random.Range(_flickerdurationmin + (_enemyScare/100f * _scareFactor), _flickerdurationmax + (_enemyScare/100f * _scareFactor));
        while(flickertime > 0)
        { 
            flickertime -= Time.deltaTime;
            _light.intensity = Random.Range(_intensityMin + (_enemyScare/100f * _scareFactor), _intensityMax + (_enemyScare/100f * _scareFactor));
            yield return null;
        }
        _light.intensity = _originalIntesity;
        
    }

    IEnumerator Flash()
    {
        _doingFlash = true;
        var timer = 0f;
        
        while(timer < _flashTime)
        {
            timer += Time.deltaTime;
            _light.intensity = _flashInstenity - (_flashDiminshFactor * _flashDiminshFactor/ timer);
            yield return null;
        }
        _light.intensity = _originalIntesity;
        _doingFlash = false;  

    }
  
}
