using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OpenBoxWindow : MonoBehaviour
{
    // ###############################################
    //             NAME : Simstealer                      
    //             MAIL : minsub4400@gmail.com         
    // ###############################################

    private Image drawImage;
    private Text drawText;

    private void Awake()
    {
        drawImage = gameObject.transform.GetChild(0).GetChild(0).GetComponent<Image>();
        drawText = gameObject.transform.GetChild(0).GetChild(1).GetComponent<Text>();
    }
    // 여기서는 뽑기 이미지와 이름을 업데이트하는 역할을 한다.
    private void OnEnable()
    {
        StartCoroutine(DrawInfoChages());
    }


    IEnumerator DrawInfoChages()
    {
        yield return new WaitForSeconds(0.01f);

        Debug.Log(DrawManager.instance.boxImage);
        Debug.Log(DrawManager.instance.boxName);
        drawImage.sprite = DrawManager.instance.boxImage;
        drawText.text = DrawManager.instance.boxName;
    }
}
