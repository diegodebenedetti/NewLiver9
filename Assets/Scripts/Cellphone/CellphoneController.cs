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
    [SerializeField] float _detAngleHigh, _detAngleLow;
    [SerializeField] Transform _detectionPoint; 
    [SerializeField] LayerMask _enemyLayer;

    [Header("Ping Noise")] 
    [SerializeField] AudioController _audioControl;
    [SerializeField] float _pingNoiseMax;
    [SerializeField] float _pingNoiseMin; 
    [SerializeField] float _pingTimeMax;
    [SerializeField] float _pingTimeMin;
    [SerializeField] float _noiseAmount; 
    
    [Header("UI")]
    [SerializeField] Image _detector;
    [SerializeField] Image _noiseScreen;
    [SerializeField] TextMeshProUGUI _scareAmt, _enemyCloseness; 
    [SerializeField] Color _colorHigh, _colorMid, _colorLow, _colorNothing;
    [SerializeField] GameObject _cellPhoneLight;
    [SerializeField] Inventory _inventory;
    EnemyAI _enemyAI; 
    GameObject _enemy;
    float _pingNoise;
    float _pingTime;
    float _pingTimer;
    float _enemyScare;
    RaycastHit _hit;
    void Start()
    {
        _enemy = GameObject.FindObjectOfType<EnemyAI>().gameObject;
        _enemyAI = _enemy?.GetComponent<EnemyAI>();
        EnemyAI.OnEnemyScareChange += HandleScareChange; 
    }

    void OnEnable()
    { 
        _cellPhoneLight.SetActive(true);
        StartCoroutine(Detect());
    }

    void OnDisable() 
    { 
        _cellPhoneLight.SetActive(false);
        StopCoroutine(Detect());
    }
    void HandleScareChange(float scareAmount) 
        => _enemyScare = scareAmount;  
    private void DoDetectionEffect()
    {   
        _noiseScreen.material.SetFloat("_NoiseAmount", _distance / EnemyDistance() * _noiseAmount);
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
    float EnemyDistance() => Vector3.Distance(transform.position, _enemyAI.transform.position);
    Vector3 EnemyDirection() => _enemyAI.transform.position - _detectionPoint.position;
    float AngleToEnemy() => Vector3.Angle(new Vector3(EnemyDirection().x, 0, EnemyDirection().z),new Vector3(_detectionPoint.forward.x,0, _detectionPoint.forward.z));
    IEnumerator Detect()
    {
        while(true)
        {
            GameObject enemy;
            try
            {
                enemy = Physics.OverlapSphere(transform.position, _radius, _enemyLayer).FirstOrDefault(x => x.gameObject == _enemy)?.gameObject;
    
                if(enemy)
                {  
                    DoDetectionEffect(); 
                    if(AngleToEnemy() <= _detAngleHigh)   
                        _enemyAI.Scare();   
                    
                    _scareAmt.text = $"{(int)_enemyScare}";
                    _enemyCloseness.text = AngleToEnemy() >= _detAngleLow ? "Ghost near view" :
                                      AngleToEnemy() >= _detAngleHigh ? "Ghost is close" : 
                                      AngleToEnemy() <= _detAngleHigh ? "Ghost in view!" : "...No ghost";
                                    
                    _detector.color = AngleToEnemy() >= _detAngleLow ? _colorLow :
                                      AngleToEnemy() >= _detAngleHigh ? _colorMid : 
                                      AngleToEnemy() <= _detAngleHigh ? _colorHigh : _colorNothing;
                                       
                    if(_enemyScare <= 100f && Input.GetButtonDown("Fire1"))
                         _enemyAI.Materialize(); 
                }
                else
                {
                    _scareAmt.text = ""; 
                    _enemyCloseness.text = "...No ghost";
                    _detector.color = _colorNothing;
                    _noiseScreen.material.SetFloat("_NoiseAmount", 0f); 

                    ResetPingTimer();
                } 
            }
            catch(Exception e)
            { 
                Debug.Log("Error" + e);
            } 
            yield return null; 
        }
    }
}
