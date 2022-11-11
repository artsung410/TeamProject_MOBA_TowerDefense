using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemOpenCount : MonoBehaviour
{
    // ###############################################
    //             NAME : Simstealer                      
    //             MAIL : minsub4400@gmail.com         
    // ###############################################

    // 버튼이 눌리면 갯수를 DrawManager에게 값을 전달함

    // x1
    public void AddPayAmount()
    {
        DrawManager.instance.boxCount = 1;
    }

    // x10
    public void MulPayAmount()
    {
        DrawManager.instance.boxCount = 10;
        ColorBlock colorBlock = gameObject.transform.parent.GetChild(2).gameObject.GetComponent<Button>().colors;
        colorBlock.normalColor = Color.white;
        gameObject.transform.parent.GetChild(2).gameObject.GetComponent<Button>().colors = colorBlock;
    }
}
