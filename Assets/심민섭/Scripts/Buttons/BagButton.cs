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
        openInven = !openInven;
        gameObject.SetActive(openInven);
        
    }
}
