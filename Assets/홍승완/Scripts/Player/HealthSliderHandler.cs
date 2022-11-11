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

    // lateUpdate���� ó���Ұ�
    private void LateUpdate()
    {
        // ü�� �������� �������̷� �����÷��̾ ���� �ٴѴ�
        if (photonView.IsMine)
        {   
            // ü�¹� ��ġ ������Ʈ
            transform.position = PlayerBehaviour.CurrentPlayerPos + new Vector3(0, 3, 0);
            transform.Rotate(0, 0, 0);
        }

    }

   
}
