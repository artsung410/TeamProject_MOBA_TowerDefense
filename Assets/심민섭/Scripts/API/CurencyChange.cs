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
        // Ȱ��ȭ�� �� ���� ��ȭ ���� ������ ������ ������Ʈ �Ѵ�.

        if (gameObject.name == "DaapxCount")
        {
            gameObject.GetComponent<Text>().text = GameObject.FindGameObjectWithTag("GetCaller").GetComponent<PlayerStorage>().win_amount;
        }
        

        if (gameObject.name == "Dappx - Text")
        {
            gameObject.GetComponent<Text>().text = "0(�ӽ�)";//GameObject.FindGameObjectWithTag("APIStorage").GetComponent<APIStorage>().amount_won;
        }
    }
}
