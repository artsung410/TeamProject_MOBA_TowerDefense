using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class PieceExplosion : MonoBehaviourPun
{
    [Header("Ÿ�� TAG")]
    [HideInInspector]
    public string enemyTag;

    public float damage;
    public int EffectID;

    private void OnEnable()
    {
        // �Ǿƽĺ�
        if (PhotonNetwork.IsMasterClient)
        {
            if (PhotonNetwork.LocalPlayer.ActorNumber == 1 && photonView.IsMine)
            {
                enemyTag = "Red";
            }

            else
            {
                enemyTag = "Blue";
            }
        }

        else
        {
            if (PhotonNetwork.LocalPlayer.ActorNumber == 2 && photonView.IsMine)
            {
                enemyTag = "Blue";
            }

            else
            {
                enemyTag = "Red";
            }
        }
    }
    private void Damage(Transform enemy)
    {
        // �÷��̾� ������ ����
        if (enemy.gameObject.layer == 7)
        {
            Debug.Log("Player ������ ����");
            Health player = enemy.GetComponent<Health>();

            if (player != null && player.gameObject.activeSelf)
            {
                player.OnDamage(damage);
                BuffManager.Instance.AddBuff(CSVtest.Instance.BuffDic[EffectID]);
            }
            else
            {
                return;
            }

        }

        // �̴Ͼ� ������ ����
        else if (enemy.gameObject.layer == 8)
        {
            Debug.Log($"�̴Ͼ� ������ ���� {enemy.gameObject.name}");
            Enemybase minion = enemy.GetComponent<Enemybase>();

            if (minion != null)
            {
                minion.TakeDamage(damage);
            }
        }

        // ����� �̴Ͼ� ������ ����
        else if (enemy.gameObject.layer == 13)
        {
            Debug.Log("����� �̴Ͼ� ������ ����");
            Enemybase special_minion = enemy.GetComponent<Enemybase>();

            if (special_minion != null)
            {
                special_minion.TakeDamage(damage);
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (enemyTag == null)
        {
            return;
        }

        if (!photonView.IsMine)
        {
            return;
        }

        if (other.tag == enemyTag)
        {
            Damage(other.gameObject.transform);
            Debug.Log("���� �ͽ��÷���");
        }
    }
}
