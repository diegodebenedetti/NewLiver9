using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class hand_with_shotgun : MonoBehaviour
{

    public Animator shotgun_animator;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.N))
        {
            shotgun_animator.SetTrigger("shoot");
            Debug.Log("shotgun");
            AudioManager.instance.Play("shotgun");
        }
    }
}
