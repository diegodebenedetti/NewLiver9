using System.Collections;
using System.Collections.Generic;
using Enemy;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class OnMaterializeFX : MonoBehaviour
{
    public Volume volume;
    LensDistortion Distorsion;
    // Start is called before the first frame update
    void Start()
    {
        volume.profile.TryGet(out Distorsion);
        EnemyAI.OnStateChange += OnStateChange;
        
    }

    

    private void OnStateChange(EnemyState pState)
    {
        switch (pState)
        {
            case EnemyState.Hiding:
                break;
            case EnemyState.Scared:
                break;
            case EnemyState.Materializing:
                Debug.Log("Materializing");
                AudioManager.Instance.Play("monster7");
                AudioManager.Instance.Play("materializedAmbient");

                LeanTween.value(0f, 1f, 0.5f).setOnUpdate((float val) =>
                {
                    Distorsion.intensity.value = val;
                }).setEaseInOutSine().setLoopPingPong(1);

                break;
            case EnemyState.Materialized:
                Debug.Log("Materialized");
                AudioManager.Instance.Play("heartbeat");
              

                break;
            case EnemyState.Escaping:
                AudioManager.Instance.Stop("heartbeat");
                AudioManager.Instance.Stop("materializedAmbient");
                break;
            case EnemyState.Dead:
                break;
            default:
                break;
        }
    }

  
   
}
