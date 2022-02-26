using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemyCellFracture : MonoBehaviour
{
    public Component[] Rigidbodys;
    public float min_explosionForce;
    public float max_explosionForce;
    public float radius;
    // Start is called before the first frame update
    void Start()
    {
        Rigidbodys = GetComponentsInChildren<Rigidbody>();
        explodeEnemy();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void explodeEnemy() {
        foreach (Rigidbody rb in Rigidbodys)
        {
            rb.AddExplosionForce(Random.Range(min_explosionForce,max_explosionForce), transform.position, radius);
        }
    }
}
