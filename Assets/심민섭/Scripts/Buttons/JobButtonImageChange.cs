using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class JobButtonImageChange : MonoBehaviour
{
    // ###############################################
    //             NAME : Simstealer                      
    //             MAIL : minsub4400@gmail.com         
    // ###############################################

    // 버튼이 눌리면 이미지가 변경된다.

    // 이미지 데이터
    [SerializeField]
    private Sprite originalImageName;
    [SerializeField]
    private Sprite changeImageName;

    public GameObject warrior;
    public GameObject wizard;
    public GameObject assassin;
    public GameObject inherence;

    public void warriorButtonImageChange()
    {
        // 변경될 이미지를 넣음.
        gameObject.GetComponent<Image>().sprite = changeImageName;
        wizard.GetComponent<Image>().sprite = originalImageName;
        assassin.GetComponent<Image>().sprite = originalImageName;
        inherence.GetComponent<Image>().sprite = originalImageName;

        // 전사 버튼을 누르면 장착 123슬롯의 테그를 변경한다.
        for (int i = 1; i < 4; i++) // 1 ~ 3번, 세번 반복
        {
            GameObject.FindGameObjectWithTag("EquipmentSystem").transform.GetChild(1).GetChild(i).gameObject.tag = "WarriorSlot";
        }
    }

    public void wizardButtonImageChange()
    {
        // 변경될 이미지를 넣음.
        gameObject.GetComponent<Image>().sprite = changeImageName;
        warrior.GetComponent<Image>().sprite = originalImageName;
        assassin.GetComponent<Image>().sprite = originalImageName;
        inherence.GetComponent<Image>().sprite = originalImageName;
        // 마법사 버튼을 누르면 장착 123슬롯의 테그를 변경한다.
        for (int i = 1; i < 4; i++) // 1 ~ 3번, 세번 반복
        {
            GameObject.FindGameObjectWithTag("EquipmentSystem").transform.GetChild(1).GetChild(i).gameObject.tag = "WizardSlot";
        }
    }

    public void assassinButtonImageChange()
    {
        // 변경될 이미지를 넣음.
        gameObject.GetComponent<Image>().sprite = changeImageName;
        warrior.GetComponent<Image>().sprite = originalImageName;
        wizard.GetComponent<Image>().sprite = originalImageName;
        inherence.GetComponent<Image>().sprite = originalImageName;
        // 암살자 버튼을 누르면 장착 123슬롯의 테그를 변경한다.
        for (int i = 1; i < 4; i++) // 1 ~ 3번, 세번 반복
        {
            GameObject.FindGameObjectWithTag("EquipmentSystem").transform.GetChild(1).GetChild(i).gameObject.tag = "AssassinSlot";
        }
    }

    public void inherenceButtonImageChange()
    {
        // 변경될 이미지를 넣음.
        gameObject.GetComponent<Image>().sprite = changeImageName;
        warrior.GetComponent<Image>().sprite = originalImageName;
        assassin.GetComponent<Image>().sprite = originalImageName;
        wizard.GetComponent<Image>().sprite = originalImageName;
    }
}
