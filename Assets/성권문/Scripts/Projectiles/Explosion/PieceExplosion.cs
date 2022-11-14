using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class PieceExplosion : MonoBehaviourPun
{
    [Header("타겟 TAG")]
    public string enemyTag;
    public float damage;

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

    private void OnTriggerEnter(Collider other)
    {
        if (enemyTag == null)
        {
            return;
        }

        if (!PhotonNetwork.IsMasterClient)
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
