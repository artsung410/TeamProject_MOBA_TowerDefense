using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PayImageChange : MonoBehaviour
{
    // ###############################################
    //             NAME : Simstealer                      
    //             MAIL : minsub4400@gmail.com         
    // ###############################################

    [SerializeField]
    private Sprite zera;
    [SerializeField]
    private Sprite dappx;

    [SerializeField]
    private Image payImage;

    public void ZeraImageChange()
    {
        if (DrawManager.instance.buyCurencyName == "Zera")
            payImage.sprite = zera;
        //DrawManager.instance.buyCurencyName = "Zera";
    }

    public void DappxImageChange()
    {
        if (DrawManager.instance.buyCurencyName == "Dappx")
            payImage.sprite = dappx;
        //DrawManager.instance.buyCurencyName = "Dappx";
    }
    
}
