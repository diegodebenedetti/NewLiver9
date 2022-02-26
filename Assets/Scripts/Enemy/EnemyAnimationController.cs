using UnityEngine;

public class EnemyAnimationController : MonoBehaviour
{

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
    }

    private void OnStateChange(EnemyState pEnemyState)
    {
        switch (pEnemyState)
        {
            case EnemyState.Materialized:
                _anim.speed = 2;
                break;
            case EnemyState.Escaping:
                _anim.speed = 3;
                break;
            case EnemyState.Dead:
                _anim.speed = 0;
                break;
           
        }
    }


}
