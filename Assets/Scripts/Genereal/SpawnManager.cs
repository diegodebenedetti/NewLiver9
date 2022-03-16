using System.Collections.Generic;
using Core;
using Enemy;
using UnityEngine;
using Random = UnityEngine.Random;

public class SpawnManager : Singleton<SpawnManager>
{
    [Header("Enemy")]
    [SerializeField]
    private EnemyAI _enemy;

    [SerializeField]
    private Location[] _enemySpawnLocations;

    [SerializeField] 
    private Transform _escapeRoute;
  
    
    [Header("Bullets")]
    [SerializeField] private GameObject _bulletsPool;
    [SerializeField] private Bullet _bulletPrefab;
    private Queue<Bullet> _bullets = new Queue<Bullet>();
    
    
    private float ammountOfBullets = 80;
    
    private int _locationAssignedForEnemy;
   
    

    private void Start()
    {
        _locationAssignedForEnemy = 0;
        SendEnemyToRandomSpawnLocation();
        
        for (int i = 0; i < ammountOfBullets; i++)
        {
            var bulletInstance = Instantiate(_bulletPrefab, _bulletsPool.transform);
            _bullets.Enqueue(bulletInstance);
        }
        
    }

    public Bullet InstantiateBullet()
    {
        return _bullets.Dequeue();
    }

    public Vector3 GetRandomRoom()
    {
        var randomLocation = SelectRandomSpawnLocation();
        return randomLocation.position;
    }

    public void ReturnObjectToPool(Bullet pBulletObject)
    {
        pBulletObject.gameObject.transform.SetParent(_bulletsPool.transform);
        _bullets.Enqueue(pBulletObject);
    }

    public void SendEnemyToRandomSpawnLocation()
    {
        var randomLocation = SelectRandomSpawnLocation();
        Teleport(randomLocation.position, _enemy.transform);
        _enemy.SetMovementArea(_enemySpawnLocations[_locationAssignedForEnemy].GetMovementArea);
        
    }

    private void Teleport(Vector3 pPosition, Transform pObject)
    {
        Debug.Log("Sending Object To" + pPosition);
        pObject.gameObject.SetActive(false);
        pObject.position = pPosition;
        pObject.gameObject.SetActive(true);
    }

    private Transform SelectRandomSpawnLocation()
    {
        var randomIndex = 0;
        while (randomIndex == _locationAssignedForEnemy)
        {
            randomIndex = Random.Range(0, _enemySpawnLocations.Length);
        }
        _locationAssignedForEnemy = randomIndex;
        return _enemySpawnLocations[randomIndex].GetSpawnPoint;
    }

    public Transform GetEscapeRoute()
    {
        return _escapeRoute;
    }
    
    
}
