using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Cancel : MonoBehaviour
{
    // ###############################################
    //             NAME : Simstealer                      
    //             MAIL : minsub4400@gmail.com         
    // ###############################################

    public GameObject cancelObj;

    public void CancelButton()
    {
        cancelObj.SetActive(false);
    }

    public void InventoryBackButton()
    {
        cancelObj.SetActive(false);
        GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerInventory>().BackButton();

    }

    public void CancleDrawButton()
    {
        cancelObj.SetActive(false);
        // 스킬 카드 뽑기창
        GameObject.FindGameObjectWithTag("DrawInventory").transform.GetChild(0).GetChild(1).gameObject.SetActive(false);
        // 타워 카드 뽑기창
        GameObject.FindGameObjectWithTag("DrawInventory").transform.GetChild(0).GetChild(2).gameObject.SetActive(false);
    }

    // Back 버튼을 누르면 DrawManager에 buyCount를 1로 초기화
    // oneButton의 노말 컬러를 노랑색으로 변경
    public void BuyBackButton()
    {
        cancelObj.SetActive(false);
        ColorBlock colorBlock = gameObject.transform.parent.GetChild(3).gameObject.GetComponent<Button>().colors;
        colorBlock.normalColor = Color.yellow;
        gameObject.transform.parent.GetChild(3).gameObject.GetComponent<Button>().colors = colorBlock;
    }

    // 버튼을 누르면 카드/타워 선택 화면으로 돌아감
    public void CardAndTowerDrawWindow()
    {
        cancelObj.SetActive(false);
        GameObject.FindGameObjectWithTag("DrawInventory").transform.GetChild(0).GetChild(0).gameObject.SetActive(true);
    }

    // 이미 구매한 후 체크 Back 버튼
    // 구매창도 함께 닫음
    public void alreadyBuyBackButton()
    {
        GameObject.FindGameObjectWithTag("DrawInventory").transform.GetChild(1).gameObject.SetActive(false);
        cancelObj.SetActive(false);
    }

    public void DrawComplitedBackButton()
    {
        DrawManager.instance.CardDataInit();
        cancelObj.SetActive(false);
    }
}
