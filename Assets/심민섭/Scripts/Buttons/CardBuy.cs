using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CardBuy : MonoBehaviour
{
    // ###############################################
    //             NAME : Simstealer                      
    //             MAIL : minsub4400@gmail.com         
    // ###############################################

    // CardBuy�� ���Ÿ� �ϰ� �ؽ�Ʈ�� ������ �� ��
    // ������ DrawManager���� �Ѵ�.
    // ���߿� DB���� �����͸� ���� ����.

    // zera���� dappx���� �Ǻ�
    // 1. DrawManager���� ���� �޴´�.

    [SerializeField]
    private Text payAmount;

    [SerializeField]
    private Text zera;

    [SerializeField]
    private Text dappx;

    [SerializeField]
    private GameObject noMoney;

    [SerializeField]
    private GameObject buyCheckObj;

    [SerializeField]
    private GameObject buyWindow;

    // ������ ���� ��ũ��Ʈ
    [SerializeField]
    private PayAmount buyCount;
    // ������ �������� �̹���
    [SerializeField]
    public Image buyItemImage;
    // ������ ������ ��
    [SerializeField]
    public Text buyItemName;

    // ���� bool
    private bool buyItem;

    private void OnEnable()
    {
        buyItem = false;
    }

    // ��ư�� ������ ��ȭ�� �Ҹ��Ѵ�.
    public void BuyCard()
    {
        if (DrawManager.instance.buyCurencyName == "Zera")
        {
            // ��ȭ�� ����
            int amount = int.Parse(zera.text.Split('.')[0]) - int.Parse(payAmount.text);
            if (amount < 0)
            {
                // ���� ��� �ֱ�
                noMoney.SetActive(true);
                return;
            }
            //Debug.Log(int.Parse(zera.text.Split('.')[0])); //���ڸ��� ����, ���ڸ� �Ҽ���
            zera.text = (int.Parse(zera.text.Split('.')[0]) - int.Parse(payAmount.text)).ToString();
            buyItem = true;
            if (buyItem)
            {
                buyCheckObj.SetActive(true);
                buyItem = false;
            }
            //DrawManager.instance.buyCount = buyCount.cardCount;
            DrawManager.instance.buyItemImage = buyItemImage;
            DrawManager.instance.buyItemName = buyItemName.text;
            DrawManager.instance.PutCardItem_AfterBuy();
        }
        else if (DrawManager.instance.buyCurencyName == "Dappx")
        {
            int amount = int.Parse(dappx.text.Split('.')[0]) - int.Parse(payAmount.text);
            if (amount < 0)
            {
                // ���� ��� �ֱ�
                noMoney.SetActive(true);
                return;
            }

            dappx.text = (int.Parse(dappx.text.Split('.')[0]) - int.Parse(payAmount.text)).ToString();
            buyItem = true;
            if (buyItem)
            {
                buyCheckObj.SetActive(true);
                buyItem = false;
            }
            //DrawManager.instance.buyCount = buyCount.cardCount;
            DrawManager.instance.buyItemImage = buyItemImage;
            DrawManager.instance.buyItemName = buyItemName.text;
            DrawManager.instance.PutCardItem_AfterBuy();
        }
        else
        {
            Debug.LogError("��ȭ ����!!!");
        }
        // Buy ��ư�� ������ �ش� â�� ������.
        buyWindow.SetActive(false);
    }
    
}
