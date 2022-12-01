using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CurencyChange : MonoBehaviour
{
    // ###############################################
    //             NAME : Simstealer                      
    //             MAIL : minsub4400@gmail.com         
    // ###############################################

    private void OnEnable()
    {
        // 활성화될 때 마다 재화 배팅 수량을 가져와 업데이트 한다.

        if (gameObject.name == "DaapxCount")
        {
            gameObject.GetComponent<Text>().text = GameObject.FindGameObjectWithTag("GetCaller").GetComponent<PlayerStorage>().win_amount;
        }
        

        if (gameObject.name == "Dappx - Text")
        {
            gameObject.GetComponent<Text>().text = "0(임시)";//GameObject.FindGameObjectWithTag("APIStorage").GetComponent<APIStorage>().amount_won;
        }
    }
}
