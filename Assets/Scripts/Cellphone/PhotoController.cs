using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; 
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using Enemy;

public class PhotoController : MonoBehaviour
{ 
    [SerializeField] Image _img;
    [SerializeField] CellphoneLight _cellphoneLight;
    [SerializeField] Camera _cellphoneCamera; 
    [SerializeField] float _timeOnScreen,_photoTimeOnScreen, _flashTime, _postExpoFlash, _postExpoNormal;
    [SerializeField] int _resWidth, _resHeight; 
    [SerializeField] Volume _volume;
    ColorAdjustments _colorAdj;
    LensDistortion _distorsion;
    EnemyState _enemyState;
    void Start()
    {
        _volume?.profile.TryGet(out _colorAdj);
        _volume?.profile.TryGet(out _distorsion);
        CellphoneController.OnTakePicture += TakePicture;
        EnemyAI.OnStateChange += OnEnemyState;
        _img.gameObject.SetActive(false);
    }

    public void TakePicture() => StartCoroutine(TakePictureRoutine());
    public void OnEnemyState(EnemyState state) => _enemyState = state;
    private IEnumerator TakePictureRoutine()
    {
        _cellphoneLight.DoFlash();
        _colorAdj.postExposure.value = _postExpoFlash;

        yield return new WaitForSeconds(_flashTime);
        _colorAdj.postExposure.value = _postExpoNormal;
        _img.gameObject.SetActive(true);
        yield return new WaitForSeconds(_timeOnScreen);
        _img.gameObject.SetActive(false);

        var defaultRenderTexture = _cellphoneCamera.targetTexture;
        RenderTexture rt = new RenderTexture(_resWidth, _resHeight, 24);
        _cellphoneCamera.targetTexture = rt;
        Texture2D screenShot = new Texture2D(_resWidth, _resHeight, TextureFormat.RGB24, false);  
        _cellphoneCamera.Render();
        yield return new WaitForEndOfFrame();
        screenShot.ReadPixels(new Rect(0, 0, _resWidth, _resHeight), 0, 0);
        screenShot.Apply();
        _cellphoneCamera.targetTexture = defaultRenderTexture;
        Destroy(rt);

        // byte[] bytes = screenShot.EncodeToPNG();
        // string filename = ScreenShotName(_resWidth, _resHeight);
        // System.IO.File.WriteAllBytes(filename, bytes);
        // Debug.Log(string.Format("Took screenshot to: {0}", filename));
         
        _img.gameObject.SetActive(true); 
        _img.color = Color.white;
        _img.material.mainTexture = screenShot;

        yield return new WaitForSeconds(_photoTimeOnScreen); 

        _img.color = Color.black;
        _img.material.mainTexture = null;
        _img.gameObject.SetActive(false); 
    }


    void OnDestroy() => CellphoneController.OnTakePicture -= TakePicture; 
  
   
}
