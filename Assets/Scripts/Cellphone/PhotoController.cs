using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; 
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class PhotoController : MonoBehaviour
{ 
    [SerializeField] Image _img;
    [SerializeField] CellphoneLight _cellphoneLight;
    [SerializeField] float _timeOnScreen, _flashTime, _postExpoFlash, _postExpoNormal;
    [SerializeField] int _resWidth, _resHeight;
    [SerializeField] Texture2D _screenCapture;
    [SerializeField] ColorAdjustments _colorAdj;
    [SerializeField] Volume _volume;
    void Start()
    {
        CellphoneController.OnTakePicture += TakePicture;
        _img.gameObject.SetActive(false);
        _volume?.profile.TryGet(out _colorAdj);
    } 

    public void TakePicture()
    {
        StartCoroutine(TakePictureRoutine());
    }
    private IEnumerator TakePictureRoutine()
    { 
        _cellphoneLight.DoFlash();
        _colorAdj.postExposure.value = _postExpoFlash;
        yield return new WaitForSeconds(_flashTime);
        _colorAdj.postExposure.value = _postExpoNormal;
        _img.gameObject.SetActive(true); 
        yield return new WaitForSeconds(_timeOnScreen);  
        _img.gameObject.SetActive(false);
    }

    void OnDestroy()
    {
         CellphoneController.OnTakePicture -= TakePicture;
    }

      void Screenshot() 
      {  
        // RenderTexture rt = new RenderTexture(_resWidth, _resHeight, 24);
        // GetComponent<Camera>().targetTexture = rt;
        // Texture2D screenShot = new Texture2D(_resWidth, _resHeight, TextureFormat.RGB24, false);
        // GetComponent<Camera>().Render();
        // RenderTexture.active = rt;
        // screenShot.ReadPixels(new Rect(0, 0, _resWidth, resHeight), 0, 0);
        // GetComponent<Camera>().targetTexture = null;
        // RenderTexture.active = null; // JC: added to avoid errors
        // Destroy(rt);
        // byte[] bytes = screenShot.EncodeToPNG();
        // string filename = ScreenShotName(_resWidth, resHeight);
        // System.IO.File.WriteAllBytes(filename, bytes);
        // Debug.Log(string.Format("Took screenshot to: {0}", filename));
        // takeHiResShot = false; 
     }
}
