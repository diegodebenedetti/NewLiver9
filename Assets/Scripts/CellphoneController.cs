using System;
using System.Linq;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections;

public class CellphoneController : MonoBehaviour
{
    [Header("Cellphone detection")]
    [SerializeField] float _distance;
    [SerializeField] float _radius; 
    [SerializeField] float _detAngle;
    [SerializeField] Transform _detectionPoint; 
    [SerializeField] LayerMask _enemyLayer;

    [Header("Ping Noise")] 
    [SerializeField] AudioController _audioControl;
    [SerializeField] float _pingNoiseMax;
    [SerializeField] float _pingNoiseMin; 
    [SerializeField] float _pingTimeMax;
    [SerializeField] float _pingTimeMin;
    [SerializeField] float _noiseAmount; 
    
    [SerializeField] MeshRenderer render;
    [SerializeField] Image img;
    
    EnemyAI _enemy;
    float _pingNoise;
    float _pingTime;
    float _pingTimer;
    float _enemyScare;
    RaycastHit _hit;
    void Start() => EnemyAI.OnEnemyScareChange += HandleScareChange;

    void OnEnable()
    {
        StartCoroutine(Detect());
    }

    void OnDisable()
    {
        StopAllCoroutines();
    }

    void HandleScareChange(float scareAmount) 
        => _enemyScare = scareAmount;
    void Update()
    { 
    }

    private void DoDetectionEffect()
    {   
        render.material.SetFloat("_NoiseAmount", _distance / EnemyDistance() * _noiseAmount);
        PerformPingNoise(); 
    }

    private void PerformPingNoise()
    {
        _pingNoise =  Mathf.Clamp(_enemyScare /25f,  _pingNoiseMin, _pingNoiseMax) ;
        _pingTime = Mathf.Clamp(25f/_enemyScare, _pingTimeMin, _pingTimeMax); 

        if(_pingTimer < _pingTime)
        {
            _pingTimer += Time.deltaTime;  
        }
        else
        {
            _pingTimer = 0f;
            _audioControl.PlaySound(_pingNoise) ;
        }
    }
    void ResetPingTimer()
    {
        _pingNoise = 0f;
        _pingTime = 0f;
        _pingTimer = 0f;
    } 
    float EnemyDistance() => Vector3.Distance(transform.position, _enemy.transform.position);
    Vector3 EnemyDirection() => _enemy.transform.position - transform.position;
    IEnumerator Detect()
    {
        while(true)
        {
            try
            {
                _enemy = Physics.OverlapSphere(transform.position, _radius, _enemyLayer).FirstOrDefault()?.GetComponent<EnemyAI>();

                if(_enemy)
                { 
                    DoDetectionEffect();
                    if(Vector3.Angle(_detectionPoint.forward, EnemyDirection()) <= _detAngle)  
                    {
                        Debug.Log("Is inside");
                        _enemy.Scare();  
                    }
                    else
                    { 
                        Debug.Log("Not inside");
                        ResetPingTimer();
                    }
                }
                else
                    render.material.SetFloat("_NoiseAmount", 0f);
                Debug.Log("Runnings");
            }
            catch(Exception e)
            { 
                Debug.Log("Error" + e);
            } 
            yield return null; 
        }
        Debug.Log("Exit coroutine");
    }
}
