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
        if (other.gameObject.layer == 7)
        {
            PlayerBehaviour playerBehaviour = other.GetComponent<PlayerBehaviour>();
            playerBehaviour.ForSkillAgent(transform.position);
        }

        // 미니언 데미지 적용
        else if (other.gameObject.layer == 8)
        {

        }

        // 스페셜 미니언 데미지 적용
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
            Damage(other.gameObject.transform);
        }
    }

    float elapsedTime;
    private void OnTriggerStay(Collider other)
    {
        elapsedTime += Time.deltaTime;
        if (other.tag == enemyTag)
        {
            if (elapsedTime >= 0.55f)
            {
                Suck(other);
                elapsedTime = 0f;
            }
        }
    }
}
