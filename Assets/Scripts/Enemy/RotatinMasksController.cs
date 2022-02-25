
using System;
using System.Collections.Generic;
using UnityEngine;

public class RotatinMasksController : MonoBehaviour
{
    [SerializeField]
    private Transform _head;

    [SerializeField]
    private RotatingMask _maskPrefab;

    private Queue<RotatingMask> _masks = new();

    [ContextMenu("Spawn")]
    public void SpawnMask(float pRotationSpeed)
    {
        var maskInstance = Instantiate(_maskPrefab);
        maskInstance.transform.position = _head.transform.position;
        maskInstance.SetCenter(_head);
        maskInstance.SetSpeed(pRotationSpeed);
        _masks.Enqueue(maskInstance);
        
    }
    
    [ContextMenu("Destroy")]
    public void DestroyMask()
    {
        if(_masks.Count == 0) return;
        
        var maskOnScene = _masks.Dequeue();
        maskOnScene.SetMovementEnabled(false);
        maskOnScene.gameObject.SetActive(false);
        Destroy(maskOnScene);

        var maskWithRb = Instantiate(_maskPrefab,maskOnScene.transform.position, maskOnScene.transform.rotation);
        maskWithRb.SetMovementEnabled(false);
        maskWithRb.SetEnableMasks(true);
        maskWithRb.gameObject.AddComponent<Rigidbody>();

    }
}
