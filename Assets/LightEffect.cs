using UnityEngine;

[RequireComponent(typeof(Light))]
public class LightEffect : MonoBehaviour
{
    [SerializeField] float _lerp;  
    [SerializeField] Transform objective;
    Light _light;
    void Start()
    {
        TryGetComponent<Light>(out _light);
    }

    // Update is called once per frame
    void Update()
    { 
    }
}
