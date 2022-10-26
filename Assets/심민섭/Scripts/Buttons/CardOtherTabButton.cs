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

    // �̹��� ������
    [SerializeField]
    private Sprite originalImageName;
    [SerializeField]
    private Sprite changeImageName;

    public GameObject card;
    public GameObject other;

    public void cardButtonImageChange()
    {
        // ����� �̹����� ����.
        gameObject.GetComponent<Image>().sprite = changeImageName;
        gameObject.transform.GetChild(0).GetComponent<Text>().color = Color.white;
        other.GetComponent<Image>().sprite = originalImageName;
        other.transform.GetChild(0).GetComponent<Text>().color = Color.grey;
        // ���õǸ� FFFFFF

        // ���õ��� ������ 323232
    }

    public void otherButtonImageChange()
    {
        // ����� �̹����� ����.
        gameObject.GetComponent<Image>().sprite = changeImageName;
        gameObject.transform.GetChild(0).GetComponent<Text>().color = Color.white;
        card.GetComponent<Image>().sprite = originalImageName;
        card.transform.GetChild(0).GetComponent<Text>().color = Color.grey;
    }
}
