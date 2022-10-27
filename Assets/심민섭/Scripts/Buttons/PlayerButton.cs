using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.Networking;
using LitJson;

// ��ư�� ������ Select Matchȭ���� Ȱ��ȭ �ȴ�.
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
}
