using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class hand_with_shotgun : MonoBehaviour
{

    public Animator shotgun_animator;
 
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.N))
        {
            shotgun_animator.SetTrigger("shoot");
            Debug.Log("shotgun");
            AudioManager.Instance.Play("shotgun");
        }
    }
}
