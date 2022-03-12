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

        if (pState==EnemyState.Materialized)
        {
            Debug.Log("materialized alan");
        }
    }
        // Update is called once per frame
        void Update()
    {
        
    }
}
