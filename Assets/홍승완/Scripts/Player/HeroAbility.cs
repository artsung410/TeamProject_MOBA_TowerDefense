using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class HeroAbility : MonoBehaviourPun
{
    // ###############################################
    //             NAME : HongSW                      
    //             MAIL : gkenfktm@gmail.com         
    // ###############################################

    public GameObject[] AbilityPrefabs;
    public Transform skillSpawn;


    private bool isCoolMode;

    void Awake()
    {

    }

    void Start()
    {
        isCoolMode = false;
    }

    void Update()
    {
        if (photonView.IsMine)
        {
            AbilityInput();
            
        }
    }


    private void AbilityInput()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            //PhotonNetwork.Instantiate(AbilityPrefabs[0], skillSpawn);
            PhotonNetwork.Instantiate(AbilityPrefabs[0].name, skillSpawn.position, Quaternion.identity);

        }
        if (Input.GetKeyDown(KeyCode.W))
        {
            //Instantiate(AbilityPrefabs[1], skillSpawn);
            PhotonNetwork.Instantiate(AbilityPrefabs[1].name, skillSpawn.position, Quaternion.identity);

        }
        if (Input.GetKeyDown(KeyCode.E))
        {
            //Instantiate(AbilityPrefabs[2], skillSpawn);
            PhotonNetwork.Instantiate(AbilityPrefabs[2].name, skillSpawn.position, Quaternion.identity);

        }
        if (Input.GetKeyDown(KeyCode.R))
        {
            //Instantiate(AbilityPrefabs[3], skillSpawn);
            PhotonNetwork.Instantiate(AbilityPrefabs[3].name, skillSpawn.position, Quaternion.identity);

        }
    }


}
