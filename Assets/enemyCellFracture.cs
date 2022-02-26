using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemyCellFracture : MonoBehaviour
{
    public Component[] Rigidbodys;
    public float min_explosionForce;
    public float max_explosionForce;
    public float radius;
    
    public void Explode() {
        foreach (Rigidbody rb in Rigidbodys)
        {
            rb.AddExplosionForce(Random.Range(min_explosionForce,max_explosionForce), transform.position, radius);
        }
    }
}
