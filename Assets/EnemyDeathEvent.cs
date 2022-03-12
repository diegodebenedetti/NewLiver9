using System.Collections; 
using UnityEngine;

public class EnemyDeathEvent : MonoBehaviour
{  
    [SerializeField] Light _sunLight;
    [SerializeField] Color _color;
    [SerializeField] float _sunIntesntiy, _time;
    [SerializeField] Material _newSkybox;
 

    void Ending(EnemyState state)
    {  
        if(state == EnemyState.Dead)
        {
            AudioManager.Instance.Stop("Ambient");
            AudioManager.Instance.Play("Ending");
            StartCoroutine(Light()); 
        }
     
    }

    IEnumerator Light()
    { 
        RenderSettings.skybox = _newSkybox; 
        _sunLight.color = _color;
        var timer = 0f;
        while(timer < _time)
        {
            timer += Time.deltaTime;
            _sunLight.intensity = _sunIntesntiy * timer/_time;
        }
        yield return null;
    }
}
