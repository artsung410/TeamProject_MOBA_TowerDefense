using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.Networking;
using LitJson;
using UnityEngine.UI;

// 버튼이 눌리면 Select Match화면이 활성화 된다.
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

    // 기획팀 사운드 작업본(매칭 효과음)
    public AudioClip clickSound;
    //public AudioClip clickCancleSound;

    [SerializeField]
    private AudioSource collectAudio;

    // Collect 버튼 클릭 사운드
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
