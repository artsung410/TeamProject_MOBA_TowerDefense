using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamFollower : MonoBehaviour
{
    // ###############################################
    //             NAME : HongSW                      
    //             MAIL : gkenfktm@gmail.com         
    // ###############################################

    #region CAM FOLLOW
    [Header("카메라 고정")]
    public Transform Player;
    private Vector3 _cameraOffset;

    [Range(0.01f, 1)]
    [SerializeField] float FollowSmoothness;
    #endregion

    #region CAM SCROLL
    Camera _cam;

    float _camFOV;
    float _mouseScrollInput;

    [Header("카메라 줌")]
    [SerializeField] float _zoomSpeed;
    [SerializeField] float _minZoomInValue;
    [SerializeField] float _maxZoomOutValue;

    [Range(0.01f, 1)]
    [SerializeField] float ScrollSmoothness;
    #endregion

    #region CAM ROAM
    [Header("자유 시점")]
    [SerializeField] float _camMoveSpeed;
    float _screenSizeThickness = 10f;
    bool CamViewChanged;
    #endregion

    private void Awake()
    {
        _cam = GetComponent<Camera>();
    }

    void Start()
    {
        _cameraOffset = transform.position - Player.position;
        _camFOV = _cam.fieldOfView;

        NumericalInialize();
    }

    private void NumericalInialize()
    {
        // zoom 관련
        _zoomSpeed = 30f;
        _minZoomInValue = 20f;
        _maxZoomOutValue = 60f;
        
        // 스크롤 관련
        ScrollSmoothness = 0.5f;
        FollowSmoothness = 0.5f;

        // edge 이동관련
        _camMoveSpeed = 20f;
        CamViewChanged = false;
    }

    private void Update()
    {
        CameraScroll();


        if (Input.GetKeyDown(KeyCode.Space))
        {
            CamViewChanged = !CamViewChanged;
        }


        if (CamViewChanged)
        {
            //Debug.Log("카메라 이동가능");
            CameraRoam();
        }
        else
        {
            //Debug.Log("카메라 고정");
            CameraFollow();
        }

        
    }

    private void CameraRoam()
    {
        //Vector3 cameraPos = transform.position;

        // UP
        if (Input.mousePosition.y >= Screen.height - _screenSizeThickness)
        {
            transform.Translate(_camMoveSpeed * Time.deltaTime * Vector3.forward, Space.World);
        }

        // DOWN
        if (Input.mousePosition.y <= _screenSizeThickness)
        {
            transform.Translate(_camMoveSpeed * Time.deltaTime * Vector3.back, Space.World);

        }

        // RIGHT
        if (Input.mousePosition.x >= Screen.width - _screenSizeThickness)
        {
            transform.Translate(_camMoveSpeed * Time.deltaTime * Vector3.right, Space.World);

        }

        // LEFT
        if (Input.mousePosition.x <= _screenSizeThickness)
        {
            transform.Translate(_camMoveSpeed * Time.deltaTime * Vector3.left, Space.World);

        }


    }

    private void CameraScroll()
    {
        // 마우스 스크롤값
        _mouseScrollInput = Input.GetAxis("Mouse ScrollWheel");

        // input값에 따라 FOV값 연산
        _camFOV -= _mouseScrollInput * _zoomSpeed;
        // FOV최대 최소 제한하기
        _camFOV = Mathf.Clamp(_camFOV, _minZoomInValue, _maxZoomOutValue);
        // 줌 인/아웃 보간
        _cam.fieldOfView = Mathf.Lerp(_cam.fieldOfView, _camFOV, ScrollSmoothness);

    }

    private void CameraFollow()
    {
        Vector3 newPos = Player.position + _cameraOffset;
        transform.position = Vector3.Slerp(transform.position, newPos, FollowSmoothness);
    }
}
