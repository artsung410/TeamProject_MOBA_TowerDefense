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

    // lateUpdate���� ó���Ұ�
    private void LateUpdate()
    {
        // 3Dü�°������� ī�޶� �ٶ󺻴�
        transform.LookAt(_cam.transform);
        transform.Rotate(0, 180, 0);
    }
}
