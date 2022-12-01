using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;

public class ConsumeItem : MonoBehaviour, IPointerDownHandler
{
    public Item item;
    private static Tooltip tooltip;

    // 케릭터 장비창에서 가져올 아이템 타입을 저장할 배열
    public ItemType[] itemTypeOfSlot;
    // 케릭터 장비창의 컴포넌트를 가져올 변수
    public static EquipmentSystem eS;

    public GameObject duplication;
    public static GameObject mainInventory;

    void Start()
    {
        item = GetComponent<ItemOnObject>().item;

        // 툴팁은 아이템 설명 스크립트
        if (GameObject.FindGameObjectWithTag("Tooltip") != null)
            tooltip = GameObject.FindGameObjectWithTag("Tooltip").GetComponent<Tooltip>();

        // 게임 오브젝트 태그 중에 "EquipmentSystem"가 있다면
        if (GameObject.FindGameObjectWithTag("EquipmentSystem") != null)
        {
            // "Player" 태그를 찾아서 PlayerInventory스크립트 안에 characterSystem 오브젝트의 EquipmentSystem스크립트를 가져온다.
            eS = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerInventory>().characterSystem.GetComponent<EquipmentSystem>();
        }

        if (GameObject.FindGameObjectWithTag("MainInventory") != null)
            mainInventory = GameObject.FindGameObjectWithTag("MainInventory");
    }

    // 포인터가 다운되었을 때
    public void OnPointerDown(PointerEventData data)
    {
        // EquipmentSystem(케릭터 장비 창) 컴포넌트가 null이면
        if (this.gameObject.transform.parent.parent.parent.parent.parent.GetComponent<EquipmentSystem>() == null)
        {
            bool gearable = false;
            // 인벤토리 컴포넌트를 가져온다.
            Inventory inventory = GameObject.FindGameObjectWithTag("MainInventory").GetComponent<Inventory>();
            //Inventory inventory = transform.parent.parent.parent.parent.parent.GetComponent<Inventory>();

            // EquipmentSystem(케릭터 장비 창) 컴포넌트를 가져오는데 성공했다면
            if (eS != null)
            {
                // 아이템 타입 배열 999개 짜리 공간을 전달 받음
                itemTypeOfSlot = eS.itemTypeOfSlots;
            }

            // data(pointer)가 오른쪽 클릭이면
            if (data.button == PointerEventData.InputButton.Right)
            {
                // 소비 타입과 같으면
                if (this.gameObject.GetComponent<ItemOnObject>().item.itemType == ItemType.Consumable)
                {
                    // 가지고 있는 아이템의 갯수가 10개 이하면 DrawManager x10 비활성화 함수 호출
                    if (this.gameObject.GetComponent<ItemOnObject>().item.itemValue < 10)
                    {
                        DrawManager.instance.ButtonDisable();
                    }
                    else
                    {
                        DrawManager.instance.ButtonEnable();
                    }
                    // 뽑기창을 띄워준다.
                    GameObject.FindGameObjectWithTag("DrawInventory").transform.GetChild(4).gameObject.SetActive(true);
                    // 뽑고 있는 박스에 대한 정보 저장
                    DrawManager.instance.boxItem = this.gameObject.GetComponent<ItemOnObject>();
                    DrawManager.instance.boxImage = this.gameObject.GetComponent<ItemOnObject>().item.itemIcon;
                    DrawManager.instance.boxName = this.gameObject.GetComponent<ItemOnObject>().item.itemName;
                }

                if (transform.parent.GetComponent<CraftResultSlot>() != null)
                {
                    // checkIfItemAllreadyExist(item.itemID, item.itemValue)을 사용하고 값을 check에 저장한다.
                    // checkIfItemAllreadyExist(item.itemID, item.itemValue) == 해당 아이템의 ID, Value를 넣고 인벤토리에 까지 넣게 된다.
                    bool check = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerInventory>().inventory.GetComponent<Inventory>().checkIfItemAllreadyExist(item.itemID, item.itemValue);

                    // 아이템이 인벤토리에 업데이트가 되지 않았다면
                    if (!check)
                    {
                        // 아이템을 추가한다
                        GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerInventory>().inventory.GetComponent<Inventory>().addItemToInventory(item.itemID, item.itemValue);
                        GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerInventory>().inventory.GetComponent<Inventory>().stackableSettings();
                    }
                    // 툴팁 비활성화
                    tooltip.deactivateTooltip();
                    gearable = true;
                    // 아이템 인벤토리에 업데이트
                    GameObject.FindGameObjectWithTag("MainInventory").GetComponent<Inventory>().updateItemList();
                }
                else // 아이템이 인벤토리에 업데이트가 되었다면
                {
                    // 제작 오브젝트가 없기 때문에 무조건 여기로 들어옴.
                    //bool stop = false;
                    // EquipmentSystem(케릭터 장비 창) 컴포넌트를 가져오는데 성공했다면
                    if (eS != null)
                    {
                        // eS.slotsInTotal 인벤토리 슬롯 갯수만큼 반복
                        for (int i = 0; i < eS.slotsInTotal; i++)
                        {
                            // 아이템[999] 배열에 있는 타입과 item.itemType를 비교한다.
                            if (itemTypeOfSlot[i].Equals(item.itemType))
                            {
                                if (eS.transform.GetChild(1).GetChild(i).childCount == 1)
                                {
                                    //Debug.Log(eS.transform.GetChild(1).GetChild(i).GetChild(0).gameObject.name); // itemIcon(clone)
                                    // 같은 카드는 한 개 이상 장착할 수 없게 하는 코드
                                    if (item.itemName == eS.transform.GetChild(1).GetChild(i).GetChild(0).GetComponent<ItemOnObject>().item.itemName)
                                    {
                                        return;
                                    }
                                    // 클래스가 다를때 카드를 장착할 수 없게 하는 코드
                                    if (item.ClassType != eS.transform.GetChild(1).GetChild(i).GetChild(0).GetComponent<ItemOnObject>().item.ClassType)
                                    {
                                        return;
                                    }

                                    // 미니언 근거리 데이터 range
                                    if (item.itemType == ItemType.Tower && item.objType == "Minion_T")
                                    {
                                        if (eS.transform.GetChild(1).GetChild(i).GetChild(0).GetComponent<ItemOnObject>().item.objType == "Minion_T" && (int)CSVtest.Instance.MinionDic[eS.transform.GetChild(1).GetChild(i).GetChild(0).GetComponent<ItemOnObject>().item.towerData.MinionID].Range == 5)
                                        {
                                            int minionRangeData = (int)CSVtest.Instance.MinionDic[item.towerData.MinionID].Range;
                                            if (minionRangeData == 5)
                                            {
                                                return;
                                            }
                                        }
                                        if (eS.transform.GetChild(1).GetChild(i).GetChild(0).GetComponent<ItemOnObject>().item.objType == "Minion_T" && (int)CSVtest.Instance.MinionDic[eS.transform.GetChild(1).GetChild(i).GetChild(0).GetComponent<ItemOnObject>().item.towerData.MinionID].Range == 25)
                                        {
                                            int minionRangeData = (int)CSVtest.Instance.MinionDic[item.towerData.MinionID].Range;
                                            if (minionRangeData == 25)
                                            {
                                                return;
                                            }
                                        }
                                    }
                                    
                                }

                                // 장비창 슬롯 하위 오브젝트가 있는가 없는가, 없으면 == 0 있으면 1이상
                                // 하위 오브젝트가 없으면, 0이면
                                // 장착 창의 슬롯이 비워있다면 아이템을 장착하는 코드
                                if (eS.transform.GetChild(1).GetChild(i).childCount == 0)
                                {
                                    //stop = true;
                                    if (eS.transform.GetChild(1).GetChild(i).parent.parent.GetComponent<EquipmentSystem>() != null && this.gameObject.transform.parent.parent.parent.parent.parent.GetComponent<EquipmentSystem>() != null) { }
                                    else
                                    {
                                        // 하나라도 null이면 아이템을 장착한다.
                                        inventory.EquiptItem(item);
                                    }

                                    // 장착 슬롯에 Item 오브젝트를 생성한다.
                                    // 그냥 옮기면 안되고 갯수를 파악해서 1 기준으로 큰지 작은지 확인한다.
                                    //Debug.Log($"item.itemName : {item.itemName}");
                                    //Debug.Log($"item.itemValue : {item.itemValue}");
                                    
                                    // 우클릭 시 아이템 갯수 감소 코드
                                    if (item.itemValue <= 1) // 1 보다 작거나 같으면
                                    {
                                        // 통으로 옮김.
                                        this.gameObject.transform.SetParent(eS.transform.GetChild(1).GetChild(i)); // 장착 슬롯으로 옮기는 코드
                                    }
                                    else // 1 보다 크다면
                                    {
                                        item.itemValue -= 1;

                                        //Debug.Log(eS.transform.GetChild(1).name);
                                        //Debug.Log(eS.transform.GetChild(1).GetChild(i).name); // Slot(1)
                                        if (eS.transform.GetChild(1).GetChild(i).childCount == 0)
                                        {
                                            Instantiate(this.gameObject.transform, eS.transform.GetChild(1).GetChild(i));
                                            // 장착 슬롯 크기 지정
                                            eS.transform.GetChild(1).GetChild(i).GetChild(0).localScale = new Vector3(0.9166667f, 1.05f, 0f);
                                            //Debug.Log(eS.transform.GetChild(1).GetChild(i).GetChild(0).name);
                                            if (eS.transform.GetChild(1).GetChild(i).GetChild(0).GetComponent<ItemOnObject>().item.itemValue >= 1)
                                            {
                                                eS.transform.GetChild(1).GetChild(i).GetChild(0).GetComponent<ItemOnObject>().item.itemValue = 1;
                                            }
                                        }
                                    }

                                    this.gameObject.GetComponent<RectTransform>().localPosition = Vector3.zero;
                                    eS.gameObject.GetComponent<Inventory>().updateItemList();
                                    inventory.updateItemList();
                                    gearable = true;
                                    // 카드 현황 업데이트
                                    EquimentInventory.instance.cardMonitorUpdate();
                                    /*if (duplication != null)
                                        Destroy(duplication.gameObject);*/
                                    break;
                                }
                            }
                        }

                        // 현 상황에선 무조건 실행됨
                        /*if (!stop)
                        {
                            // eS 장착 슬롯 갯수만큼 반복
                            for (int i = 0; i < eS.slotsInTotal; i++)
                            {
                                // 아이템[99] 배열에 있는 타입과 item.itemType를 비교하고
                                if (itemTypeOfSlot[i].Equals(item.itemType))
                                {
                                    // 장비창 슬롯 하위 오브젝트가 있으면
                                    if (eS.transform.GetChild(1).GetChild(i).childCount != 0)
                                    {
                                        // 장착중인 Item 오브젝트를 otherItemFromCharacterSystem에 저장한다.
                                        GameObject otherItemFromCharacterSystem = eS.transform.GetChild(1).GetChild(i).GetChild(0).gameObject;
                                        // 장착한 아이템의 정보도 otherSlotItem에 저장한다.
                                        Item otherSlotItem = otherItemFromCharacterSystem.GetComponent<ItemOnObject>().item;

                                        // 들어온 아이템의 타입이 현재 장착중인 아이템 타입과 같다면
                                        *//*if (item.itemType == otherSlotItem.itemType)
                                        {
                                            // 장착 중인 아이템을 장착 해제하고
                                            inventory.UnEquipItem1(otherItemFromCharacterSystem.GetComponent<ItemOnObject>().item);
                                            // 들어온 아이템을 장착한다.
                                            inventory.EquiptItem(item);
                                        }
                                        else // 들어온 아이템의 타입이 장착 중인 아이템과 다르다면
                                        {
                                            // 들어온 아이템을 장착한다.
                                            inventory.EquiptItem(item);
                                            // 아이템 타입이 Backpack이 아니면
                                            if (item.itemType != ItemType.Backpack)
                                            {
                                                // 장착 중인 아이템을 장착 해제
                                                inventory.UnEquipItem1(otherItemFromCharacterSystem.GetComponent<ItemOnObject>().item);
                                            }
                                        }*//*
                                        gearable = true;
                                        if (duplication != null)
                                            Destroy(duplication.gameObject);
                                        eS.gameObject.GetComponent<Inventory>().updateItemList();
                                        inventory.OnUpdateItemList();
                                        break;
                                    }
                                }
                            }
                        }*/

                    }

                }
                /*if (!gearable && item.itemType != ItemType.UFPS_Ammo && item.itemType != ItemType.UFPS_Grenade)
                {

                    Item itemFromDup = null;
                    if (duplication != null)
                        itemFromDup = duplication.GetComponent<ItemOnObject>().item;

                    inventory.ConsumeItem(item);

                    item.itemValue--;
                    if (itemFromDup != null)
                    {
                        duplication.GetComponent<ItemOnObject>().item.itemValue--;
                        if (itemFromDup.itemValue <= 0)
                        {
                            if (tooltip != null)
                                tooltip.deactivateTooltip();
                            inventory.deleteItemFromInventory(item);
                            Destroy(duplication.gameObject);
                        }
                    }
                    if (item.itemValue <= 0)
                    {
                        if (tooltip != null)
                            tooltip.deactivateTooltip();
                        inventory.deleteItemFromInventory(item);
                        Destroy(this.gameObject);
                    }
                }*/

            }


        } // 큰 if 문 끝.
    }
}
