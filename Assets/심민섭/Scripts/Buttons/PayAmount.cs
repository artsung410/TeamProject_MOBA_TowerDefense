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

    [SerializeField]
    private GameObject oneButton;

    // ���� ����
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
        payAmount.text = 100.ToString();// (int.Parse(payAmount.text) + 100).ToString();
        cardCount = 1;
        DrawManager.instance.buyCount = cardCount;
        //Debug.Log(cardCount);
    }

    public void MulPayAmount()
    {
        payAmount.text = 1000.ToString(); //(int.Parse(payAmount.text) + 1000).ToString();
        cardCount = 10;
        DrawManager.instance.buyCount = cardCount;
        ColorBlock colorBlock = oneButton.GetComponent<Button>().colors;
        colorBlock.normalColor = Color.white;
        oneButton.GetComponent<Button>().colors = colorBlock;
        //Debug.Log(cardCount);
    }
}
