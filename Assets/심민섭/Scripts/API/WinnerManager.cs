using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WinnerManager : MonoBehaviour
{
    // ###############################################
    //             NAME : Simstealer                      
    //             MAIL : minsub4400@gmail.com         
    // ###############################################
    public static WinnerManager instance;

    private void Awake()
    {
        instance = this;
    }

    // ���� ȣ�� ������
    public GameObject WinPostAPICallerPre;

    // ����
    public string winAmount;

    private void Update()
    {
        if (GameManager.Instance.isGameEnd)
        {
            Instantiate(WinPostAPICallerPre, Vector3.zero, Quaternion.identity);
            CurencyChange.instance.amountCurrency = winAmount;
            Destroy(GameObject.FindGameObjectWithTag("APIStorage").gameObject);
            Destroy(GameObject.FindGameObjectWithTag("GetCaller").gameObject);
            Destroy(gameObject);
        }
        /*if (Input.GetKeyDown(KeyCode.T))
        {
            Instantiate(WinPostAPICallerPre, Vector3.zero, Quaternion.identity);
            Destroy(gameObject);
        }*/
    }
}
