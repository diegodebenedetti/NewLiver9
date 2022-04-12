using System.Collections;
using System.Collections.Generic;
using Enemy;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class OnMaterializeFX : MonoBehaviour
{
    public Volume volume;
    [SerializeField] Color _color;
    [SerializeField] PlayerMovement _movement;
    [SerializeField] CameraController _camera;
    LensDistortion Distorsion;
    ColorAdjustments Adjustments;
    ChromaticAberration Aberration;

    
    // Start is called before the first frame update
    void Start()
    {
        volume.profile.TryGet(out Distorsion);
        volume.profile.TryGet(out Adjustments);
        volume.profile.TryGet(out Aberration);
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
                AudioManager.Instance.Play("Scary");
                _camera?.SetMove(false);
                _movement?.SetMove(false);
                LeanTween.value(0f, 1f, 2f).setOnUpdate((float val) =>
                {
                    Distorsion.intensity.value = val;
                }).setEaseInOutSine().setLoopPingPong(1);  
                LeanTween.value(new GameObject(), Color.white, _color, 2f).setOnUpdate((Color val) =>
                {
                    Adjustments.colorFilter.value = val;
                }).setEaseInOutSine().setLoopPingPong(1); 
                LeanTween.value(0f, 180f, 2f).setOnUpdate((float val) =>
                {
                    Adjustments.saturation.value = val;
                }).setEaseInOutSine().setLoopPingPong(1); 
                LeanTween.value(0f, 1f, 2f).setOnUpdate((float val) =>
                {
                    Aberration.intensity.value = val;
                }).setEaseInOutSine().setLoopPingPong(1); 

                break;
            case EnemyState.Materialized: 
                Debug.Log("Materialized"); 
                _camera?.SetMove(true);
                _movement?.SetMove(true);
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
