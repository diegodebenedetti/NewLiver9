using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class hand_with_phone : MonoBehaviour
{
    public Animator phone_animator;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            phone_animator.SetTrigger("pressbutton");
            Debug.Log("phone");
        }
    }
}
