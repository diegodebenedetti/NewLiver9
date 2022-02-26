using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerMovement : MonoBehaviour
{ 
    [SerializeField] Transform _pivot;
    [SerializeField] Rigidbody _rb;
    [SerializeField] float _walkSpeed, _sprintSpeed, _acceleartion; 
    
    CameraController _camCont;
    Vector3 _dir;
    private bool _canMove;

    void Start()
    {
        if (!_rb) _rb = GetComponent<Rigidbody>();
        if (!_camCont) _camCont = GetComponentInChildren<CameraController>();
        tutorial_helper_method.OnTutorialEnded += OnTutorialEnded;
    }

    private void OnTutorialEnded()
    {
        _canMove = true;
    }

    void Update()
    {
        if(!_canMove) return;
        Movement(_walkSpeed);
    }

    void Movement(float speed)
    {
        if(Input.GetButton("Horizontal") || Input.GetButton("Vertical"))
        { 
            var sideways = _pivot.right * Input.GetAxis("Horizontal");
            var forwards = _pivot.forward * Input.GetAxis("Vertical");
            _dir = Vector3.Lerp(_dir, sideways + forwards, _acceleartion); 

            var velocity = _dir * speed;
            _rb.velocity = new Vector3(velocity.x, 0, velocity.z);
            _camCont.HeadBobbing();
        }
    }
}
