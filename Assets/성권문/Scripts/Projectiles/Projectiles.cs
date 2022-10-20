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

    [Header("Å¸°Ù TAG")]
    public string enemyTag;
    public float damage;

    protected void OnEnable()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            if (PhotonNetwork.LocalPlayer.ActorNumber == 1 && photonView.IsMine)
            {
                gameObject.tag = "Blue";
                enemyTag = "Red";
            }

            else
            {
                gameObject.tag = "Red";
                enemyTag = "Blue";
            }
        }

        else
        {
            if (PhotonNetwork.LocalPlayer.ActorNumber == 2 && photonView.IsMine)
            {
                gameObject.tag = "Red";
                enemyTag = "Blue";
            }

            else
            {
                gameObject.tag = "Blue";
                enemyTag = "Red";
            }
        }
    }

    protected void Damage(Transform enemy)
    {
        EnemyMinion e = enemy.GetComponent<EnemyMinion>();

        if (e != null)
        {
            e.TakeDamage(damage);
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
