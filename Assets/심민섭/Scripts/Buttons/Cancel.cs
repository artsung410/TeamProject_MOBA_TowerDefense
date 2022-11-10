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

    public void CancleDrawButton()
    {
        cancelObj.SetActive(false);
        // ��ų ī�� �̱�â
        GameObject.FindGameObjectWithTag("DrawInventory").transform.GetChild(0).GetChild(1).gameObject.SetActive(false);
        // Ÿ�� ī�� �̱�â
        GameObject.FindGameObjectWithTag("DrawInventory").transform.GetChild(0).GetChild(2).gameObject.SetActive(false);
    }

    // Back ��ư�� ������ DrawManager�� buyCount�� 1�� �ʱ�ȭ
    // oneButton�� �븻 �÷��� ��������� ����
    public void BuyBackButton()
    {
        cancelObj.SetActive(false);
        ColorBlock colorBlock = gameObject.transform.parent.GetChild(3).gameObject.GetComponent<Button>().colors;
        colorBlock.normalColor = Color.yellow;
        gameObject.transform.parent.GetChild(3).gameObject.GetComponent<Button>().colors = colorBlock;
    }

    // �̹� ������ �� üũ Back ��ư
    // ����â�� �Բ� ����
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
