using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 가방을 누르면 인벤토리창이 열린다.
public class BagButton : MonoBehaviour
{
    // ###############################################
    //             NAME : Simstealer                      
    //             MAIL : minsub4400@gmail.com         
    // ###############################################

    private bool openInven;


    public void InventoryOnpen()
    {
        // LobbyCanvas Tag를 찾는다.
        /*openInven = !openInven;
        GameObject.FindGameObjectWithTag("LobbyCanvas").transform.GetChild(1).GetChild(0).gameObject.SetActive(openInven);*/
        GameObject.FindGameObjectWithTag("LobbyCanvas").transform.GetChild(1).GetChild(0).gameObject.SetActive(true);
    }

    public void InventoryClose()
    {
        GameObject.FindGameObjectWithTag("LobbyCanvas").transform.GetChild(1).GetChild(0).gameObject.SetActive(false);
    }
}
