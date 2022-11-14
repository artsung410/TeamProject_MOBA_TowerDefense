using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class MageAnimaionEvent : MonoBehaviourPun
{
    // ###############################################
    //             NAME : HongSW                      
    //             MAIL : gkenfktm@gmail.com         
    // ###############################################

    public PlayerBehaviour _playerScript;
    public Stats _stats;
    public Transform EnergyGem; // ����ü ���� ��ġ
    public GameObject Bullet; // ������ ������

    GameObject bulletGo; // ����ü ���� ��°�

    #region �ִϸ��̼� �̺�Ʈ ����

    public void FireEnergyBall()
    {
        if (photonView.IsMine)
        {
            // �Ѿ� �߻�
            //Debug.Log($"�Ѿ˻���");
            // TODO : �Ѿ��� ������ƮǮ�� �����ϴ°��� ����غ���
            bulletGo = PhotonNetwork.Instantiate(Bullet.name, EnergyGem.transform.position, Quaternion.identity);

            //Debug.Log($"�Ѿ˹߻� : {bulletGo.name}");

            if (bulletGo != null)
            {
                // �Ѿ˿� PlayerBehaviour������Ʈ ������ �Ѱ���
                bulletGo.GetComponent<EnergyBall>().GetTargetObject(_playerScript);
                bulletGo.GetComponent<EnergyBall>().GetStatData(_stats);

                //Debug.Log($"�Ѿ��� ��ũ��Ʈ�� �����°�? : {bulletGo.GetComponent<EnergyBall>()}");
            }
        }
    }

    #endregion
}
