
using System;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] private float _bulletSpeed;
    private Rigidbody _bulletRb;
    

    private void Awake()
    {
        _bulletRb = GetComponent<Rigidbody>();
        Physics.IgnoreLayerCollision(7,7);

    }

    private void OnEnable()
    {
        _bulletRb.AddForce(transform.forward * _bulletSpeed, ForceMode.VelocityChange);
        
    }

    private void OnCollisionEnter(Collision collision)
    {
        var enemy = collision.collider.GetComponent<EnemyAI>();
        
        if (enemy != null)
        {
            enemy.Damage(1.5f);
            ResetBullet();
            
        }
        
        ResetBullet();
        
    }

    private void ResetBullet()
    {
        _bulletRb.velocity = new Vector3(0f,0f,0f); 
        _bulletRb.angularVelocity = new Vector3(0f,0f,0f);
        gameObject.SetActive(false);
        SpawnManager.Instance.ReturnObjectToPool(this);

    }
}
