using UnityEngine;

public class Location : MonoBehaviour
{
    [SerializeField] private Transform _spawnPoint;
    [SerializeField] private BoxCollider _movmentArea;

    public Transform GetSpawnPoint => _spawnPoint;
    public BoxCollider GetMovementArea => _movmentArea;

}