using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemySounds : MonoBehaviour
{ 
    [SerializeField] float _repeatTime;
    float _timer;
    AudioController _audioController;
    NavMeshAgent _agent;
    
    void Start()
    {
        TryGetComponent<NavMeshAgent>(out _agent);
        TryGetComponent<AudioController>(out _audioController);
    }

    // Update is called once per frame
    void Update()
    {
        if(_agent.velocity.magnitude > 0)
        {
            if(_timer > _repeatTime)
            {
                _timer = 0f;
                _audioController.PlaySound(1f);
            }
            else
            {
                _timer += Time.deltaTime;
            }
        }
        else 
            _timer = 0f; 
    }
}
