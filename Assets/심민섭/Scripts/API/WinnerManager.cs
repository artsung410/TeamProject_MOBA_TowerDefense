using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class WinnerManager : MonoBehaviourPun
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

    // win API
    public bool winner = false;
    // game over
    public bool gameIsOver = false;

    // 배팅 호출 프리펩
    public GameObject WinPostAPICallerPre;


    // 배당금 업데이트 했는지 bool 값
    public bool isWinAmountBoll = false;

    // 배당금
    public string winAmount;

    private void Update()
    {

        if (GameManager.Instance.isGameEnd)
        {
            gameIsOver = true;
        }

        if (gameIsOver && PhotonNetwork.IsMasterClient)
        {
            gameIsOver = false;
            Instantiate(WinPostAPICallerPre, Vector3.zero, Quaternion.identity);
        }

        if (winner)
        {
            winner = false;
            CurencyChange.instance.amountCurrency = winAmount;
            CurencyChange.instance.AmountCurrency();
            // 클라이언트 RPC 해주어야함
            if (isWinAmountBoll)
            {
                Destroy(GameObject.FindGameObjectWithTag("APIStorage").gameObject);
                Destroy(GameObject.FindGameObjectWithTag("GetCaller").gameObject);
                Destroy(gameObject);
            }
            
            
        }

        /*if (Input.GetKeyDown(KeyCode.T))
        {
            Instantiate(WinPostAPICallerPre, Vector3.zero, Quaternion.identity);
            Destroy(gameObject);
        }*/
    }
}
