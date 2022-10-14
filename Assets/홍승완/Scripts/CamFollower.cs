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
    [Header("ī�޶� ����")]
    public Transform Player;
    private Vector3 _cameraOffset;

    [Range(0.01f, 1)]
    [SerializeField] float FollowSmoothness;
    #endregion

    #region CAM SCROLL
    Camera _cam;

    float _camFOV;
    float _mouseScrollInput;

    [Header("ī�޶� ��")]
    [SerializeField] float _zoomSpeed;
    [SerializeField] float _minZoomInValue;
    [SerializeField] float _maxZoomOutValue;

    [Range(0.01f, 1)]
    [SerializeField] float ScrollSmoothness;
    #endregion

    #region CAM ROAM
    [Header("���� ����")]
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
        // zoom ����
        _zoomSpeed = 30f;
        _minZoomInValue = 20f;
        _maxZoomOutValue = 60f;
        
        // ��ũ�� ����
        ScrollSmoothness = 0.5f;
        FollowSmoothness = 0.5f;

        // edge �̵�����
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
            //Debug.Log("ī�޶� �̵�����");
            CameraRoam();
        }
        else
        {
            //Debug.Log("ī�޶� ����");
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
        // ���콺 ��ũ�Ѱ�
        _mouseScrollInput = Input.GetAxis("Mouse ScrollWheel");

        // input���� ���� FOV�� ����
        _camFOV -= _mouseScrollInput * _zoomSpeed;
        // FOV�ִ� �ּ� �����ϱ�
        _camFOV = Mathf.Clamp(_camFOV, _minZoomInValue, _maxZoomOutValue);
        // �� ��/�ƿ� ����
        _cam.fieldOfView = Mathf.Lerp(_cam.fieldOfView, _camFOV, ScrollSmoothness);

    }

    private void CameraFollow()
    {
        Vector3 newPos = Player.position + _cameraOffset;
        transform.position = Vector3.Slerp(transform.position, newPos, FollowSmoothness);
    }
}
