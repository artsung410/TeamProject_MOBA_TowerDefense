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
