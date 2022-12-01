using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class TurretParticles : MonoBehaviourPun
{
    // ###############################################
    //             NAME : ARTSUNG                      
    //             MAIL : artsung410@gmail.com         
    // ###############################################
    public Transform SpawnPoint;

    void Start()
    {
        //PhotonNetwork.Instantiate(.name, SpawnPoint.position, SpawnPoint.rotation);
    }
}
