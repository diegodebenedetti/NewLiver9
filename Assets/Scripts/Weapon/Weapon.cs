using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

public class Weapon : MonoBehaviour
{

    [SerializeField]
    private GameObject _muzzleFlashLight;

    [SerializeField]
    GameObject _projectile;

    [SerializeField]
    Transform _shootPoint;

    [SerializeField]
    float _pellets, _spread, _rateOfFire, _shakeTime, _shakeAmplitude, _shakeOnEnemyAmplitude;

    [SerializeField]
    UnityEvent _shootEvent;

    [SerializeField]
    Vector3 _shakeDirection;

    private CameraController _cameraController;
    private Animator _anim;
    private float _currentRate;
    private bool _isEnemyDead;

    private void Awake()
    {
        _anim = GetComponent<Animator>();
        _cameraController = GetComponentInParent<CameraController>();       
    }

    void OnEnable() => _currentRate = _rateOfFire;
    private void Start()
    {
        EnemyAI.OnStateChange += OnStateChange;
    }

    private void OnStateChange(EnemyState pEnemyState)
    {
        switch (pEnemyState)
        {
            case EnemyState.Dead:
                _isEnemyDead = true;
                _shakeOnEnemyAmplitude = 0;
                break;
        }
    }

    void Update() => CheckIfCanShoot();

    private void CheckIfCanShoot()
    {
        if (!_isEnemyDead)
        {
        //   _cameraController.Shake(_shakeTime, _shakeDirection, _shakeOnEnemyAmplitude);
        }

        if (_currentRate < _rateOfFire)
            _currentRate += Time.deltaTime;
        else
        {
            if (Input.GetButtonDown("Fire1"))
            {
                Shoot();
                _currentRate = 0f;
            }
        }
    }

    public void Shoot()
    {
        _anim.SetTrigger("shoot");
        _shootEvent.Invoke();
        if(!_isEnemyDead)
            _cameraController.Shake(_shakeTime, _shakeDirection, _shakeAmplitude);
        StartCoroutine(MuzzleFlashLightCoroutine());
        for (int i = 0; i < _pellets; i++)
        {
            var rand = Random.insideUnitSphere * _spread;

            var bullet = SpawnManager.Instance.InstantiateBullet();
            Transform bulletTransform = bullet.transform;
            bulletTransform.localPosition = _shootPoint.position;
            bulletTransform.localRotation = _shootPoint.localRotation;
            bulletTransform.SetParent(null);
            bulletTransform.forward = _shootPoint.transform.forward + rand;

            bullet.gameObject.SetActive(true);
        }

    }

    private IEnumerator MuzzleFlashLightCoroutine()
    {
        _muzzleFlashLight.SetActive(true);
        yield return new WaitForSeconds(0.15f);
        _muzzleFlashLight.SetActive(false);


    }
}