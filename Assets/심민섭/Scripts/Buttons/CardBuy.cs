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

    // CardBuy는 구매를 하고 텍스트를 변경해 줄 뿐
    // 정산은 DrawManager에서 한다.
    // 나중에 DB에서 데이터를 비교할 예정.

    // zera인지 dappx인지 판별
    // 1. DrawManager에서 값을 받는다.

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

    // 구매한 갯수 스크립트
    [SerializeField]
    private PayAmount buyCount;
    // 구매한 아이템의 이미지
    [SerializeField]
    public Image buyItemImage;
    // 구매한 아이템 명
    [SerializeField]
    public Text buyItemName;

    // 구매 bool
    private bool buyItem;

    private void OnEnable()
    {
        buyItem = false;
    }

    // 버튼이 눌리면 재화를 소모한다.
    public void BuyCard()
    {
        if (DrawManager.instance.buyCurencyName == "Zera")
        {
            // 재화량 검출
            int amount = int.Parse(zera.text.Split('.')[0]) - int.Parse(payAmount.text);
            if (amount < 0)
            {
                // 문구 띄워 주기
                noMoney.SetActive(true);
                return;
            }
            //Debug.Log(int.Parse(zera.text.Split('.')[0])); //앞자리만 정수, 뒷자리 소수점
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
                // 문구 띄워 주기
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
            Debug.LogError("재화 에러!!!");
        }
        // Buy 버튼을 누르면 해당 창이 닫힌다.
        buyWindow.SetActive(false);
    }
    
}
