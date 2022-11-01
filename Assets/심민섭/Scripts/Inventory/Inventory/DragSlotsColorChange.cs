using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DragSlotsColorChange : MonoBehaviour
{
    // ###############################################
    //             NAME : Simstealer                      
    //             MAIL : minsub4400@gmail.com         
    // ###############################################

    // �巡�� �������� Ȯ���ؼ� skill�̸� 123 ������ �÷� �����Ѵ�.
    // tower�̸� 4567 ������ �÷� �����Ѵ�.

    // ���� ���� ������ �迭
    private GameObject[] EquipmentArr = new GameObject[8]; // 0 ~ 7*/
    private Color color;

    private void Awake()
    {
        for (int i = 0; i < EquipmentArr.Length; i++)
        {
            EquipmentArr[i] = GameObject.FindGameObjectWithTag("EquipmentSystem").transform.GetChild(1).GetChild(i).gameObject;
        }
    }


    public void SlotsColorChange()
    {
        // ���� ó��
        // ���� ������Ʈ�� 0�̸� ����
        if (GameObject.FindGameObjectWithTag("DraggingItem").transform.childCount == 0)
        {
            return;
        }

        // �巡�� ������ ã��
        GameObject draggingItem = GameObject.FindGameObjectWithTag("DraggingItem").transform.GetChild(0).gameObject;
        ItemOnObject itemOnObject = draggingItem.GetComponent<ItemOnObject>();
        //Debug.Log(itemOnObject.item.itemType.ToString());
        if (itemOnObject.item.itemType.ToString() == "Skill")
        {
            // ��ų Ÿ���̸� 123 ���� ����
            color = Color.yellow;
            EquipmentArr[1].GetComponent<Image>().color = color;
            EquipmentArr[2].GetComponent<Image>().color = color;
            EquipmentArr[3].GetComponent<Image>().color = color;

            // �ٸ����� �ٲ���
            color = Color.white;
            EquipmentArr[4].GetComponent<Image>().color = color;
            EquipmentArr[5].GetComponent<Image>().color = color;
            EquipmentArr[6].GetComponent<Image>().color = color;
            EquipmentArr[7].GetComponent<Image>().color = color;
        }
        else if (itemOnObject.item.itemType.ToString() == "Tower")
        {
            // ��ų Ÿ���̸� 123 ���� ����
            color = Color.yellow;
            EquipmentArr[4].GetComponent<Image>().color = color;
            EquipmentArr[5].GetComponent<Image>().color = color;
            EquipmentArr[6].GetComponent<Image>().color = color;
            EquipmentArr[7].GetComponent<Image>().color = color;

            color = Color.white;
            EquipmentArr[1].GetComponent<Image>().color = color;
            EquipmentArr[2].GetComponent<Image>().color = color;
            EquipmentArr[3].GetComponent<Image>().color = color;
        }
        else if (itemOnObject.item.itemType.ToString() == "uniqueSkill")
        {
            // ��ų Ÿ���̸� 0 ���� ����
            color = Color.yellow;
            EquipmentArr[0].GetComponent<Image>().color = color;

            color = Color.white;
            EquipmentArr[1].GetComponent<Image>().color = color;
            EquipmentArr[2].GetComponent<Image>().color = color;
            EquipmentArr[3].GetComponent<Image>().color = color;
            EquipmentArr[4].GetComponent<Image>().color = color;
            EquipmentArr[5].GetComponent<Image>().color = color;
            EquipmentArr[6].GetComponent<Image>().color = color;
            EquipmentArr[7].GetComponent<Image>().color = color;
        }
        else
        {
            return;
        }
    }

    public void SlotsWhiteColorChange()
    {
        color = Color.white;
        for (int i = 0; i < EquipmentArr.Length; i++)
        {
            EquipmentArr[i].GetComponent<Image>().color = color;
        }
    }
}
