using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.Networking;
using LitJson;
using UnityEngine.UI;

// ��ư�� ������ Select Matchȭ���� Ȱ��ȭ �ȴ�.
public class PlayerButton : MonoBehaviourPun
{
    private GameObject selectMatch;
    public int Playercount;


    private void OnEnable()
    {
        if (gameObject.name == "CollectButton")
        {
            gameObject.GetComponent<Button>().interactable = true;
        }
    }

    private void Start()
    {
        if (gameObject.name == "PlayButton")
            gameObject.GetComponent<Button>().interactable = false;
    }
    private void Awake()
    {
        Playercount = 0;
        //selectMatch = GameObject.FindGameObjectWithTag("SelectMatch");
        selectMatch = gameObject.transform.parent.parent.GetChild(6).gameObject;
    }
    [PunRPC]
    public void SelectMatchPopUp()
    {
        Playercount++;
        selectMatch.SetActive(true);
    }

    // ��ȹ�� ���� �۾���(��Ī ȿ����)
    public AudioClip clickSound;
    //public AudioClip clickCancleSound;

    [SerializeField]
    private AudioSource collectAudio;

    // Collect ��ư Ŭ�� ����
    public void CollectButtonSound()
    {
        collectAudio.clip = clickSound;
        collectAudio.Play();
    }

    public void OnButton()
    {
        gameObject.GetComponent<Button>().interactable = true;
    }

    public void OffButton()
    {
        gameObject.GetComponent<Button>().interactable = false;
    }
}
