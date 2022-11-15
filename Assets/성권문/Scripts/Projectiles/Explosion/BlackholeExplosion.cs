using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class BlackholeExplosion : MonoBehaviourPun
{
    // ###############################################
    //             NAME : ARTSUNG                      
    //             MAIL : artsung410@gmail.com         
    // ###############################################

    [Header("Ÿ�� TAG")]
    [HideInInspector]
    public string enemyTag;

    [HideInInspector]
    public float damage;
    private void OnEnable()
    {
        StartCoroutine(Destruction());
    }

    private IEnumerator Destruction()
    {
        yield return new WaitForSeconds(3f);

        if (photonView.IsMine)
        {
            PhotonNetwork.Destroy(gameObject);
        }

        StopCoroutine(Destruction());
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

    private void Suck(Collider other)
    {
        if (other.gameObject.layer == 7)
        {
            PlayerBehaviour player = other.GetComponent<PlayerBehaviour>();
            player.ForSkillAgent(transform.position);
        }

        // �̴Ͼ� ������ ����
        else if (other.gameObject.layer == 8)
        {

        }

        // ����� �̴Ͼ� ������ ����
        else if (other.gameObject.layer == 13)
        {

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
            Suck(other);
            Damage(other.gameObject.transform);
        }
    }
}
