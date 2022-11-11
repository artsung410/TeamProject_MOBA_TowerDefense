using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrawManager_DrawCountChange : MonoBehaviour
{
    // ###############################################
    //             NAME : Simstealer                      
    //             MAIL : minsub4400@gmail.com         
    // ###############################################

    private void OnEnable()
    {
        DrawManager.instance.boxCount = 1;
    }
}
