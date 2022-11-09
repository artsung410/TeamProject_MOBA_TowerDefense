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
    public GameObject Particles;

    void Start()
    {
        PhotonNetwork.Instantiate(Particles.name, SpawnPoint.position, SpawnPoint.rotation);
    }
}
