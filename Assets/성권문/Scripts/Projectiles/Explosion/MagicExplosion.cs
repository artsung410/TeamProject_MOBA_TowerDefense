using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
    
    // ###############################################
    //             NAME : ARTSUNG                      
    //             MAIL : artsung410@gmail.com         
    // ###############################################

public class MagicExplosion : MonoBehaviourPun
{
    [Header("Ÿ�� TAG")]
    public string enemyTag;
    public float damage;

    protected void Damage(Transform enemy)
    {
        // �÷��̾� ������ ����
        if (enemy.gameObject.layer == 7)
        {
            Debug.Log("Player ������ ����");
            Health player = enemy.GetComponent<Health>();

            if (player != null && player.gameObject.activeSelf)
            {
                player.OnDamage(damage);
            }
            else
            {
                return;
            }

        }

        // �̴Ͼ� ������ ����
        else if (enemy.gameObject.layer == 8)
        {
            Debug.Log("�̴Ͼ� ������ ����");
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

    protected void OnTriggerEnter(Collider other)
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
