using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SelectButtonColorChange : MonoBehaviour
{
    // ###############################################
    //             NAME : Simstealer                      
    //             MAIL : minsub4400@gmail.com         
    // ###############################################

    private void OnEnable()
    {
        ColorBlock colorBlock = gameObject.GetComponent<Button>().colors;
        colorBlock.normalColor = Color.yellow;
        gameObject.GetComponent<Button>().colors = colorBlock;
    }
}
