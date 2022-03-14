using System;
using System.Linq;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections;
using Enemy;

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
    [SerializeField] Transform _needlePivot;
    [SerializeField] Image _noiseScreen;   
    [SerializeField] float _needleTimeToReturn;
    [SerializeField] LightEffect _cellPhoneLight; 
    EnemyAI _enemyAI; 
    GameObject _enemy;
    CameraController _cameraController;
    float _pingNoise, _pingTime, _pingTimer, _enemyScare;
    Vector3 _needleOrignialPos;
    RaycastHit _hit;
    private bool _isEnemyDead; 
    void Start()
    {
        _enemy = GameObject.FindObjectOfType<EnemyAI>().gameObject;
        _enemyAI = _enemy?.GetComponent<EnemyAI>();
        _cameraController = GetComponentInParent<CameraController>();
        _needleOrignialPos = _needlePivot.localPosition; 
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
                StartCoroutine(AltFFourToExit());

                break;
        }
    }
    private void Update() 
    { 
        if(Input.GetButtonDown("Fire1"))
        {
            _cellPhoneLight.DoFlash();
            if(_enemyScare >= _enemyAI.MaterializeThreshold)
                _enemyAI.Materialize();  
        } 
        MoveNeedle(_enemyScare);
    }
    private IEnumerator AltFFourToExit()
    {
        yield return new WaitForSeconds(13f);
        // _materializeText.GetComponent<TextMeshProUGUI>().SetText("ALT + F4 to exit");
        // _materializeText.SetActive(true);
    }

    void OnEnable()
    { 
        _cellPhoneLight.gameObject.SetActive(true);
        StartCoroutine(Detect());
    }

    void OnDisable()
    {
        _cellPhoneLight.gameObject.SetActive(false);
        StopCoroutine(Detect());
    }
    void HandleScareChange(float scareAmount) 
        => _enemyScare = Mathf.Abs(scareAmount);  
    private void DoDetectionEffect()
    {   
        _noiseScreen.material.SetFloat("_NoiseAmount", _distance / EnemyDistance() * _noiseAmount);
        PerformPingNoise();
        if(!_isEnemyDead)
            ShakeCamera(); 
    }

    private void PerformPingNoise()
    {
        _pingNoise =  Mathf.Clamp(_enemyScare/_enemyAI.MaterializeThreshold * 4,  _pingNoiseMin, _pingNoiseMax) ;
        _pingTime = Mathf.Clamp(_enemyAI.MaterializeThreshold * 4/_enemyScare, _pingTimeMin, _pingTimeMax); 

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
    void ResetNeedle()
    {
        _pingNoise = 0f;
        _pingTime = 0f;
        _pingTimer = 0f;
        ResetNeedlePosition();
    }
    void MoveNeedle(float rotation) => _needlePivot.localEulerAngles = new Vector3(0,0, 1.8f * -rotation); 
    void ShakeCamera() => _cameraController.Shake(_shakeTime, _shakeDirection, _shakeAmplitude * _enemyScare/_enemyAI.MaterializeThreshold);
    float EnemyDistance() => Vector3.Distance(transform.position, _enemyAI.transform.position);
    Vector3 EnemyDirection() => _enemyAI.transform.position - _detectionPoint.position;
    float AngleToEnemy() => Vector3.Angle(new Vector3(EnemyDirection().x, 0, EnemyDirection().z),new Vector3(_detectionPoint.forward.x,0, _detectionPoint.forward.z));
    
    void Shake()
    {  
        var forward = _needlePivot.forward;  
        var amplitude =  AngleToEnemy() <= _detAngleHigh ? 10 : AngleToEnemy() <= _detAngleLow ? 5 :  AngleToEnemy() >= _detAngleLow ? 0.5f : 0f;
        var Random = UnityEngine.Random.Range(0.1f, 1f) * amplitude;
        _needlePivot.localPosition = (_needleOrignialPos + (forward.normalized * Random)); 
    }
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
                    Shake(); 
                    if(AngleToEnemy() <= _detAngleHigh)   
                        _enemyAI.IncreaseMaterializeFactor();   
                }
                else
                {  
                    _noiseScreen.material.SetFloat("_NoiseAmount", 0f); 
                    ResetNeedle();
                } 
            }
            catch(Exception e)
            { 
                Debug.Log("Error" + e);
            } 
            yield return null; 
        }
    }
    
    IEnumerator ResetNeedlePosition()
    {
        var timer = _needleTimeToReturn;
        while(timer > 0)
        { 
            MoveNeedle(_needlePivot.localEulerAngles.z * timer/_needleTimeToReturn);
            timer -= Time.deltaTime;
            yield return null;
        }
    }
 
}
