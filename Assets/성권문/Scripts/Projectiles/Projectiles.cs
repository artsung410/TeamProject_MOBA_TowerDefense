using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public enum ProjectileType
{
    Laser,
    Arc,
    Circle,
    Bullet
}

public class Projectiles : MonoBehaviourPun
{
    // ###############################################
    //             NAME : ARTSUNG                      
    //             MAIL : artsung410@gmail.com         
    // ###############################################
    public ProjectileType projectileType;

    [Header("타겟 TAG")]
    public string enemyTag;
    public float damage;

    protected void OnEnable()
    {
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

    protected void Damage(Transform enemy)
    {
        // 플레이어 데미지 적용
        if (enemy.gameObject.layer == 7)
        {
            Health player= enemy.GetComponent<Health>();
             
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
        else if(enemy.gameObject.layer == 8)
        {
            Enemybase minion = enemy.GetComponent<Enemybase>();

            if (minion != null)
            {
                minion.TakeDamage(damage);
            }
        }

        // 스페셜 미니언 데미지 적용
        else if (enemy.gameObject.layer == 13)
        {
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

        if (projectileType == ProjectileType.Bullet)
        {
            return;
        }

        if (other.tag == enemyTag)
        {
            Damage(other.gameObject.transform);
        }
    }
}
