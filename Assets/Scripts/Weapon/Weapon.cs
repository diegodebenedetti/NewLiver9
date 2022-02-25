using UnityEngine;
using UnityEngine.Events; 

public class Weapon : MonoBehaviour
{    
    [SerializeField] GameObject _projectile;
    [SerializeField] Transform _shootPoint;
    [SerializeField] float _pellets, _spread,_rateOfFire, _shakeTime, _shakeAmplitude;
    [SerializeField] UnityEvent _shootEvent;
    [SerializeField] Vector3 _shakeDirection;
    private CameraController _cameraController;
    private Animator _anim; 
    private float _currentRate;

    private void Awake()
    {
        _anim = GetComponent<Animator>();
        _cameraController = GetComponentInParent<CameraController>(); 
        _currentRate = _rateOfFire;
    }

    void Update() => CheckIfCanShoot();

    private void CheckIfCanShoot()
    {
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
        _cameraController.Shake(_shakeTime, _shakeDirection, _shakeAmplitude);
        for(int i = 0; i < _pellets ; i++)
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
}
