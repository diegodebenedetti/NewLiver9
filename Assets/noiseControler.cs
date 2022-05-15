using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class noiseControler : MonoBehaviour
{
    public float noise_cycle_seconds;
    private float noise_cycle_seconds_randomized;
    public float noise_cycle_seconds_randomizer_margin;

    public float min_index;
    public float max_index;
    private float min_index_randomized;
    private float max_index_randomized;
    [Range(0f,0.5f)]
    public float margin;
    public Material wallNoise;

    // Start is called before the first frame update
    void Start()
    {

        min_index_randomized = min_index;
        max_index_randomized = max_index;
        noise_cycle_seconds_randomized = noise_cycle_seconds;
        LeanTween.value(min_index_randomized, max_index_randomized, noise_cycle_seconds_randomized).setOnUpdate((float val)=> {
            wallNoise.SetFloat("_contrast", val);
            
        }).setLoopPingPong().setEaseInOutSine().setOnCompleteOnRepeat(true).setOnComplete(()=> {
            min_index_randomized = min_index+ (Random.Range(-margin, margin));
            max_index_randomized =max_index+(Random.Range(-margin, margin));
            noise_cycle_seconds_randomized = noise_cycle_seconds + Random.Range(0f, noise_cycle_seconds_randomizer_margin);
           
            
        });
    }

    private void OnDestroy()
    {
        LeanTween.cancel(this.gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
