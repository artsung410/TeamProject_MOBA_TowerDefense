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
        payImage.sprite = zera;
        DrawManager.instance.buyCurencyName = "Zera";
    }

    public void DappxImageChange()
    {
        payImage.sprite = dappx;
        DrawManager.instance.buyCurencyName = "Dappx";
    }
    
}
