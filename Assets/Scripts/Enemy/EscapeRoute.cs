
using Enemy;
using UnityEngine;

public class EscapeRoute : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        var enemy = other.GetComponent<EnemyAI>();
        if (enemy != null)
        {
            enemy.SetEscapeReady();
        }
    }
}
