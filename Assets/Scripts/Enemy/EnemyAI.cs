using System;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Serialization;


[RequireComponent(typeof(NavMeshAgent))]
public class EnemyAI : MonoBehaviour
{
    public static event Action<EnemyState> OnStateChange = delegate { };
    public static event Action<float> OnEnemyScareChange = delegate { };
    public static event Action OnEnemyDied = delegate {  };

    [Header("General Dependencies")] 
    [SerializeField]
    private GameObject _player;

    [SerializeField] 
    private GameObject _enemyModel;

    [SerializeField] private BoxCollider _movementArea;

    [Header("Health")] 
    [SerializeField] 
    private float _maxHealth = 100f;


    [Header("On Hiding State")] 
    [SerializeField]
    private float _onHidingPositionChangeTimer;

    [SerializeField] 
    private float _onHidingMovementSpeed;


    [Header("On Scared State")] 
    [SerializeField]
    private float _scareThreshold;

    [SerializeField] 
    private float _onScaredPositionChangeTimer;

    [SerializeField] 
    private float _onScaredMovementSpeed;

    [SerializeField] 
    private float _runDistanceWhenScared;


    [Header("On Materialized State")] 
    [SerializeField]
    private float _materializeThreshold;

    [SerializeField] 
    private float _onMaterializedPositionChangeTimer;

    [SerializeField] 
    private float _onMaterializeddMovementSpeed;

    [Header("On Escape State")] 
    [SerializeField]
    private int[] _escapeThresholds;

    [SerializeField] 
    private float _onEscapeMovementSpeed;


    //Components
    private NavMeshAgent _navmeshAgent;
    private EnemyState _currentState;
    private Transform _myTransform;
    private Transform _playerTransform;

    //ENEMY TREATS
    private bool _canReceiveDamage;
    private bool _canMove;
    private float _currentScareLevel;
    private int _escapeThresholdindex;

    //STATE RELATED
    //Hide
    private bool _isHidingInitialized;
    private float _onHideTimer;

    //Scare
    private bool _canScare;
    private bool _isScaredInitialized;
    private float _onScaredTimer;

    //Materialized
    private bool _canMaterialize;
    private bool _readyForMaterialize;
    private bool _isMaterializedInitialized;
    private float _onMaterializedTimer;

    //Escape
    private bool _canEscape;
    private bool _isEscapingInitialized;
    private float _currentEscapeThreshold;

    //Dead
    private bool _isDead;
    private float _currentHealth;
   


    private void Awake()
    {
        _navmeshAgent = GetComponent<NavMeshAgent>();
        _myTransform = transform;
        _playerTransform = _player.transform;
        _escapeThresholdindex = 0;
        _currentEscapeThreshold = _escapeThresholds[_escapeThresholdindex];
        _currentHealth = _maxHealth;
    }

    private void SendEventOfScareLevel() => OnEnemyScareChange.Invoke(_currentScareLevel);

    private void Update()
    {
        CheckDeath();
        StateUpdate();
        

        DecreaseCellPhoneFocus();
        SendEventOfScareLevel();
    }

    private void CheckDeath()
    {
        if (_currentHealth <= 0 && !_isDead)
        {
            _isDead = true;
            ChangeState(EnemyState.Dead);
            OnEnemyDied.Invoke();
        }
    }

    private void StateUpdate()
    {
        switch (_currentState)
        {
            case EnemyState.Hiding:
                OnHiding();
                break;
            case EnemyState.Scared:
                OnScared();
                break;
            case EnemyState.Materialized:
                OnMaterialized();
                break;
            case EnemyState.Escaping:
                OnEscaping();
                break;
            case EnemyState.Dead:
                OnDead();
                break;
            default:
                OnHiding();
                break;
        }
    }


    #region StateCallbacks

    private void OnHiding()
    {
        if (!_isHidingInitialized)
        {
            InitializeHidingState();
        }

        _onHideTimer += Time.deltaTime;

        if (IsTimeToChangeHidingLocation())
        {
            Action_MoveToRandomPositionInsideMovementArea();
            _onHideTimer = 0;
        }

        if (_canScare)
        {
            ChangeState(EnemyState.Scared);
        }
    }

    private bool IsTimeToChangeHidingLocation()
    {
        return _onHideTimer >= _onHidingPositionChangeTimer;
    }

    private void InitializeHidingState()
    {
        _enemyModel.SetActive(false);
        _canReceiveDamage = false;
        _isHidingInitialized = true;
        _navmeshAgent.speed = _onHidingMovementSpeed;
    }

    private void OnScared()
    {
        if (!_isScaredInitialized)
        {
            InitializeScareState();
        }

        _onScaredTimer += Time.deltaTime;
        if (HasArrivedToDestination())
        {
            Action_MoveToRandomPositionFarAwayFromPlayerInsideMovementArea();
            _onScaredTimer = 0;
        }

        if (_canMaterialize)
        {
            ChangeState(EnemyState.Materialized);
        }
    }

    private bool HasArrivedToDestination()
    {
        bool changeLocation = false;
        if (!_navmeshAgent.pathPending)
        {
            if (_navmeshAgent.remainingDistance <= _navmeshAgent.stoppingDistance)
            {
                if (!_navmeshAgent.hasPath || _navmeshAgent.velocity.sqrMagnitude == 0f)
                    changeLocation = true;
            }
        }

        return changeLocation;

    }

    private void InitializeScareState()
    {
        _canReceiveDamage = false;
        _navmeshAgent.speed = _onScaredMovementSpeed;
        _isScaredInitialized = true;
    }

    private void OnMaterialized()
    {
        if (!_isMaterializedInitialized)
        {
            InitializeMaterializeState();
        }

        _onMaterializedTimer += Time.deltaTime;

        if (IsTimeToChangeMaterializedPosition())
        {
            Action_MoveToRandomPositionFarAwayFromPlayerInsideMovementArea();
          
        }

        if (_currentHealth <= _escapeThresholds[_escapeThresholdindex])
        {
            _escapeThresholdindex++;
            ChangeState(EnemyState.Escaping);
            
        }
    }

    private bool IsTimeToChangeMaterializedPosition()
    {
        return _onMaterializedTimer >= _onMaterializedPositionChangeTimer;
    }

    private void InitializeMaterializeState()
    {
        _enemyModel.SetActive(true);
        _canReceiveDamage = true;
        _navmeshAgent.speed = _onMaterializeddMovementSpeed;
    }

    private void OnEscaping()
    {
        if (!_isEscapingInitialized)
        {
            InitializeEscapingState();
            Escape();
        }

        if (HasArrivedToDestination() || _canEscape)
        {
            SpawnManager.Instance.SendEnemyToRandomSpawnLocation();
            ResetPlayerState();
            ChangeState(EnemyState.Hiding);
        }
    }

    private void InitializeEscapingState()
    {
        _canReceiveDamage = false;
        _navmeshAgent.speed = _onEscapeMovementSpeed;
        _isEscapingInitialized = true;
        _currentHealth = 100;

    }

    private void Escape()
    {
        Vector3 escapeRoute = SpawnManager.Instance.GetEscapeRoute().position;
        _navmeshAgent.SetDestination(escapeRoute);
    }
    
    private void OnDead()
    {
        _enemyModel.SetActive(true);
        _canReceiveDamage = false;
        //Dead Animation
    }

    #endregion

    #region Actions

    private void Action_MoveToRandomPositionInsideMovementArea()
    {
        Vector3 position = Helpers.ChooseRandomPositionInsideCollider(_movementArea);
        _navmeshAgent.SetDestination(position);
    }

    private void Action_MoveToRandomPositionFarAwayFromPlayerInsideMovementArea()
    {
        float distanceToPlayer = 0;
        Vector3 position = Vector3.zero;

        while (distanceToPlayer <= _runDistanceWhenScared)
        {
            position = Helpers.ChooseRandomPositionInsideCollider(_movementArea);
            distanceToPlayer = Vector3.Distance(_playerTransform.position, position);
        }

        _navmeshAgent.SetDestination(position);
    }

    #endregion

    #region TestStates

    [ContextMenu("Hide the Motherfucker")]
    private void SetStateHIding()
    {
        _currentState = EnemyState.Hiding;
    }

    [ContextMenu("Scare the Motherfucker")]
    private void SetStateScared()
    {
        _currentState = EnemyState.Scared;
    }

    [ContextMenu("Materialize the Motherfucker")]
    private void SetStateMaterialized()
    {
        _currentState = EnemyState.Materialized;
    }

    #endregion

    public void Damage(float pAmount)
    {
        if (!_canReceiveDamage) return;
        
        if (_currentHealth <= 0) return;
        
        _currentHealth -= pAmount;
        Debug.Log($"Ouch -{pAmount}");
    }

    public void Scare()
    {
        switch (_currentState)
        {
            case EnemyState.Hiding:

                IncreaseCellPhoneFocusByFactor(8f);
                if (_currentScareLevel >= _scareThreshold)
                    _canScare = true;

                break;
            case EnemyState.Scared:

                IncreaseCellPhoneFocusByFactor(15f);
                if (_currentScareLevel >= _materializeThreshold)
                {
                    _currentScareLevel = _materializeThreshold;
                    _readyForMaterialize = true;
                }

                break;
        }
    }

    public void Materialize()
    {
        if(!_readyForMaterialize) return;

        _canMaterialize = true;
    }

    public void SetMovementArea(BoxCollider pMovementArea)
    {
        _movementArea = pMovementArea;
    }

    private void IncreaseCellPhoneFocusByFactor(float pFactor = 2f)
    {
        _currentScareLevel += Time.deltaTime * pFactor;
    }

    private void DecreaseCellPhoneFocus()
    {
        if (_currentScareLevel <= 0)
            return;

        _currentScareLevel -= Time.deltaTime;
    }

    private void ChangeState(EnemyState pState)
    {
        _currentState = pState;
        OnStateChange(_currentState);
        Debug.Log(_currentState.ToString());
    }

    private void ResetPlayerState()
    {
        _currentScareLevel = 0;

        _canMaterialize = false;
        _canEscape = false;
        _readyForMaterialize = false;
        _canScare = false;

        _isHidingInitialized = false;
        _isScaredInitialized = false;
        _isMaterializedInitialized = false;
        _isEscapingInitialized = false;
    }

    public void SetEscapeReady()
    {
        _canEscape = true;
    }
}