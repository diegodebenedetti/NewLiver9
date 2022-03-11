using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerMovement : MonoBehaviour
{ 
    [SerializeField] Transform _pivot;
    [SerializeField] float _walkSpeed, _sprintSpeed, _acceleartion; 
    
    CharacterController _ch;
    CameraController _camCont;
    Vector3 _dir;
    private bool _canMove;

    void Start()
    {
        if (!_ch) _ch = GetComponent<CharacterController>();
        if (!_camCont) _camCont = GetComponentInChildren<CameraController>();
        tutorial_helper_method.OnTutorialEnded += OnTutorialEnded;
    }

    private void OnTutorialEnded() => _canMove = true;

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
            _dir = Vector3.Lerp(_dir, sideways + forwards + Physics.gravity, _acceleartion); 

            var velocity = _dir * speed * Time.deltaTime;
            _ch.Move(velocity);
            _camCont.HeadBobbing();
        }
    }
}
