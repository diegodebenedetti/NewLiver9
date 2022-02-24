using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField] float _damage, _speed, _slowDownRate, _gravity; 
    RaycastHit hit;
    
    void FixedUpdate()
    {
        transform.position = transform.position + (transform.forward * GetDistance() + (Vector3.down * _gravity));

        if(hit.collider)
        {
            if(hit.collider.GetComponent<EnemyAI>())
            {
                Debug.Log("Enemy hit");
            } 
        }
    }
    float GetDistance() =>(_speed - Time.fixedDeltaTime * _slowDownRate) * Time.fixedDeltaTime;
    bool RaycastHit() 
        => Physics.Raycast(transform.position, transform.forward, out hit, 1f);
}
