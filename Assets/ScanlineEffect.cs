using UnityEngine;
using UnityEngine.UI;

public class ScanlineEffect : MonoBehaviour
{ 
    [SerializeField] Image _img;
    [SerializeField] float _panSpeed;

    void Update()
    {
        _img.material.SetTextureOffset("_Texture2D", new Vector2(0, _img.material.GetTextureOffset("_Texture2D").y + (Time.deltaTime * _panSpeed)));
    }
}
