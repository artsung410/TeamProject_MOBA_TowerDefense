using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class SMS_TestScript : MonoBehaviour, IPunObservable
{
    // ###############################################
    //             NAME : Simstealer                      
    //             MAIL : minsub4400@gmail.com         
    // ###############################################

    public int Health = 5;

    // IPunObservable �������̽��� �����ؾ��Ѵ�.
    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            // We own this player: send the others our data
            stream.SendNext(Health);
        }
        else
        {
            this.Health = (int)stream.ReceiveNext();
        }
    }
}
