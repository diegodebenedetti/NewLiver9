using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Materialize : MonoBehaviour
{ 
    public string MaterialLayer, GhostLayer;
    void Start()
    {
        gameObject.layer = LayerMask.NameToLayer(GhostLayer);
    }

    // Update is called once per frame
    public void ChangeLayer(string newlayer) => gameObject.layer = LayerMask.NameToLayer(newlayer);
}
