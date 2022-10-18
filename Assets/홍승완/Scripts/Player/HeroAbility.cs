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
            Instantiate(AbilityPrefabs[0], skillSpawn);

        }
        if (Input.GetKeyDown(KeyCode.W))
        {
            Instantiate(AbilityPrefabs[1], skillSpawn);

        }
        if (Input.GetKeyDown(KeyCode.E))
        {
            Instantiate(AbilityPrefabs[2], skillSpawn);

        }
        if (Input.GetKeyDown(KeyCode.R))
        {
            Instantiate(AbilityPrefabs[3], skillSpawn);

        }
    }


}
