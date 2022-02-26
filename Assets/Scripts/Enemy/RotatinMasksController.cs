using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class RotatinMasksController : MonoBehaviour
{

    [Header("Main Mask")]
    [SerializeField]
    private GameObject _mainMask;
    [SerializeField]
    private GameObject _mainRbMask;
    
    [Header("Rotating Masks")]
    [SerializeField]
    private Transform _head;

    [FormerlySerializedAs("_maskPrefab")]
    [SerializeField]
    private RotatingMask _flotatingMask;

    [SerializeField]
    private GameObject _maskWithPhysics;

    private Queue<RotatingMask> _masks = new();

    [ContextMenu("Spawn")]
    public void SpawnRotatingMask(float pRotationSpeed)
    {
        var maskInstance = Instantiate(_flotatingMask);
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

        Instantiate(_maskWithPhysics,maskOnScene.transform.position, maskOnScene.transform.rotation);

    }

    public void MainMaskFinale()
    {
        StartCoroutine(MainMaskFinaleCoroutine());
    }

    private IEnumerator MainMaskFinaleCoroutine()
    {
        var maskRbInstance = Instantiate(_mainRbMask, _head.transform);
        _mainMask.SetActive(false);
        yield return new WaitForSeconds(3f);
        maskRbInstance.GetComponent<Rigidbody>().isKinematic = false;
        maskRbInstance.GetComponent<BoxCollider>().enabled = true;



    }
}
