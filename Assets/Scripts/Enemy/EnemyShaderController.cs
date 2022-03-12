using System;
using System.Collections;
using System.Collections.Generic;
using Enemy;
using UnityEngine;
using UnityEngine.Serialization;

public class EnemyShaderController : MonoBehaviour
{
    private readonly int _dissolve = Shader.PropertyToID("_Dissolve");

    [FormerlySerializedAs("_material")]
    [SerializeField]
    private Material _bodyMaterial;

    [SerializeField]
    private Material _maskMaterial;

    [SerializeField]
    private float _dissolveSpeed;

    private float _currentDissolveAmount;

    private void Start()
    {
        EnemyAI.OnStateChange += OnStateChange;
    }

    private void OnStateChange(EnemyState pEnemyState)
    {
        switch (pEnemyState)
        {
            case EnemyState.Hiding:
                ResetDissolve();
                break;
            case EnemyState.Scared:
                break;
            case EnemyState.Materialized:
                break;
            case EnemyState.Escaping:
                StartCoroutine(Dissolve(_dissolveSpeed));
                break;
            case EnemyState.Dead:
                break;
        }
    }

    private void ResetDissolve()
    {
        _currentDissolveAmount = 0;
        _bodyMaterial.SetFloat(_dissolve, _currentDissolveAmount);
        _maskMaterial.SetFloat(_dissolve, _currentDissolveAmount);
    }
    
    private IEnumerator Dissolve(float pTime)
    {
        float time = 0;
        float dissolveValue;
        while (time <= pTime)
        {
            time += Time.deltaTime;
            dissolveValue = Mathf.Lerp(0, 1, time / pTime);
            _bodyMaterial.SetFloat(_dissolve, dissolveValue);
            _maskMaterial.SetFloat(_dissolve, dissolveValue);
            yield return null;
        }
        
    }

}
