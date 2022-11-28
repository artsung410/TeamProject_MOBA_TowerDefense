using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResultWindow : MonoBehaviour
{
    // ###############################################
    //             NAME : Simstealer                      
    //             MAIL : minsub4400@gmail.com         
    // ###############################################

    [SerializeField]
    private GameObject escWindow;

    private void Update()
    {
        if (escWindow.activeSelf)
        {
            escWindow.SetActive(false);
        }
    }
}
