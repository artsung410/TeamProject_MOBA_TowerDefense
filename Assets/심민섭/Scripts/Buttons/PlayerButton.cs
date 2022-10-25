using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

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

}
