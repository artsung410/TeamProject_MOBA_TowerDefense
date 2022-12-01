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
    [Header("타겟 TAG")]
    [HideInInspector]
    public string enemyTag;

    [HideInInspector]
    public float damage;

    public int EffectID;

    public float effectRange;
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

                Vector3 vecDistance = enemy.transform.position - transform.position; //거리계산
                float distance = vecDistance.sqrMagnitude; // 최적화
                if (distance <= effectRange * effectRange) //최적화 공격범위 안에있을때
                {
                    BuffManager.Instance.AddBuff(CSVtest.Instance.BuffDic[EffectID]);
                }
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
            Debug.Log("매직 익스플로젼");
        }
    }
}
