using System.Collections; 
using UnityEngine;

public class CameraController : MonoBehaviour
{ 
    [SerializeField] float _sensitivity, _headBobbingSpeed, _maxHeadBobY,_minHeadBobY, _minHeadBobX, _maxHeadBobX;
    [SerializeField] Transform _pivot;
    [SerializeField] AudioController _audioController;
    float _xRot, _yRot, x, y;
    bool _bobUp, _bobleft, _mouseEnable;
    [SerializeField] private bool _canMove;
    public Transform Pivot => _pivot;
    void Start()
    {
        _mouseEnable = false;
        tutorial_helper_method.OnTutorialEnded += OnTutorialEnded;
    }

    private void OnTutorialEnded()
    {
        _canMove = true;
        SetMouse();
    }

    void Update()
    {  
         if(Input.GetKeyDown(KeyCode.Escape))
        {
            _mouseEnable = !_mouseEnable;
            SetMouse();
        }
    } 

    private void SetMouse()
    {
        Cursor.lockState = _mouseEnable ? CursorLockMode.None : CursorLockMode.Locked;
        Cursor.visible = _mouseEnable;
    }

    void LateUpdate()
    {
        if (!_canMove) return;
        moveCamera();
    }
    void moveCamera()
    {    
        var inputx = Input.GetAxis("Mouse Y");
        var inputy = Input.GetAxis("Mouse X"); 
 
        _xRot -= inputx * _sensitivity;
        _yRot += inputy * _sensitivity;
        _xRot = Mathf.Clamp(_xRot, -89.9f, 89.9f);
        _pivot.localEulerAngles = new Vector3(_xRot, _yRot);
    }

    internal void Shake(float time, Vector3 direction, float amplitude)
    {
        StopAllCoroutines();
        StartCoroutine(AnimationCam(time,direction,amplitude));
    }
    
    public void HeadBobbing()
    {
        if(_bobUp)
        {
            if(transform.localPosition.y >= _maxHeadBobY)
            {
                _bobUp = false;
                _audioController.PlaySound(1f);
            }
            else
                y = transform.localPosition.y + Time.deltaTime * _headBobbingSpeed;
        }
        else
        {
             if(transform.localPosition.y <= _minHeadBobY) 
                _bobUp = true;   
            else
                y = transform.localPosition.y - Time.deltaTime * _headBobbingSpeed;
        }

        if(_bobleft)
        {
            if(transform.localPosition.x >= _maxHeadBobX)
                _bobleft = false;
            else
                x = transform.localPosition.x + Time.deltaTime * _headBobbingSpeed;
        }
        else
        {
             if(transform.localPosition.x <= _minHeadBobX)
                _bobleft = true;
            else
                x = transform.localPosition.x - Time.deltaTime * _headBobbingSpeed;
        } 

        transform.localPosition = new Vector3(x,y);
    }
 
    IEnumerator AnimationCam(float time, Vector3 direction, float amplitude)
    {
        var timer = 0f;
        var origin = transform.localPosition;
        while(timer < time)
        {
            timer += Time.deltaTime;
            transform.localPosition = Vector3.Lerp(origin, direction * amplitude, timer/time); 
            transform.localPosition += Random.insideUnitSphere * amplitude;
            yield return null;
        }
        transform.localPosition = origin;

    }

    
}
