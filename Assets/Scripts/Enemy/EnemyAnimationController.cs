using Enemy;
using UnityEngine;

public class EnemyAnimationController : MonoBehaviour
{
    
    [SerializeField]
    private enemyCellFracture _explodingEnemyBody;

    [SerializeField]
    private GameObject _enemyBody;
    
    private Animator _anim;

    private readonly int _movementSpeed = Animator.StringToHash("movementSpeed");
    private void Awake()
    {
        _anim = GetComponentInChildren<Animator>();
    }

    void Start()
    {
        EnemyAI.OnStateChange += OnStateChange;
        EnemyAI.OnHitRecieved += OnHitRecieved;
    }

    private void OnHitRecieved()
    {
        _anim.SetTrigger("enemy_recieve_hit");
        AudioManager.Instance.Play("bulletHitsMonster");
    }

    private void OnStateChange(EnemyState pEnemyState)
    {
        switch (pEnemyState)
        {
            case  EnemyState.Materializing:
                _anim.speed = 0;
                break;
            case EnemyState.Materialized:
                _anim.speed = 2;
                break;
            case EnemyState.Escaping:
                _anim.speed = 3;
                break;
            case EnemyState.Dead:
                _anim.speed = 0;
                ExplodeEnemy();
                break;
           
        }
    }

    private void ExplodeEnemy()
    {
        _enemyBody.SetActive(false);
        _explodingEnemyBody.gameObject.SetActive(true);
        _explodingEnemyBody.Explode();
        AudioManager.Instance.Play("monsterexplode");
    }

    public void StopAnimations()
    {
        _anim.speed = 0;
    }
}
