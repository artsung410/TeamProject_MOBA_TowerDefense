using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    public void DrawComplitedBackButton()
    {
        DrawManager.instance.CardDataInit();
        cancelObj.SetActive(false);
    }
}
