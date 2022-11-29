using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EquimentInventory : MonoBehaviour
{
    public static EquimentInventory instance;
    private void Awake()
    {
        instance = this;
    }

    [SerializeField]
    private Sprite warriorImage;
    [SerializeField]
    private Sprite wizardImage;
    [SerializeField]
    private Sprite backImage;
    [SerializeField]
    private GameObject selectedCharacter;
    [SerializeField]
    public string ClassName;
    [SerializeField]
    private GameObject playerButton;

    public bool setItemComplited;

    private bool isSelected;

    private void Start()
    {
        setItemComplited = false;
    }

    [SerializeField]
    private GameObject activeSkillText;
    [SerializeField]
    private GameObject passiveSkillText;
    [SerializeField]
    private GameObject attackTowerText;
    [SerializeField]
    private GameObject minionTowerText;
    [SerializeField]
    private GameObject passiveTowerText;

    // 1. 장착 슬롯이 변동 있는지 확인해서 업데이트한다.
    public void cardMonitorUpdate()
    {
        int activeSkill = 0;
        int passiveSkill = 0;
        int activeTower = 0;
        int minionTower = 0;
        int passiveTower = 0;
        // 현재 장착된 슬롯 수 저장
        for (int i = 0; i < gameObject.transform.GetChild(1).childCount; i++)
        {
            if (gameObject.transform.GetChild(1).GetChild(i).childCount != 0) // 아이템이 있으면
            {
                // 아이템의 obj타입을 확인한다.
                if (gameObject.transform.GetChild(1).GetChild(i).GetChild(0).GetComponent<ItemOnObject>().item.objType == "Active_S")
                {
                    activeSkill += 1;
                }
                else if (gameObject.transform.GetChild(1).GetChild(i).GetChild(0).GetComponent<ItemOnObject>().item.objType == "Passive_S")
                {
                    passiveSkill += 1;
                }
                else if (gameObject.transform.GetChild(1).GetChild(i).GetChild(0).GetComponent<ItemOnObject>().item.objType == "Active_T")
                {
                    activeTower += 1;
                }
                else if (gameObject.transform.GetChild(1).GetChild(i).GetChild(0).GetComponent<ItemOnObject>().item.objType == "Minion_T")
                {
                    minionTower += 1;
                }
                else if (gameObject.transform.GetChild(1).GetChild(i).GetChild(0).GetComponent<ItemOnObject>().item.objType == "Passive_T")
                {
                    passiveTower += 1;
                }
            }
        }
        activeSkillText.GetComponent<Text>().text = activeSkill.ToString();
        passiveSkillText.GetComponent<Text>().text = passiveSkill.ToString();
        attackTowerText.GetComponent<Text>().text = activeTower.ToString();
        minionTowerText.GetComponent<Text>().text = minionTower.ToString();
        passiveTowerText.GetComponent<Text>().text = passiveTower.ToString();
    }

    private void Update()
    {
        // 1, 2, 3 슬롯 확인
        SeletedCharacterChange();
    }

    private void SeletedCharacterChange()
    {
        int stack = 0;
        if (gameObject.transform.GetChild(1).GetChild(1).childCount != 0 || gameObject.transform.GetChild(1).GetChild(2).childCount != 0 || gameObject.transform.GetChild(1).GetChild(3).childCount != 0)
        {
            isSelected = true;
            if (isSelected)
            {
                for (int i = 1; i < 4; i++)
                {
                    if (gameObject.transform.GetChild(1).GetChild(i).childCount != 0)
                    {
                        stack++;
                    }
                }
                isSelected = false;
            }
        }
        if (stack == 3)
        {
            if (gameObject.transform.GetChild(1).GetChild(1).GetChild(0).gameObject.GetComponent<ItemOnObject>().item.ClassType == "Warrior")
            {
                selectedCharacter.GetComponent<Image>().sprite = warriorImage;
                ClassName = "Warrior";
                SelectCharacterSend();
            }
            if (gameObject.transform.GetChild(1).GetChild(1).GetChild(0).gameObject.GetComponent<ItemOnObject>().item.ClassType == "Wizard")
            {
                selectedCharacter.GetComponent<Image>().sprite = wizardImage;
                ClassName = "Wizard";
                SelectCharacterSend();
            }
        }
        else
        {
            selectedCharacter.GetComponent<Image>().sprite = backImage;
        }
    }

    // 아이템 장착 확인
    public void AllItemCheck()
    {
        if (setItemComplited)
        {
            playerButton.GetComponent<PlayerButton>().OnButton();
        }
    }


    private void SelectCharacterSend()
    {
        GameObject.FindGameObjectWithTag("GetCaller").GetComponent<TrojanHorse>().selectCharacter = ClassName;
    }

}
