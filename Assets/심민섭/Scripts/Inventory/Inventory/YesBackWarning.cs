using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class YesBackWarning : MonoBehaviour
{
    // ###############################################
    //             NAME : Simstealer                      
    //             MAIL : minsub4400@gmail.com         
    // ###############################################
    [SerializeField]
    private GameObject charactor;
    [SerializeField]
    private GameObject text;

    // Ȱ��ȭ �ɶ� �ɸ��͸� ���� �ؽ�Ʈ�� �ٲ۴�.
    private void OnEnable()
    {
        if (charactor.GetComponent<Image>().sprite.name == "shiba")
            text.GetComponent<Text>().text = "You chose the current warrior character. \n Is it right ?";
        else if (charactor.GetComponent<Image>().sprite.name == "cat")
            text.GetComponent<Text>().text = "You chose the current wizard character. \n Is it right ?";
    }
}
