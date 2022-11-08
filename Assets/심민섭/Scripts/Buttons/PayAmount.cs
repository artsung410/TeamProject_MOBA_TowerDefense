using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class PayAmount : MonoBehaviour
{
    // ###############################################
    //             NAME : Simstealer                      
    //             MAIL : minsub4400@gmail.com         
    // ###############################################

    // x1은 더하기 1
    // x10은 곱하기 10

    // PayAmount Text
    [SerializeField]
    private Text payAmount;

    // 구매 갯수
    public int cardCount;

    private void OnEnable()
    {
        cardCount = 1;
    }

    private void OnDisable()
    {
        cardCount = 1;
    }

    public void AddPayAmount()
    {
        payAmount.text = (int.Parse(payAmount.text) + 100).ToString();
        cardCount += 1;
        Debug.Log(cardCount);
    }

    public void MulPayAmount()
    {
        payAmount.text = (int.Parse(payAmount.text) + 1000).ToString();
        cardCount += 10;
        Debug.Log(cardCount);
    }
}
