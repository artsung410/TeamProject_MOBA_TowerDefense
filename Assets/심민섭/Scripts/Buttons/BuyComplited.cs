using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BuyComplited : MonoBehaviour
{
    // ###############################################
    //             NAME : Simstealer                      
    //             MAIL : minsub4400@gmail.com         
    // ###############################################

    [SerializeField]
    private Image buyItemImage;
    [SerializeField]
    private Text buyItemName;
    [SerializeField]
    private Text buyCount;

    private void OnEnable()
    {
        StartCoroutine(BuyerInfoUpdate());
    }

    IEnumerator BuyerInfoUpdate()
    {
        yield return new WaitForSeconds(0.01f);
        // 활성화 되면 버튼 정보를 업데이트
        buyItemImage.sprite = DrawManager.instance.selectImage;
        buyItemName.text = DrawManager.instance.selectNameText;
        buyCount.text = "X" + DrawManager.instance.buyCount.ToString();
    }
    
}
