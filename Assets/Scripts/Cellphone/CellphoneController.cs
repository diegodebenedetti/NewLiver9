using System.Runtime.CompilerServices;
using System.Threading.Tasks.Dataflow;
using System.Transactions;
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
    float _pingNoise, _pingTime, _pingTimer, _enemyScare, timer;
    Vector3 _needleOrignialPos; 
    Ray ray;
    private bool _isEnemyDead, _isEnemyEscaping, _isEnemyInView, _isEnemyInsideRange; 
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
            case EnemyState.Hiding: 
                _shakeAmplitude = 0;
                _shakeTime = 0; 
                timer = _needleTimeToReturn;
                _isEnemyEscaping = true;
                ResetNeedle();
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
        StartCoroutine(IsEnemyInFront());
    }

    void OnDisable()
    {
        _cellPhoneLight.gameObject.SetActive(false);
        StopAllCoroutines();
    }
    void HandleScareChange(float scareAmount) 
    { 
        _enemyScare = Mathf.Abs(scareAmount); 
        if(!_isEnemyEscaping)
            MoveNeedle(_enemyScare);
        else
        {
            if(timer > 0)
            {
                timer -= Time.deltaTime;
                MoveNeedle(_needlePivot.localEulerAngles.z * timer/_needleTimeToReturn);
            }
            else   
                _isEnemyEscaping = false;
        }
    }
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
            try
            {
                _isEnemyInsideRange = Physics.OverlapSphere(transform.position, _radius, _enemyLayer).FirstOrDefault(x => x.gameObject == _enemy)?.gameObject != null;
                

                if(_isEnemyInsideRange)
                {  
                    DoDetectionEffect();  
                    Shake(); 
                    if(AngleToEnemy() <= _detAngleHigh && _isEnemyInView)   
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

    IEnumerator IsEnemyInFront()
    {
        RaycastHit hit;
        while(true)
        { 
            for(float i = -_detAngleHigh; i < _detAngleHigh; i++)
            {
                var angle = ((_detectionPoint.forward * Mathf.Cos(i  * Mathf.Deg2Rad)) + _detectionPoint.right * Mathf.Sin(i  * Mathf.Deg2Rad)).normalized;
                ray = new Ray(_detectionPoint.position, angle);
                if(Physics.Raycast(_detectionPoint.position, angle, out hit, _distance) && hit.collider.CompareTag("Enemy"))
                    _isEnemyInView = true;
                else
                    _isEnemyInView = false;
                i += 1f; 
            } 
        yield return null;
        } 
    }

    void OnDrawGizmos() => Gizmos.DrawRay(ray);
}
