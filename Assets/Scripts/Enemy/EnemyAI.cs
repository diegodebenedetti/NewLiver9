using System;
using System.Collections;
using Enemy.EnemyStateMachine;
using UnityEngine;

namespace Enemy
{
    public class EnemyAI : EnemyAIStateMachine
    {
        public static event Action<EnemyState> OnStateChange = delegate { };
        public static event Action<float> OnEnemyScareChange = delegate { };
        public static event Action OnHitRecieved = delegate { };

        [Header("General Dependencies")]
        [SerializeField]
        private GameObject _player;

        [SerializeField]
        internal GameObject _enemyModel;

        [SerializeField]
        internal BoxCollider _movementArea;

        [Header("Health")]
        [SerializeField]
        private float _maxHealth = 100f;

        [Header("On Hiding State")]
        [SerializeField]
        internal float _onHidingPositionChangeTimer;

        [Header("On Scared State")]
        [SerializeField]
        internal float _scareThreshold;

        [SerializeField]
        private float _onScaredPositionChangeTimer;

        [SerializeField]
        internal float _runDistanceWhenScared;

        [Header("On Materializing State")]
        [SerializeField]
        internal float _onMaterializingRoutineDuration;

        [Header("On Materialized State")]
        [SerializeField]
        internal float _changeRoomThreshold;

        [SerializeField]
        internal float _materializeThreshold;

        public float MaterializeThreshold => _materializeThreshold;

        [SerializeField]
        private float _onMaterializedPositionChangeTimer;

        [Header("On Escape State")]
        [SerializeField]
        internal int[] _escapeThresholds;


        //Components
        internal EnemyMovementController _enemyMovementController;
        private EnemyAnimationController _enemyAnimationController;
        private EnemyState _currentState;
        public EnemyState CurrentState => _currentState;
        internal Transform _playerTransform;
        internal RotatinMasksController _masksController;


        //ENEMY TREATS
        internal bool _canReceiveDamage;
        private bool _canMove;
        internal float _currentScareLevel;
        internal int _escapeThresholdindex;

        //STATE RELATED
        //Hide
        protected internal float _onHideTimer;

        //Scare
        internal bool _canScare;
        internal float _onScaredTimer;

        //Materializing
        internal float _onMaterializingTimer;

        //Materialized
        internal bool _canMaterialize;
        internal bool _readyForMaterialize;
        internal float _onMaterializedTimer;


        //Escape
        internal bool _canEscape;

        //Dead
        private bool _isDead;
        internal float _currentHealth;


        private void Awake()
        {
            _enemyMovementController = GetComponent<EnemyMovementController>();
            _enemyAnimationController = GetComponent<EnemyAnimationController>();

            _masksController = GetComponent<RotatinMasksController>();
            ;
            _playerTransform = _player.transform;
            _escapeThresholdindex = 0;
            _currentHealth = _maxHealth;
        }

        private void Start()
        {

            //ChangeState(EnemyState.Hiding);
            SetState(new HidingState(this));
            SpawnMasks();
        }


        private void Update()
        {

            CurrentAIState.RunState();
            CheckDeath();

            DecreaseCellPhoneFocus();
            SendEventOfScareLevel();
        }

        private void CheckDeath()
        {
            if (_currentHealth <= 0 && !_isDead)
            {
                _isDead = true;
                SetState(new DeadState(this));
            }
        }

        #region Actions

        internal Vector3 Action_SelectRandomPositionFarAwayFromPlayerInsideMovementArea()
        {
            float distanceToPlayer = 0;
            Vector3 position = Vector3.zero;

            while (distanceToPlayer <= _runDistanceWhenScared)
            {
                position = Helpers.ChooseRandomPositionInsideCollider(_movementArea);
                distanceToPlayer = Vector3.Distance(_playerTransform.position, position);
            }

            return position;
        }

        #endregion
        #region Public Methods

        public void SetEscapeReady()
        {
            _canEscape = true;
        }

        public void Damage(float pAmount)
        {
            if (!_canReceiveDamage) return;

            if (_currentHealth <= 0) return;

            _currentHealth -= pAmount;
            var ouchindex = UnityEngine.Random.Range(0, 2);
            AudioManager.Instance.Play($"Ouch{ouchindex}");
            OnHitRecieved.Invoke();
        }

        public void IncreaseMaterializeFactor()
        {
            CurrentAIState.InteractWithCellPhone();
        }

        public void Materialize()
        {
            if (!_readyForMaterialize) return;

            _canMaterialize = true;
        }

        public void SetMovementArea(BoxCollider pMovementArea)
        {
            _movementArea = pMovementArea;
        }

        #endregion
        #region Private Methods

        internal void IncreaseCellPhoneFocusByFactor(float pFactor = 2f)
        {
            _currentScareLevel += Time.deltaTime * pFactor;
        }

        private void DecreaseCellPhoneFocus()
        {
            if (_currentScareLevel <= 0)
                return;

            _currentScareLevel -= Time.deltaTime;
        }

        private void SendEventOfScareLevel() => OnEnemyScareChange.Invoke(Mathf.Clamp(_currentScareLevel, 0, 100));

        internal void NotifyStateChange(EnemyState pState)
        {
            OnStateChange(pState);
            _enemyMovementController.SetEnemySpeed(pState);
            _currentState = pState;
            Debug.Log(pState.ToString());
        }

        internal void ResetEnemyStats()
        {
            _currentScareLevel = 0;
            _canMaterialize = false;
            _canEscape = false;
            _readyForMaterialize = false;
            _canScare = false;
        }

        private void SpawnMasks()
        {
            for (int i = 0; i < _escapeThresholds.Length - 1; i++)
            {
                if (i % 2 == 0)
                {
                    _masksController.SpawnRotatingMask(100);
                }
                else
                {
                    _masksController.SpawnRotatingMask(-100);
                }
            }
        }

        #endregion

    }
}