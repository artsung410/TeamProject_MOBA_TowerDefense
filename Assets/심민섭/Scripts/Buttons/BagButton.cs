using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// ������ ������ �κ��丮â�� ������.
public class BagButton : MonoBehaviour
{
    // ###############################################
    //             NAME : Simstealer                      
    //             MAIL : minsub4400@gmail.com         
    // ###############################################

    private bool openInven;


    public void InventoryOnpen()
    {
        // LobbyCanvas Tag�� ã�´�.
        /*openInven = !openInven;
        GameObject.FindGameObjectWithTag("LobbyCanvas").transform.GetChild(1).GetChild(0).gameObject.SetActive(openInven);*/
        GameObject.FindGameObjectWithTag("LobbyCanvas").transform.GetChild(1).GetChild(0).gameObject.SetActive(true);
    }

    public void InventoryClose()
    {
        GameObject.FindGameObjectWithTag("LobbyCanvas").transform.GetChild(1).GetChild(0).gameObject.SetActive(false);
    }
}
