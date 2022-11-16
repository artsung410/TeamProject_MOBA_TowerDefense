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

    [Header("타겟 TAG")]
    public string enemyTag;

    [HideInInspector]
    public float damage;
    private void OnEnable()
    {
        // 피아식별
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
        // 플레이어 데미지 적용
        if (enemy.gameObject.layer == 7)
        {
            Debug.Log("Player 데미지 적용");
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

        // 미니언 데미지 적용
        else if (enemy.gameObject.layer == 8)
        {
            Debug.Log("미니언 데미지 적용");
            Enemybase minion = enemy.GetComponent<Enemybase>();

            if (minion != null)
            {
                minion.TakeDamage(damage);
            }
        }

        // 스페셜 미니언 데미지 적용
        else if (enemy.gameObject.layer == 13)
        {
            Debug.Log("스페셜 미니언 데미지 적용");
            Enemybase special_minion = enemy.GetComponent<Enemybase>();

            if (special_minion != null)
            {
                special_minion.TakeDamage(damage);
            }
        }
    }

    private void Suck(Collider other)
    {
        // 플레이어 빨아들이기
        if (other.gameObject.layer == 7)
        {
            PlayerBehaviour playerBehaviour = other.GetComponent<PlayerBehaviour>();
            playerBehaviour.ForSkillAgent(transform.position);
        }

        // 미니언 빨아들이기
        else if (other.gameObject.layer == 8)
        {
            Enemybase minion = other.GetComponent<Enemybase>();
            minion.ForSkillAgent(transform.position);
        }

        // 스폐셜 미니언 빨아들이기
        else if (other.gameObject.layer == 13)
        {
            Enemybase minion = other.GetComponent<Enemybase>();
            minion.ForSkillAgent(transform.position);
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
        }
    }

    float elapsedTime;
    private void OnTriggerStay(Collider other)
    {
        elapsedTime += Time.deltaTime;
        if (other.tag == enemyTag)
        {
            if (elapsedTime >= 1f)
            {
                Suck(other);
                elapsedTime = 0f;
            }
        }
    }
}
