using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class TestPlayer : MonoBehaviourPun
{
    // ###############################################
    //             NAME : ARTSUNG                      
    //             MAIL : artsung410@gmail.com         
    // ###############################################

    public Transform bulletSpawnPoint;
    public GameObject bulletPf;
    public Vector3[] movePositions;
    public int i;
    private void Update()
    {
        if (!photonView.IsMine)
        {
            return;
        }

        if (Input.GetKeyDown(KeyCode.P))
        {
            PhotonNetwork.Instantiate(bulletPf.name, bulletSpawnPoint.position, bulletSpawnPoint.rotation);
        }

        if (Input.GetKeyDown(KeyCode.O))
        {
            i++;

            if (i > 3)
            {
                i = 0;
            }

            transform.position = movePositions[i];
        }
    }
}
