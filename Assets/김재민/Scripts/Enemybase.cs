using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class Enemybase : MonoBehaviourPun
{
    // ###############################################
    //             NAME : KimJaeMin                      
    //             MAIL : woals1566@gmail.com         
    // ###############################################

    // �̵��ӵ�
    protected float moveSpeed;
    // ���ݻ�Ÿ�
    protected float attackRange;
    // ���ݷ�
    protected float Damage;
    // ü��
    protected float HP = 100f;
    //���� ��Ÿ��
    protected float AttackTime;

    public string EnemyTag;



    protected void OnEnable()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            if (PhotonNetwork.LocalPlayer.ActorNumber == 1 && photonView.IsMine)
            {
                gameObject.tag = "Blue";
                EnemyTag = "Red";
            }

            else
            {
                gameObject.tag = "Red";
                EnemyTag = "Blue";
            }
        }

        else
        {
            if (PhotonNetwork.LocalPlayer.ActorNumber == 2 && photonView.IsMine)
            {
                gameObject.tag = "Red";
                EnemyTag = "Blue";
            }
            else
            {
                gameObject.tag = "Blue";
                EnemyTag = "Red";
            }
        }
    }




}
