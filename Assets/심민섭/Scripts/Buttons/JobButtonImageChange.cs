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

    private GameObject eS;


    private void eSItemChecker()
    {
        eS = GameObject.FindGameObjectWithTag("EquipmentSystem").transform.GetChild(1).gameObject;
        // 장착된 아이템의 타입을 확인해서 어느 인벤인지 확인한다.
        for (int i = 1; i < 4; i++) // 1 ~ 3 슬롯
        {
            if (eS.transform.GetChild(i).tag == "WarriorSlot")
            {
                // 워리어 카드가 들어 있다면
                // 워리어 카드를 워리어 인벤트로 넣어 준다. 비어 있는 순차적으로
                /*GameObject[] eSSlots = GameObject.FindGameObjectsWithTag("WarriorSlot");
                foreach (var es in eSSlots)
                {
                    if (es.transform.GetChild(i).childCount == 0)
                    {
                        eS.transform.GetChild(i).SetParent(es.transform.GetChild(i));
                    }
                }*/
            }
            else if (eS.transform.GetChild(i).tag == "WizardSlot") // 위자드 슬롯으로 바뀌었음
            {
                // 워리어 카드가 들어 있다면
                // 워리어 카드를 워리어 인벤트로 넣어 준다. 비어 있는 순차적으로
                
                //Debug.Log(GameObject.FindGameObjectWithTag("EquipmentSystem").transform.GetChild(1).name);
                if (eS.transform.GetChild(1).childCount == 0)
                {
                    return;
                }

                string eSSlots = eS.transform.GetChild(1).GetChild(0).gameObject.GetComponent<ItemOnObject>().item.ClassType;

                if (eSSlots == "Warrior")
                {
                    // 워리어 카드면 워리어 인벤토리로 넣어줌.
                    //Debug.Log(GameObject.FindGameObjectWithTag("WarriorTab").gameObject.name);
                    GameObject war = gameObject.transform.parent.GetChild(0).GetChild(0).GetChild(0).GetChild(0).gameObject;

                    for (int j = 0; j < war.transform.childCount; j++)
                    {
                        if (war.transform.GetChild(j).childCount == 0)
                        {
                            eS.transform.GetChild(i).GetChild(0).SetParent(war.transform.GetChild(j));
                        }
                    }
                }
                else if (eSSlots == "Assassin")
                {
                    // 어쌔신 카드면 어쌔신 인벤토리로 넣어줌.
                }
            }
        }
        

        // 버튼을 누르면 장착된 카드 아이템이 인벤 슬롯으로 되돌아간다.
    }

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
        //eSItemChecker();
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
