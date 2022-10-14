using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthSliderPos : MonoBehaviour
{
    // ###############################################
    //             NAME : HongSW                      
    //             MAIL : gkenfktm@gmail.com         
    // ###############################################

    [SerializeField] Camera _cam;

    // lateUpdate에서 처리할것
    private void LateUpdate()
    {
        // 3D체력게이지는 카메라르 바라본다
        transform.LookAt(_cam.transform);
        transform.Rotate(0, 180, 0);
    }
}
