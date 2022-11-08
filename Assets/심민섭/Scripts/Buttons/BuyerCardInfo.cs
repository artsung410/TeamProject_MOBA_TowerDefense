using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BuyerCardInfo : MonoBehaviour
{
    // ###############################################
    //             NAME : Simstealer                      
    //             MAIL : minsub4400@gmail.com         
    // ###############################################

    // 버튼 정보 업데이트
    private Image buyerItem;
    private Text buyerName;
    private Text buyerTootip;

    private void Awake()
    {
        buyerItem = gameObject.transform.GetChild(0).GetChild(0).GetComponent<Image>();
        buyerName = gameObject.transform.GetChild(0).GetChild(1).GetComponent<Text>();
        buyerTootip = gameObject.transform.GetChild(0).GetChild(2).GetComponent<Text>();
    }

    private void OnEnable()
    {
        StartCoroutine(BuyerInfoUpdate());
    }

    IEnumerator BuyerInfoUpdate()
    {
        yield return new WaitForSeconds(0.01f);
        // 활성화 되면 버튼 정보를 업데이트
        buyerItem.sprite = DrawManager.instance.selectImage;
        buyerName.text = DrawManager.instance.selectNameText;
        buyerTootip.text = DrawManager.instance.selectExplanationText;
    }

    // PayAmount init
    [SerializeField]
    private Text payAmount;
    private void OnDisable()
    {
        payAmount.text = 100.ToString();
    }

}
