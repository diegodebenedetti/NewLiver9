using System;
using UnityEngine;
using UnityEngine.AI;

namespace Enemy
{
    [RequireComponent(typeof(NavMeshAgent))]
    public class EnemyMovementController : MonoBehaviour
    {
        [Header("Hiding")]
        [SerializeField]
        private float _runSpeedWhenHiding;
        
        [Header("Scared")]
        [SerializeField]
        private float _runSpeedWhenScared;
        
        [Header("Materialized")]
        [SerializeField]
        private float _runSpeedWhenMaterialized;
        
        [Header("Escaping")]
        [SerializeField]
        private float _runSpeedWhenEscaping;
        
        private NavMeshAgent _navmeshAgent;
        private Transform _myTransform;
      
    
        void Awake()
        {
            _navmeshAgent = GetComponent<NavMeshAgent>();
            _myTransform = this.transform;
        }

        public void SetEnemyDestination(Vector3 pPosition)
        {
            _navmeshAgent.SetDestination(pPosition);
        }
        
        public void SetEnemySpeed(EnemyState pState)
        {
            switch (pState)
            {
                case EnemyState.Hiding:
                    _navmeshAgent.speed = _runSpeedWhenHiding;
                    break;
                case EnemyState.Scared:
                    _navmeshAgent.speed = _runSpeedWhenScared;
                    break;
                case EnemyState.Materialized:
                    _navmeshAgent.speed = _runSpeedWhenMaterialized;
                    break;
                case EnemyState.Escaping:
                    _navmeshAgent.speed = _runSpeedWhenEscaping;
                    break;
                case EnemyState.Dead:
                   StopEnemyMovement();
                    break;
            }
        }
        
        public bool HasArrivedToDestination()
        {
            bool hasArrived = false;
            
            if (!_navmeshAgent.pathPending)
            {
                if (_navmeshAgent.remainingDistance <= _navmeshAgent.stoppingDistance)
                {
                    if (!_navmeshAgent.hasPath || _navmeshAgent.velocity.sqrMagnitude == 0f)
                        hasArrived = true;
                }
            }

            return hasArrived;

        }
        
        private void StopEnemyMovement()
        {
            _navmeshAgent.speed = 0;
            _navmeshAgent.isStopped = true;
        }


    }
}
