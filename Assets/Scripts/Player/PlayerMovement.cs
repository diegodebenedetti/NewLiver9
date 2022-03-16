using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerMovement : MonoBehaviour
{ 
    [SerializeField] Transform _pivot;
    [SerializeField] float _walkSpeed, _sprintSpeed, _acceleartion, _groundDetectHeight; 
     
    [Header("Footsteps")] 
    [SerializeField] List<AudioClipList> _lists;
    [SerializeField] AudioController _audioController;
    [SerializeField] string _prefix;
    CharacterController _ch;
    CameraController _camCont;
    Vector3 _dir;
    RaycastHit _hit;
    string _currentFloorTag;
    [SerializeField] private bool _canMove;

    [Serializable] 
    internal class AudioClipList
    {
        public List<AudioClip> List;
        public string Name;
    }
    void Start()
    {
        _currentFloorTag = "Floor/Dirt";
        ChangeSoundList();
        if (!_ch) _ch = GetComponent<CharacterController>();
        if (!_camCont) _camCont = GetComponentInChildren<CameraController>();
        tutorial_helper_method.OnTutorialEnded += OnTutorialEnded;
    }

    private void OnTutorialEnded() => _canMove = true;

    void Update()
    {
        if(!_canMove) return;
        Movement(_walkSpeed);
        DetectGround();
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

    void DetectGround()
    {
        Physics.Raycast(transform.position, transform.up * -1, out _hit, _groundDetectHeight);
        if(_hit.collider != null && !_hit.collider.CompareTag(_currentFloorTag))
        { 
            _currentFloorTag = _hit.collider.tag;
            ChangeSoundList();
        }
    }

    void OnDrawGizmos()
    {
        Gizmos.DrawRay(transform.position, transform.up * -1);
    }
    public void ChangeSoundList() => _audioController.ChangeList(_lists.FirstOrDefault(x => _prefix + x.Name == _currentFloorTag)?.List);
}
