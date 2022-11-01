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

    // 드래그 아이템을 확인해서 skill이면 123 슬롯을 컬러 변경한다.
    // tower이면 4567 슬롯을 컬러 변경한다.

    // 장착 슬롯 저장할 배열
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
        // 예외 처리
        // 하위 오브젝트가 0이면 리턴
        if (GameObject.FindGameObjectWithTag("DraggingItem").transform.childCount == 0)
        {
            return;
        }

        // 드래그 아이템 찾기
        GameObject draggingItem = GameObject.FindGameObjectWithTag("DraggingItem").transform.GetChild(0).gameObject;
        ItemOnObject itemOnObject = draggingItem.GetComponent<ItemOnObject>();
        //Debug.Log(itemOnObject.item.itemType.ToString());
        if (itemOnObject.item.itemType.ToString() == "Skill")
        {
            // 스킬 타입이면 123 슬롯 변경
            color = Color.yellow;
            EquipmentArr[1].GetComponent<Image>().color = color;
            EquipmentArr[2].GetComponent<Image>().color = color;
            EquipmentArr[3].GetComponent<Image>().color = color;

            // 다른쪽은 바꿔줌
            color = Color.white;
            EquipmentArr[4].GetComponent<Image>().color = color;
            EquipmentArr[5].GetComponent<Image>().color = color;
            EquipmentArr[6].GetComponent<Image>().color = color;
            EquipmentArr[7].GetComponent<Image>().color = color;
        }
        else if (itemOnObject.item.itemType.ToString() == "Tower")
        {
            // 스킬 타입이면 123 슬롯 변경
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
            // 스킬 타입이면 0 슬롯 변경
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
