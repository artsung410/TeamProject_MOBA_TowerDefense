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

    [Header("Å¸°Ù TAG")]
    public string enemyTag;

    protected void OnEnable()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            if (PhotonNetwork.LocalPlayer.ActorNumber == 1 && photonView.IsMine)
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

        else
        {
            if (PhotonNetwork.LocalPlayer.ActorNumber == 2 && photonView.IsMine)
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


}
