using System;
using System.Collections;
using Enemy;
using UnityEngine;
using UnityEngine.Serialization;

public class RotatingMask : MonoBehaviour
{
    [SerializeField]
    private GameObject _maskModel;
    [SerializeField]
    private Vector3 _axis = Vector3.up;
    [SerializeField]
    private Vector3 _desiredPosition;
    [SerializeField]
    private float _radius = 2.0f;
    [SerializeField]
    private float _radiusSpeed = 0.5f;
    [SerializeField]
    private float _rotationSpeed = 80.0f;

    private Transform _myTransform;
    private Transform _center;
    
    private bool _canMove;

    private void Start()
    {
        EnemyAI.OnStateChange += OnStateChange;
    }

    private void OnStateChange(EnemyState pEnemyState)
    {
        switch (pEnemyState)
        {
            case EnemyState.Hiding:
                SetEnableMasks(false);
                break;
            case EnemyState.Scared:
                SetEnableMasks(false);
                break;
            case EnemyState.Materialized:
                SetEnableMasks(true);
                break;
        }
    }

    private void Update()
    {
        if(!_canMove) return;
        
        _myTransform.RotateAround(_center.position, _axis, _rotationSpeed * Time.deltaTime);
        _desiredPosition = (transform.position - _center.position).normalized * _radius + _center.position;
        _myTransform.position = Vector3.MoveTowards(_myTransform.position, _desiredPosition, Time.deltaTime * _radiusSpeed);
        
    }

    public void SetCenter(Transform pCenter)
    {
        _center = pCenter;
        _myTransform = transform;
        
        _myTransform.position = (_myTransform.position - _center.position).normalized * _radius + _center.position;
        _canMove = true;
    }

    public void SetSpeed(float pSpeed)
    {
        _rotationSpeed = pSpeed;
    }

    public void SetMovementEnabled(bool pMovementEnabeled)
    {
        _canMove = pMovementEnabeled;
    }
    
    public void SetEnableMasks(bool pEnable)
    {
        _maskModel.SetActive(pEnable);
    }

    private void OnDestroy()
    {
        EnemyAI.OnStateChange -= OnStateChange;
    }
}
