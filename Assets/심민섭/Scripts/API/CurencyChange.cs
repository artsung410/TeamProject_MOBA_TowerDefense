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

    public static CurencyChange instance;

    private void Awake()
    {
        instance = this;
    }

    public string amountCurrency;
    public bool amountCurrencyBool = false;

    public void AmountCurrency()
    {
        gameObject.GetComponent<Text>().text = amountCurrency;
        WinnerManager.instance.isWinAmountBoll = true;
    }

    private void OnEnable()
    {
        // Ȱ��ȭ�� �� ���� ��ȭ ���� ������ ������ ������Ʈ �Ѵ�.

        if (gameObject.name == "DaapxCount")
        {
            gameObject.GetComponent<Text>().text = GameObject.FindGameObjectWithTag("GetCaller").GetComponent<PlayerStorage>().win_amount;
        }
    }
}
