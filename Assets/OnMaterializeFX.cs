using System.Collections;
using System.Collections.Generic;
using Enemy;
using UnityEngine;

public class OnMaterializeFX : MonoBehaviour
{
    EnemyAI _enemyAI;
    // Start is called before the first frame update
    void Start()
    {
        EnemyAI.OnStateChange += OnStateChange;
    }

    private void OnStateChange(EnemyState pState)
    {
        switch (pState)
        {
            case EnemyState.Hiding:
                break;
            case EnemyState.Scared:
                break;
            case EnemyState.Materialized:
                //AudioManager.Instance.Play("monsterMaterialize");
                Debug.Log("materialized alan");

                break;
            case EnemyState.Escaping:
                break;
            case EnemyState.Dead:
                break;
            default:
                break;
        }
       
    }
        // Update is called once per frame
        void Update()
    {
        
    }
}
