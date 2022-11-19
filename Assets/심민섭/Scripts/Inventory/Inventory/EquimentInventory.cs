using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EquimentInventory : MonoBehaviour
{
    // 장착 아이템을 저장한다.
    private List<GameObject> equimentInventoryLIst = new List<GameObject>();

    [SerializeField]
    private Sprite warriorImage;
    [SerializeField]
    private Sprite wizardImage;
    [SerializeField]
    private Sprite backImage;
    [SerializeField]
    private GameObject selectedCharacter;

    private bool isSelected = false;
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
                selectedCharacter.GetComponent<Image>().sprite = warriorImage;
            if (gameObject.transform.GetChild(1).GetChild(1).GetChild(0).gameObject.GetComponent<ItemOnObject>().item.ClassType == "Wizard")
                selectedCharacter.GetComponent<Image>().sprite = wizardImage;
        }
        else
        {
            selectedCharacter.GetComponent<Image>().sprite = backImage;
        }
    }

}
