using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class Wand : MonoBehaviourPun
{
    // ###############################################
    //             NAME : HongSW                      
    //             MAIL : gkenfktm@gmail.com         
    // ###############################################

    public PlayerBehaviour _playerScript;
    public Transform EnergyGem;
    public GameObject Bullet;

    GameObject bulletGo;

    #region �ִϸ��̼� �̺�Ʈ ����

    public void FireEnergyBall()
    {
        if (photonView.IsMine)
        {
            // �Ѿ� �߻�
            Debug.Log($"�Ѿ˻���");
            bulletGo = PhotonNetwork.Instantiate(Bullet.name, EnergyGem.transform.position, Quaternion.identity);
            Debug.Log($"�Ѿ˹߻� : {bulletGo.name}");
            if (bulletGo != null)
            {
                // �Ѿ˿� PlayerBehaviour������Ʈ ������ �Ѱ���
                bulletGo.GetComponent<EnergyBall>().GetTargetObject(_playerScript);
                Debug.Log($"�Ѿ��� ��ũ��Ʈ�� �����°�? : {bulletGo.GetComponent<EnergyBall>()}");
            }
        }
    }

    #endregion
}
