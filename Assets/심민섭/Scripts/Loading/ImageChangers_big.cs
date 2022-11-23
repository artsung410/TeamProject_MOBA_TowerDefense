using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

public class ImageChangers_big : MonoBehaviourPun
{
    // ###############################################
    //             NAME : Simstealer                      
    //             MAIL : minsub4400@gmail.com         
    // ###############################################

    [SerializeField]
    private GameObject bigImage;

    private void Awake()
    {


        if (photonView.IsMine)
        {
            transform.position = LoadingMagager.Instance.loadingCharactorPos[0].position;
            ChangeCharacterImages();
        }

        else
        {
            transform.position = LoadingMagager.Instance.loadingCharactorPos[1].position;
        }

    }

    private void ChangeCharacterImages()
    {
        if (GameObject.FindGameObjectWithTag("GetCaller").GetComponent<TrojanHorse>().selectCharacter == "Warrior")
        {
            photonView.RPC(nameof(RPC_ImageSyncro), RpcTarget.All, (int)LoadingCharactorType.Warrior);
        }

        else if (GameObject.FindGameObjectWithTag("GetCaller").GetComponent<TrojanHorse>().selectCharacter == "Wizard")
        {
            photonView.RPC(nameof(RPC_ImageSyncro), RpcTarget.All, (int)LoadingCharactorType.Wizard);
        }
    }

    [PunRPC]
    private void RPC_ImageSyncro(int heroIndex)
    {
        bigImage.GetComponent<Image>().sprite = LoadingMagager.Instance.heroImages[heroIndex];
    }
}
