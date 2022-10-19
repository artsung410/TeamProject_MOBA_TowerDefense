using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

// ###############################################
//             NAME : ARTSUNG                      
//             MAIL : artsung410@gmail.com         
// ###############################################

public abstract class Turret : MonoBehaviourPun
{
    public int Hp;
    protected void OnEnable()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            gameObject.layer = 10;
        }
        else
        {
            gameObject.layer = 11;
        }
    }

    public void TakeDamage(int Damage)
    {
        if (Damage <= 0)
        {
            Destroy(gameObject);
            return;
        }

        Hp -= Damage;
    }

    [Header("Å¸°Ù TAG")]
    public string enemyTag = "Enemy";
}
