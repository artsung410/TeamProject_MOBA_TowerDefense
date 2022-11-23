using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class DangerZone : MonoBehaviourPun
{
    // ###############################################
    //             NAME : ARTSUNG                      
    //             MAIL : artsung410@gmail.com         
    // ###############################################

    private void OnEnable()
    {
        StartCoroutine(Destruction());
    }

    private IEnumerator Destruction()
    {
        yield return new WaitForSeconds(1.5f);

        if (photonView.IsMine)
        {
            PhotonNetwork.Destroy(gameObject);
        }

        StopCoroutine(Destruction());
    }
}
