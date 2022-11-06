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

    // ��ư�� ������ �̹����� ����ȴ�.

    // �̹��� ������
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
        // ������ �������� Ÿ���� Ȯ���ؼ� ��� �κ����� Ȯ���Ѵ�.
        for (int i = 1; i < 4; i++) // 1 ~ 3 ����
        {
            if (eS.transform.GetChild(i).tag == "WarriorSlot")
            {
                // ������ ī�尡 ��� �ִٸ�
                // ������ ī�带 ������ �κ�Ʈ�� �־� �ش�. ��� �ִ� ����������
                /*GameObject[] eSSlots = GameObject.FindGameObjectsWithTag("WarriorSlot");
                foreach (var es in eSSlots)
                {
                    if (es.transform.GetChild(i).childCount == 0)
                    {
                        eS.transform.GetChild(i).SetParent(es.transform.GetChild(i));
                    }
                }*/
            }
            else if (eS.transform.GetChild(i).tag == "WizardSlot") // ���ڵ� �������� �ٲ����
            {
                // ������ ī�尡 ��� �ִٸ�
                // ������ ī�带 ������ �κ�Ʈ�� �־� �ش�. ��� �ִ� ����������
                
                //Debug.Log(GameObject.FindGameObjectWithTag("EquipmentSystem").transform.GetChild(1).name);
                if (eS.transform.GetChild(1).childCount == 0)
                {
                    return;
                }

                string eSSlots = eS.transform.GetChild(1).GetChild(0).gameObject.GetComponent<ItemOnObject>().item.ClassType;

                if (eSSlots == "Warrior")
                {
                    // ������ ī��� ������ �κ��丮�� �־���.
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
                    // ��ؽ� ī��� ��ؽ� �κ��丮�� �־���.
                }
            }
        }
        

        // ��ư�� ������ ������ ī�� �������� �κ� �������� �ǵ��ư���.
    }

    public void warriorButtonImageChange()
    {
        // ����� �̹����� ����.
        gameObject.GetComponent<Image>().sprite = changeImageName;
        wizard.GetComponent<Image>().sprite = originalImageName;
        assassin.GetComponent<Image>().sprite = originalImageName;
        inherence.GetComponent<Image>().sprite = originalImageName;

        // ���� ��ư�� ������ ���� 123������ �ױ׸� �����Ѵ�.
        for (int i = 1; i < 4; i++) // 1 ~ 3��, ���� �ݺ�
        {
            GameObject.FindGameObjectWithTag("EquipmentSystem").transform.GetChild(1).GetChild(i).gameObject.tag = "WarriorSlot";
        }
    }

    public void wizardButtonImageChange()
    {
        // ����� �̹����� ����.
        gameObject.GetComponent<Image>().sprite = changeImageName;
        warrior.GetComponent<Image>().sprite = originalImageName;
        assassin.GetComponent<Image>().sprite = originalImageName;
        inherence.GetComponent<Image>().sprite = originalImageName;
        // ������ ��ư�� ������ ���� 123������ �ױ׸� �����Ѵ�.
        for (int i = 1; i < 4; i++) // 1 ~ 3��, ���� �ݺ�
        {
            GameObject.FindGameObjectWithTag("EquipmentSystem").transform.GetChild(1).GetChild(i).gameObject.tag = "WizardSlot";
        }
        //eSItemChecker();
    }

    public void assassinButtonImageChange()
    {
        // ����� �̹����� ����.
        gameObject.GetComponent<Image>().sprite = changeImageName;
        warrior.GetComponent<Image>().sprite = originalImageName;
        wizard.GetComponent<Image>().sprite = originalImageName;
        inherence.GetComponent<Image>().sprite = originalImageName;
        // �ϻ��� ��ư�� ������ ���� 123������ �ױ׸� �����Ѵ�.
        for (int i = 1; i < 4; i++) // 1 ~ 3��, ���� �ݺ�
        {
            GameObject.FindGameObjectWithTag("EquipmentSystem").transform.GetChild(1).GetChild(i).gameObject.tag = "AssassinSlot";
        }
    }

    public void inherenceButtonImageChange()
    {
        // ����� �̹����� ����.
        gameObject.GetComponent<Image>().sprite = changeImageName;
        warrior.GetComponent<Image>().sprite = originalImageName;
        assassin.GetComponent<Image>().sprite = originalImageName;
        wizard.GetComponent<Image>().sprite = originalImageName;
    }
}
