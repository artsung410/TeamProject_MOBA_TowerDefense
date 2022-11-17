using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class CharactorRenderer : MonoBehaviourPun
{
    // ###############################################
    //             NAME : HongSW                      
    //             MAIL : gkenfktm@gmail.com         
    // ###############################################

    public GameObject ColPosition;

    Vector3 interpolation = new Vector3(0, 2, 0);

    private void LateUpdate()
    {
        if (photonView.IsMine)
        {
            transform.position = ColPosition.transform.position - interpolation;
            transform.rotation = ColPosition.transform.localRotation;
        }
    }

}
