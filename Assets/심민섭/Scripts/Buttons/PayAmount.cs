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

    // x1�� ���ϱ� 1
    // x10�� ���ϱ� 10

    // PayAmount Text
    [SerializeField]
    private Text payAmount;

    public void AddPayAmount()
    {
        payAmount.text = (int.Parse(payAmount.text) + 100).ToString();
    }

    public void MulPayAmount()
    {
        payAmount.text = (int.Parse(payAmount.text) * 10).ToString();
    }
}
