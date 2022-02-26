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
    [Header("Cellphone shake")]
    [SerializeField] Vector3 _shakeDirection;
    [SerializeField] float _shakeTime, _shakeAmplitude;

    [Header("UI")]
    [SerializeField] Image _detector;
    [SerializeField] Image _noiseScreen;
    [SerializeField] TextMeshProUGUI _scareAmt, _enemyCloseness; 
    [SerializeField] GameObject _materializeText;
    [SerializeField] Color _colorHigh, _colorMid, _colorLow, _colorNothing;
    [SerializeField] LightEffect _cellPhoneLight;
    [SerializeField] Inventory _inventory;
    EnemyAI _enemyAI; 
    GameObject _enemy;
    CameraController _cameraController;
    float _pingNoise, _pingTime, _pingTimer, _enemyScare;
    RaycastHit _hit;
    private bool _isEnemyDead;

    void Start()
    {
        _enemy = GameObject.FindObjectOfType<EnemyAI>().gameObject;
        _enemyAI = _enemy?.GetComponent<EnemyAI>();
        _cameraController = GetComponentInParent<CameraController>();
        EnemyAI.OnEnemyScareChange += HandleScareChange;
        EnemyAI.OnStateChange += OnStateChange;
    }

    private void OnStateChange(EnemyState pState)
    {
        switch (pState)
        {
            case EnemyState.Dead:
                _shakeAmplitude = 0;
                _shakeTime = 0;
                _isEnemyDead = true;
                
                break;
        }
    }

    void OnEnable()
    { 
        _cellPhoneLight.gameObject.SetActive(true);
        StartCoroutine(Detect());
    }

    void OnDisable() => StopCoroutine(Detect());
    void HandleScareChange(float scareAmount) 
        => _enemyScare = scareAmount;  
    private void DoDetectionEffect()
    {   
        _noiseScreen.material.SetFloat("_NoiseAmount", _distance / EnemyDistance() * _noiseAmount);
        PerformPingNoise();
        if(!_isEnemyDead)
            ShakeCamera(); 
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
    void ShakeCamera() => _cameraController.Shake(_shakeTime, _shakeDirection, _shakeAmplitude * _enemyScare / 100f);
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
                    {
                        _enemyAI.Scare(); 

                        if(_enemyScare >= 98f )
                        {
                            _materializeText.SetActive(true);
                            if(Input.GetButtonDown("Fire1"))
                            {
                                _cellPhoneLight.DoFlash();
                                _enemyAI.Materialize();  
                            }
                        }
                        else
                        { 
                            _materializeText.SetActive(false);
                        }
                         
                    } 
                    
                    _scareAmt.text = $"{(int)_enemyScare}";
                    _enemyCloseness.text = AngleToEnemy() >= _detAngleLow ? "Ghost near view" :
                                      AngleToEnemy() >= _detAngleHigh ? "Ghost is close" : 
                                      AngleToEnemy() <= _detAngleHigh ? "Ghost in view!" : "...No ghost";
                                    
                    _detector.color = AngleToEnemy() >= _detAngleLow ? _colorLow :
                                      AngleToEnemy() >= _detAngleHigh ? _colorMid : 
                                      AngleToEnemy() <= _detAngleHigh ? _colorHigh : _colorNothing;
                                       

                  


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
