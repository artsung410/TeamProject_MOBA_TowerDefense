using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.UI;

public class HealthSliderHandler : MonoBehaviourPun
{
    // ###############################################
    //             NAME : HongSW                      
    //             MAIL : gkenfktm@gmail.com         
    // ###############################################

    // lateUpdate에서 처리할것
    private void LateUpdate()
    {
        // 체력 게이지는 일정높이로 로컬플레이어를 따라 다닌다
        if (photonView.IsMine)
        {   
            // 체력바 위치 업데이트
            transform.position = PlayerBehaviour.CurrentPlayerPos + new Vector3(0, 3, 0);
            transform.Rotate(0, 0, 0);
        }

    }

   
}
