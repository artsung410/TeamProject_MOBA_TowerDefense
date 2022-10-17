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
            Inventory inventory = transform.parent.parent.parent.parent.parent.GetComponent<Inventory>();
            
            // EquipmentSystem(케릭터 장비 창) 컴포넌트를 가져오는데 성공했다면
            if (eS != null)
            {
                // 아이템 타입 배열 999개 짜리 공간을 전달 받음
                itemTypeOfSlot = eS.itemTypeOfSlots;
            }

            // data가 오른쪽 클릭이면
            if (data.button == PointerEventData.InputButton.Right)
            {
                //item from craft system to inventory
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
                    //CraftSystem cS = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerInventory>().craftSystem.GetComponent<CraftSystem>();
                    //cS.deleteItems(item);
                    //CraftResultSlot result = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerInventory>().craftSystem.transform.GetChild(3).GetComponent<CraftResultSlot>();
                    //result.temp = 0;
                    // 툴팁 비활성화
                    tooltip.deactivateTooltip();
                    gearable = true;
                    // 아이템 인벤토리에 업데이트
                    GameObject.FindGameObjectWithTag("MainInventory").GetComponent<Inventory>().updateItemList();
                }
                else // 아이템이 인벤토리에 업데이트가 되었다면
                {
                    bool stop = false;
                    // EquipmentSystem(케릭터 장비 창) 컴포넌트를 가져오는데 성공했다면
                    if (eS != null)
                    {
                        // eS.slotsInTotal 인벤토리 슬롯 갯수만큼 반복
                        for (int i = 0; i < eS.slotsInTotal; i++)
                        {
                            // 아이템[999] 배열에 있는 타입과 item.itemType를 비교한다.
                            if (itemTypeOfSlot[i].Equals(item.itemType))
                            {
                                // 장비창 슬롯 하위 오브젝트가 있는가 없는가, 없으면 == 0 있으면 1이상
                                // 하위 오브젝트가 없으면, 0이면
                                if (eS.transform.GetChild(1).GetChild(i).childCount == 0)
                                {
                                    stop = true;
                                    if (eS.transform.GetChild(1).GetChild(i).parent.parent.GetComponent<EquipmentSystem>() != null && this.gameObject.transform.parent.parent.parent.parent.parent.GetComponent<EquipmentSystem>() != null) { }
                                    else
                                    {
                                        // 하나라도 null이면 아이템을 장착한다.
                                        inventory.EquiptItem(item);
                                    }
                                    // 장착 슬롯에 Item 오브젝트를 생성한다.
                                    this.gameObject.transform.SetParent(eS.transform.GetChild(1).GetChild(i));
                                    this.gameObject.GetComponent<RectTransform>().localPosition = Vector3.zero;
                                    eS.gameObject.GetComponent<Inventory>().updateItemList();
                                    inventory.updateItemList();
                                    gearable = true;
                                    if (duplication != null)
                                        Destroy(duplication.gameObject);
                                    break;
                                }
                            }
                        }


                        if (!stop)
                        {
                            // eS.slotsInTotal 인벤토리 슬롯 갯수만큼 반복
                            for (int i = 0; i < eS.slotsInTotal; i++)
                            {
                                // 아이템[999] 배열에 있는 타입과 item.itemType를 비교한다.
                                if (itemTypeOfSlot[i].Equals(item.itemType))
                                {
                                    // 장비창 슬롯 하위 오브젝트가 있으면
                                    if (eS.transform.GetChild(1).GetChild(i).childCount != 0)
                                    {
                                        // 장착중인 Item 오브젝트를 otherItemFromCharacterSystem에 저장한다.
                                        GameObject otherItemFromCharacterSystem = eS.transform.GetChild(1).GetChild(i).GetChild(0).gameObject;
                                        // 장착한 아이템의 정보도 otherSlotItem에 저장한다.
                                        Item otherSlotItem = otherItemFromCharacterSystem.GetComponent<ItemOnObject>().item;

                                        // 들어온 아이템의 타입이 UFPS_Weapon이면
                                        if (item.itemType == ItemType.UFPS_Weapon)
                                        {
                                            // 장착 중인 아이템을 장착 해제하고
                                            inventory.UnEquipItem1(otherItemFromCharacterSystem.GetComponent<ItemOnObject>().item);
                                            // 들어온 아이템을 장착한다.
                                            inventory.EquiptItem(item);
                                        }
                                        else // 들어온 아이템의 타입이 UFPS_Weapon이 아니면
                                        {
                                            // 들어온 아이템을 장착한다.
                                            inventory.EquiptItem(item);
                                            // 아이템 타입이 Backpack이 아니면
                                            if (item.itemType != ItemType.Backpack)
                                            {
                                                // 장착 중인 아이템을 장착 해제
                                                inventory.UnEquipItem1(otherItemFromCharacterSystem.GetComponent<ItemOnObject>().item);
                                            }
                                        }


                                        // 자기 자신이 null이면, 끼고 있던 아이템을 장착 해제한 상태
                                        if (this == null)
                                        {
                                            // 해제한 아이템의 프리펩을 복제해서 dropItem에 넣음
                                            GameObject dropItem = (GameObject)Instantiate(otherSlotItem.itemModel);
                                            dropItem.AddComponent<PickUpItem>();
                                            dropItem.GetComponent<PickUpItem>().item = otherSlotItem;
                                            dropItem.transform.localPosition = GameObject.FindGameObjectWithTag("Player").transform.localPosition;
                                            inventory.OnUpdateItemList();
                                        }
                                        else
                                        {
                                            otherItemFromCharacterSystem.transform.SetParent(this.transform.parent);
                                            otherItemFromCharacterSystem.GetComponent<RectTransform>().localPosition = Vector3.zero;
                                            if (this.gameObject.transform.parent.parent.parent.parent.parent.GetComponent<Hotbar>() != null)
                                                createDuplication(otherItemFromCharacterSystem);

                                            this.gameObject.transform.SetParent(eS.transform.GetChild(1).GetChild(i));
                                            this.gameObject.GetComponent<RectTransform>().localPosition = Vector3.zero;
                                        }                                        
                                        
                                        gearable = true;                                        
                                        if (duplication != null)
                                            Destroy(duplication.gameObject);
                                        eS.gameObject.GetComponent<Inventory>().updateItemList();
                                        inventory.OnUpdateItemList();
                                        break;
                                    }
                                }
                            }
                        }

                    }

                }
                if (!gearable && item.itemType != ItemType.UFPS_Ammo && item.itemType != ItemType.UFPS_Grenade)
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

                }
                
            }
            

        } // 큰 if 문 끝.
    }    

    public void consumeIt()
    {
        Inventory inventory = transform.parent.parent.parent.GetComponent<Inventory>();

        bool gearable = false;

        if (GameObject.FindGameObjectWithTag("EquipmentSystem") != null)
            eS = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerInventory>().characterSystem.GetComponent<EquipmentSystem>();

        if (eS != null)
            itemTypeOfSlot = eS.itemTypeOfSlots;

        Item itemFromDup = null;
        if (duplication != null)
            itemFromDup = duplication.GetComponent<ItemOnObject>().item;       

        bool stop = false;
        if (eS != null)
        {
            
            for (int i = 0; i < eS.slotsInTotal; i++)
            {
                if (itemTypeOfSlot[i].Equals(item.itemType))
                {
                    if (eS.transform.GetChild(1).GetChild(i).childCount == 0)
                    {
                        stop = true;
                        this.gameObject.transform.SetParent(eS.transform.GetChild(1).GetChild(i));
                        this.gameObject.GetComponent<RectTransform>().localPosition = Vector3.zero;
                        eS.gameObject.GetComponent<Inventory>().updateItemList();
                        inventory.updateItemList();
                        inventory.EquiptItem(item);
                        gearable = true;
                        if (duplication != null)
                            Destroy(duplication.gameObject);
                        break;
                    }
                }
            }

            if (!stop)
            {
                for (int i = 0; i < eS.slotsInTotal; i++)
                {
                    if (itemTypeOfSlot[i].Equals(item.itemType))
                    {
                        if (eS.transform.GetChild(1).GetChild(i).childCount != 0)
                        {
                            GameObject otherItemFromCharacterSystem = eS.transform.GetChild(1).GetChild(i).GetChild(0).gameObject;
                            Item otherSlotItem = otherItemFromCharacterSystem.GetComponent<ItemOnObject>().item;
                            if (item.itemType == ItemType.UFPS_Weapon)
                            {
                                inventory.UnEquipItem1(otherItemFromCharacterSystem.GetComponent<ItemOnObject>().item);
                                inventory.EquiptItem(item);
                            }
                            else
                            {
                                inventory.EquiptItem(item);
                                if (item.itemType != ItemType.Backpack)
                                    inventory.UnEquipItem1(otherItemFromCharacterSystem.GetComponent<ItemOnObject>().item);
                            }
                            if (this == null)
                            {
                                GameObject dropItem = (GameObject)Instantiate(otherSlotItem.itemModel);
                                dropItem.AddComponent<PickUpItem>();
                                dropItem.GetComponent<PickUpItem>().item = otherSlotItem;
                                dropItem.transform.localPosition = GameObject.FindGameObjectWithTag("Player").transform.localPosition;
                                inventory.OnUpdateItemList();
                            }
                            else
                            {
                                otherItemFromCharacterSystem.transform.SetParent(this.transform.parent);
                                otherItemFromCharacterSystem.GetComponent<RectTransform>().localPosition = Vector3.zero;
                                if (this.gameObject.transform.parent.parent.parent.parent.parent.GetComponent<Hotbar>() != null)
                                    createDuplication(otherItemFromCharacterSystem);

                                this.gameObject.transform.SetParent(eS.transform.GetChild(1).GetChild(i));
                                this.gameObject.GetComponent<RectTransform>().localPosition = Vector3.zero;
                            }

                            gearable = true;
                            if (duplication != null)
                                Destroy(duplication.gameObject);
                            eS.gameObject.GetComponent<Inventory>().updateItemList();
                            inventory.OnUpdateItemList();
                            break;                           
                        }
                    }
                }
            }


        }
        if (!gearable && item.itemType != ItemType.UFPS_Ammo && item.itemType != ItemType.UFPS_Grenade)
        {

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

        }        
    }

    public void createDuplication(GameObject Item)
    {
        Item item = Item.GetComponent<ItemOnObject>().item;
        GameObject dup = mainInventory.GetComponent<Inventory>().addItemToInventory(item.itemID, item.itemValue);
        Item.GetComponent<ConsumeItem>().duplication = dup;
        dup.GetComponent<ConsumeItem>().duplication = Item;
    }
}
