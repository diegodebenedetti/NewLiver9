using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
 
public class DebugText : MonoBehaviour
{ 
    public static DebugText instance {get; set;}
    public TextMeshProUGUI text; 
    
    void Awake()
    {
        if (instance != null && instance != this) 
            Destroy(this); 
        else 
            instance = this; 
    }


}
