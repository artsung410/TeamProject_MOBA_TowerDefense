using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CardOtherTabButton : MonoBehaviour
{
    // ###############################################
    //             NAME : Simstealer                      
    //             MAIL : minsub4400@gmail.com         
    // ###############################################

    // 이미지 데이터
    [SerializeField]
    private Sprite originalImageName;
    [SerializeField]
    private Sprite changeImageName;

    public GameObject card;
    public GameObject other;

    public void cardButtonImageChange()
    {
        // 변경될 이미지를 넣음.
        gameObject.GetComponent<Image>().sprite = changeImageName;
        gameObject.transform.GetChild(0).GetComponent<Text>().color = Color.white;
        other.GetComponent<Image>().sprite = originalImageName;
        other.transform.GetChild(0).GetComponent<Text>().color = Color.grey;
        // 선택되면 FFFFFF

        // 선택되지 않으면 323232
    }

    public void otherButtonImageChange()
    {
        // 변경될 이미지를 넣음.
        gameObject.GetComponent<Image>().sprite = changeImageName;
        gameObject.transform.GetChild(0).GetComponent<Text>().color = Color.white;
        card.GetComponent<Image>().sprite = originalImageName;
        card.transform.GetChild(0).GetComponent<Text>().color = Color.grey;
    }
}
