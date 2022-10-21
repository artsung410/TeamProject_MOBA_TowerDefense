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

            if (player != null)
            {
                if (player.hpSlider3D.value <= 0)
                {
                    player.transform.gameObject.SetActive(false);
                }
                player.OnDamage(damage);
                
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
    }

    protected void OnTriggerEnter(Collider other)
    {
        if (projectileType == ProjectileType.Bullet)
        {
            return;
        }

        if (other.CompareTag(enemyTag))
        {
            Damage(other.gameObject.transform);
        }
    }
}
