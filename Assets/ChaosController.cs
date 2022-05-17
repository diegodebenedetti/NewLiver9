using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChaosController : MonoBehaviour
{
    public Material wallNoiseMat;
    public ChaosLevel currentChaosLevel;
    public enum ChaosLevel
    {
        Chaos0,
        Chaos1,
        Chaos2,
        Chaos3,
        Chaos4
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }
    private void Awake()
    {
        currentChaosLevel = ChaosLevel.Chaos0;
    }

    private void setWallNoiseAlphaStr(float alphaStr) {
        wallNoiseMat.SetFloat("_alpha_str",alphaStr*2.5f);
    }

    [ContextMenu("increaseChaosLevel")]
    public void increaseChaosLevel() {
        switch (currentChaosLevel)
        {
            case ChaosLevel.Chaos0:
                setChaosLevel(ChaosLevel.Chaos1);
                break;
            case ChaosLevel.Chaos1:
                setChaosLevel(ChaosLevel.Chaos2);
                break;
            case ChaosLevel.Chaos2:
                setChaosLevel(ChaosLevel.Chaos3);
                break;
            case ChaosLevel.Chaos3:
                setChaosLevel(ChaosLevel.Chaos4);
                break;
            case ChaosLevel.Chaos4:
                
                break;
            default:
                break;
        }
    }

    [ContextMenu("decreaseChaosLevel")]
    public void decreaseChaosLevel()
    {
        switch (currentChaosLevel)
        {
            case ChaosLevel.Chaos0:
               
                break;
            case ChaosLevel.Chaos1:
                setChaosLevel(ChaosLevel.Chaos0);
                break;
            case ChaosLevel.Chaos2:
                setChaosLevel(ChaosLevel.Chaos1);
                break;
            case ChaosLevel.Chaos3:
                setChaosLevel(ChaosLevel.Chaos2);
                break;
            case ChaosLevel.Chaos4:
                setChaosLevel(ChaosLevel.Chaos3);
                break;
            default:
                break;
        }
    }

    public void setChaosLevel(ChaosLevel level) {
        currentChaosLevel = level;


        switch (level)
        {
            case ChaosLevel.Chaos0:
                setWallNoiseAlphaStr(0f);
                break;
            case ChaosLevel.Chaos1:
                setWallNoiseAlphaStr(0.2f);
                break;
            case ChaosLevel.Chaos2:
                setWallNoiseAlphaStr(0.4f);
                break;
            case ChaosLevel.Chaos3:
                setWallNoiseAlphaStr(0.6f);
                break;
            case ChaosLevel.Chaos4:
                setWallNoiseAlphaStr(1f);
                break;
            default:
                break;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
