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
            if (PhotonNetwork.LocalPlayer.ActorNumber == 1 && photonView.IsMine)
            {
                gameObject.layer = 10;
            }

            else
            {
                gameObject.layer = 11;
            }
        }

        else
        {
            if (PhotonNetwork.LocalPlayer.ActorNumber == 2 && photonView.IsMine)
            {
                gameObject.layer = 11;
            }

            else
            {
                gameObject.layer = 10;
            }
        }

    }

    //[PunRPC]
    //public void SetLayer()
    //{
    //    if (PhotonNetwork.IsMasterClient)
    //    {
    //        gameObject.layer = 10;

      
    //        photonView.RPC("RedSet", RpcTarget.Others);
    //        photonView.RPC("SetLayer", RpcTarget.Others);
           
    //    }
    //}

    //[PunRPC]
    //public void RedSet()
    //{
    //    gameObject.layer = 11;
    //}

    public void TakeDamage(int Damage)
    {
        if (Damage <= 0)
        {
            Destroy(gameObject);
            return;
        }

        Hp -= Damage;
    }

    //private void Update()
    //{
    //    SetLayer();
    //}

    [Header("Å¸°Ù TAG")]
    public string enemyTag = "Enemy";
}
