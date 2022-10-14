using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthSliderPos : MonoBehaviour
{
    // ###############################################
    //             NAME : HongSW                      
    //             MAIL : gkenfktm@gmail.com         
    // ###############################################


    // lateUpdate에서 처리할것
    private void LateUpdate()
    {
        transform.LookAt(Camera.main.transform);
        transform.Rotate(0, 180, 0);
    }
}
