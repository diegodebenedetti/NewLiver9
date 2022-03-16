using System.Collections;
using System.Collections.Generic;
using Enemy;
using UnityEngine;

public class OnMaterializeFX : MonoBehaviour
{
    EnemyAI _enemyAI;
    public Animator enemyAnimator;
    // Start is called before the first frame update
    void Start()
    {
        EnemyAI.OnStateChange += OnStateChange;
        
    }

    

    private void OnStateChange(EnemyState pState)
    {
        switch (pState)
        {
            
            case EnemyState.Materialized:
                Debug.Log("Materialized");
             
              
                break;
            case EnemyState.Escaping:
             
                break;
            case EnemyState.Dead:
                break;
            default:
                break;
        }
    }
 
}
