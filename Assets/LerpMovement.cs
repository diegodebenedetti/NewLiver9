using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LerpMovement : MonoBehaviour
{
    [SerializeField] Transform _objective; 
    [SerializeField] float _lerpPerc, intensity, smoothing;
    
    void FixedUpdate()
    {        
        transform.localRotation = Quaternion.Lerp(transform.localRotation, _objective.rotation,_lerpPerc);
    }
}
