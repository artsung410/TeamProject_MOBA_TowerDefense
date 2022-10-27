using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.Networking;
using LitJson;

// 버튼이 눌리면 Select Match화면이 활성화 된다.
public class PlayerButton : MonoBehaviourPun
{
    private GameObject selectMatch;
    public int Playercount;
    private void Awake()
    {
        Playercount = 0;
        //selectMatch = GameObject.FindGameObjectWithTag("SelectMatch");
        selectMatch = gameObject.transform.parent.parent.GetChild(5).gameObject;
    }
    [PunRPC]
    public void SelectMatchPopUp()
    {
        Playercount++;
        Debug.Log($"{Playercount}");
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
}
