using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class DragItem : MonoBehaviour, IDragHandler, IPointerDownHandler, IEndDragHandler
{
    private Vector2 pointerOffset;
    private RectTransform rectTransform;
    private RectTransform rectTransformSlot;
    private CanvasGroup canvasGroup;
    private GameObject oldSlot;
    private Inventory inventory;
    private Transform draggedItemBox;
    private GameObject dragItemObj;
    private ItemOnObject dragItem;

// 케릭터 장비창의 컴포넌트를 가져올 변수
    public static EquipmentSystem eS;

    public delegate void ItemDelegate();
    public static event ItemDelegate updateInventoryList;
    void Start()
    {
        // Rect 위치
        rectTransform = GetComponent<RectTransform>();
        // 캔버스 그룹 컴포넌트 가져옴
        canvasGroup = GetComponent<CanvasGroup>();
        // 드래그아이템 Rect 위치
        rectTransformSlot = GameObject.FindGameObjectWithTag("DraggingItem").GetComponent<RectTransform>();
        // 인벤토리 컴포넌트
        inventory = GameObject.FindGameObjectWithTag("MainInventory").GetComponent<Inventory>();
        // 드래그 아이템 박스 위치
        draggedItemBox = GameObject.FindGameObjectWithTag("DraggingItem").transform;

        if (GameObject.FindGameObjectWithTag("EquipmentSystem") != null)
        {
            // "Player" 태그를 찾아서 PlayerInventory스크립트 안에 characterSystem 오브젝트의 EquipmentSystem스크립트를 가져온다.
            eS = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerInventory>().characterSystem.GetComponent<EquipmentSystem>();
        }
    }


    // 드래그 하는 동안, 마우스를 움직일때 마다 실행됨
    public void OnDrag(PointerEventData data)
    {
        // Rect 위치가 null이면
        if (rectTransform == null)
            return;

        // 드래그 아이템 정보 저장하기
        if (draggedItemBox.childCount != 0)
        {
            dragItemObj = GameObject.FindGameObjectWithTag("DraggingItem").transform.GetChild(0).gameObject;
            dragItem = dragItemObj.GetComponent<ItemOnObject>();
        }

        // 왼 클릭이고 CraftResultSlot이 null이면
        if (data.button == PointerEventData.InputButton.Left && transform.parent.GetComponent<CraftResultSlot>() == null)
        {
            /*Transform.SetAsLastSibling
            해당 오브젝트의 순위를 마지막으로 변경(가장 나중에 출력되므로 겹쳐졋을 경우 앞으로 나옵니다.)

            Transform.SetAsFirstSibling
            해당 오브젝트의 순위를 처음으로 변경(가장 처음 출력되므로 겹쳐졋을 경우 가려집니다.)
            
            Transform.SetSiblingIndex(int nIndex)
            nIndex를 매개변수를 넣어서 순위를 지정합니다.(0이 처음입니다.)
            
            Transform.GetSiblingIndex()
            해당 오브젝트의 순위를 얻어옵니다.*/

            rectTransform.SetAsLastSibling();
            transform.SetParent(draggedItemBox);
            Vector2 localPointerPosition;
            canvasGroup.blocksRaycasts = false;
            if (RectTransformUtility.ScreenPointToLocalPointInRectangle(rectTransformSlot, Input.mousePosition, data.pressEventCamera, out localPointerPosition))
            {
                rectTransform.localPosition = localPointerPosition - pointerOffset;
                if (transform.GetComponent<ConsumeItem>().duplication != null)
                    Destroy(transform.GetComponent<ConsumeItem>().duplication);
            }
        }

        inventory.OnUpdateItemList();
    }

    public void OnPointerDown(PointerEventData data)
    {
        if (data.button == PointerEventData.InputButton.Left)
        {
            RectTransformUtility.ScreenPointToLocalPointInRectangle(rectTransform, data.position, data.pressEventCamera, out pointerOffset);
            oldSlot = transform.parent.gameObject;
        }
        if (updateInventoryList != null)
            updateInventoryList();
    }

    public void createDuplication(GameObject Item)
    {
        Item item = Item.GetComponent<ItemOnObject>().item;
        GameObject duplication = GameObject.FindGameObjectWithTag("MainInventory").GetComponent<Inventory>().addItemToInventory(item.itemID, item.itemValue);
        duplication.transform.parent.parent.parent.parent.parent.GetComponent<Inventory>().stackableSettings();
        Item.GetComponent<ConsumeItem>().duplication = duplication;
        duplication.GetComponent<ConsumeItem>().duplication = Item;
    }


    private string classType;

    public void OnEndDrag(PointerEventData data)
    {
        // 아이템 삭제
        /*Transform trashCan = null;
        if (data.pointerEnter != null)
            trashCan = data.pointerEnter.transform;
        if (trashCan.gameObject.name == "TrashCan")
        {
            Destroy(draggedItemBox.GetChild(0).gameObject);
        }*/
        // -------------------------------------------------------
        if (data.button == PointerEventData.InputButton.Left)
        {
            canvasGroup.blocksRaycasts = true;
            Transform newSlot = null;
            if (data.pointerEnter != null)
                newSlot = data.pointerEnter.transform;

            if (newSlot != null)
            {
                // 내 게임 오브젝트(Slot)
                GameObject firstItemGameObject = this.gameObject;
                // 마우스 포인터의 상위 게임 오브젝트(Slots)
                GameObject secondItemGameObject = newSlot.parent.gameObject;
                // 내 게임 오브젝트 위치
                RectTransform firstItemRectTransform = this.gameObject.GetComponent<RectTransform>();
                // 마우스 포인터의 상위 게임 오브젝트 위치
                RectTransform secondItemRectTransform = newSlot.parent.GetComponent<RectTransform>();
                // 들고 있는 아이템의 정보
                Item firstItem = rectTransform.GetComponent<ItemOnObject>().item;
                // 새로운 곳에 만들 아이템 공간
                Item secondItem = new Item();
                // 새로운 곳에 만들 아이템 공간에 이미 아이템이 존재하다면 아이템을 서로 바꿔치기한다.
                if (newSlot.parent.GetComponent<ItemOnObject>() != null)
                    secondItem = newSlot.parent.GetComponent<ItemOnObject>().item;
                Debug.Log(newSlot.name);
                if (newSlot == null)
                {
                    firstItemGameObject.transform.SetParent(oldSlot.transform);
                    firstItemRectTransform.localPosition = Vector3.zero;
                    return;
                }

                int skill_stack = 0;
                int tower_stack = 0;
                // 아이템이 슬롯에 있는 지 확인
                if (newSlot.transform.parent.parent.gameObject.tag == "EquipmentSystem")
                {
                    for (int i = 1; i < 4; i++)
                    {
                        if (GameObject.FindGameObjectWithTag("EquipmentSystem").transform.GetChild(1).GetChild(i).childCount != 0)
                        {
                            skill_stack++;
                            classType = GameObject.FindGameObjectWithTag("EquipmentSystem").transform.GetChild(1).GetChild(i).GetChild(0).GetComponent<ItemOnObject>().item.ClassType;
                        }
                    }
                    for (int i = 4; i < 8; i++)
                    {
                        if (GameObject.FindGameObjectWithTag("EquipmentSystem").transform.GetChild(1).GetChild(i).childCount != 0)
                        {
                            tower_stack++;
                        }
                    }
                }

                // 현재 아이템과 타겟 아이템의 이름이 같은가?
                bool sameItem = firstItem.itemName == secondItem.itemName;
                // 참조를 제외하고 값만을 다시 비교한다.
                bool sameItemRerferenced = firstItem.Equals(secondItem);
                bool secondItemStack = false;
                bool firstItemStack = false;
                // 이름이 같다면
                if (sameItem)
                {
                    // 아이템의 수를 비교 (1 < 99)
                    firstItemStack = firstItem.itemValue < firstItem.maxStack;
                    secondItemStack = secondItem.itemValue < secondItem.maxStack;
                }

                // 마우스 포인터의 상위 게임 오브젝트 위치에서 상위 오브젝트를 저장
                // 일반적으로 Viewport, Equipment - panel
                GameObject Inventory = secondItemRectTransform.parent.gameObject;
                if (Inventory.tag == "Slot" || newSlot.tag == oldSlot.tag)
                    Inventory = secondItemRectTransform.parent.parent.parent.gameObject; // Panel - Card

                if (Inventory.tag.Equals("Slot") || oldSlot.tag.Equals(newSlot.tag))
                    Inventory = Inventory.transform.parent.parent.gameObject; // Panel - Card

                //dragging in an Inventory      
                if (Inventory.GetComponent<EquipmentSystem>() == null && Inventory.GetComponent<CraftSystem>() == null)
                {
                    //you cannot attach items to the resultslot of the craftsystem
                    if (newSlot.transform.parent.tag == "ResultSlot" || newSlot.transform.tag == "ResultSlot" || newSlot.transform.parent.parent.tag == "ResultSlot")
                    {
                        firstItemGameObject.transform.SetParent(oldSlot.transform);
                        firstItemRectTransform.localPosition = Vector3.zero;
                    }
                    else
                    {
                        int newSlotChildCount = newSlot.transform.parent.childCount;
                        bool isOnSlot = newSlot.transform.parent.GetChild(0).tag == "ItemIcon";
                        if (newSlotChildCount != 0 && isOnSlot)
                        {
                            bool fitsIntoStack = false;
                            if (sameItem)
                                fitsIntoStack = (firstItem.itemValue + secondItem.itemValue) <= firstItem.maxStack;

                            if (inventory.stackable && sameItem && firstItemStack && secondItemStack)
                            {
                                // 카드 한장을 장착하고 같은 카드를 장착인벤에 있는 같은 카드로 합칠때 장착 슬롯이고 같은 카드가 있으면 되돌아 온다.
                                if (firstItem.itemID == secondItem.itemID && secondItemGameObject.transform.parent.parent.parent.gameObject.tag == "EquipmentSystem")// || secondItemGameObject.transform.parent.parent.gameObject.name == "Tower Slots")
                                {
                                    firstItemGameObject.transform.SetParent(oldSlot.transform);
                                    firstItemRectTransform.localPosition = Vector3.zero;
                                    return;
                                }
                                

                                if (fitsIntoStack && !sameItemRerferenced)
                                {
                                    secondItem.itemValue = firstItem.itemValue + secondItem.itemValue;
                                    secondItemGameObject.transform.SetParent(newSlot.parent.parent);
                                    Destroy(firstItemGameObject);
                                    secondItemRectTransform.localPosition = Vector3.zero;
                                    if (secondItemGameObject.GetComponent<ConsumeItem>().duplication != null)
                                    {
                                        GameObject dup = secondItemGameObject.GetComponent<ConsumeItem>().duplication;
                                        dup.GetComponent<ItemOnObject>().item.itemValue = secondItem.itemValue;
                                        dup.transform.parent.parent.parent.parent.parent.GetComponent<Inventory>().updateItemList();
                                    }
                                }
                                else
                                {
                                    int rest = (firstItem.itemValue + secondItem.itemValue) % firstItem.maxStack;

                                    if (!fitsIntoStack && rest > 0)
                                    {
                                        firstItem.itemValue = firstItem.maxStack;
                                        secondItem.itemValue = rest;

                                        firstItemGameObject.transform.SetParent(secondItemGameObject.transform.parent);
                                        secondItemGameObject.transform.SetParent(oldSlot.transform);

                                        firstItemRectTransform.localPosition = Vector3.zero;
                                        secondItemRectTransform.localPosition = Vector3.zero;
                                    }
                                }

                            }
                            //if does not fit
                            else
                            {
                                //creates the rest of the item
                                int rest = 0;
                                if (sameItem)
                                    rest = (firstItem.itemValue + secondItem.itemValue) % firstItem.maxStack;

                                //fill up the other stack and adds the rest to the other stack 
                                if (!fitsIntoStack && rest > 0)
                                {
                                    secondItem.itemValue = firstItem.maxStack;
                                    firstItem.itemValue = rest;

                                    firstItemGameObject.transform.SetParent(secondItemGameObject.transform.parent);
                                    secondItemGameObject.transform.SetParent(oldSlot.transform);

                                    firstItemRectTransform.localPosition = Vector3.zero;
                                    secondItemRectTransform.localPosition = Vector3.zero;
                                }
                                //if they are different items or the stack is full, they get swapped
                                else if (!fitsIntoStack && rest == 0)
                                {
                                    // 여기도 안들어옴
                                    Debug.Log(firstItem.itemType);
                                    Debug.Log(secondItem.itemType);
                                    if (oldSlot.transform.parent.parent.GetComponent<EquipmentSystem>() != null && firstItem.itemType == secondItem.itemType)
                                    {
                                        //Debug.Log($"7 : {newSlot.transform.parent.parent.parent.parent.parent.parent.parent.parent.parent.parent.gameObject.name}");
                                        if (oldSlot.tag == "WarriorSlot")
                                        {
                                            firstItemGameObject.transform.SetParent(oldSlot.transform);
                                            firstItemRectTransform.localPosition = Vector3.zero;
                                            return;
                                        }
                                        if (oldSlot.tag == "WizardSlot")
                                        {
                                            firstItemGameObject.transform.SetParent(oldSlot.transform);
                                            firstItemRectTransform.localPosition = Vector3.zero;
                                            return;
                                        }
                                        if (oldSlot.tag == "AssassinSlot")
                                        {
                                            firstItemGameObject.transform.SetParent(oldSlot.transform);
                                            firstItemRectTransform.localPosition = Vector3.zero;
                                            return;
                                        }
                                        if (oldSlot.tag == "InherenceSlot")
                                        {
                                            firstItemGameObject.transform.SetParent(oldSlot.transform);
                                            firstItemRectTransform.localPosition = Vector3.zero;
                                            return;
                                        }
                                        if (oldSlot.tag == "Slot")
                                        {
                                            firstItemGameObject.transform.SetParent(oldSlot.transform);
                                            firstItemRectTransform.localPosition = Vector3.zero;
                                            return;
                                        }

                                        if (oldSlot.tag != newSlot.tag)
                                        {
                                            firstItemGameObject.transform.SetParent(oldSlot.transform);
                                            firstItemRectTransform.localPosition = Vector3.zero;
                                            return;
                                        }

                                        newSlot.transform.parent.parent.parent.parent.parent.parent.parent.parent.parent.parent.GetComponent<Inventory>().UnEquipItem1(firstItem);
                                        oldSlot.transform.parent.parent.GetComponent<Inventory>().EquiptItem(secondItem);

                                        firstItemGameObject.transform.SetParent(secondItemGameObject.transform.parent);
                                        secondItemGameObject.transform.SetParent(oldSlot.transform);
                                        secondItemRectTransform.localPosition = Vector3.zero;
                                        firstItemRectTransform.localPosition = Vector3.zero;

                                        if (secondItemGameObject.GetComponent<ConsumeItem>().duplication != null)
                                            Destroy(secondItemGameObject.GetComponent<ConsumeItem>().duplication);

                                    }
                                    //if you are dragging an item from the equipmentsystem to the inventory and they are not from the same itemtype they do not get swapped.                                    
                                    else if (oldSlot.transform.parent.parent.GetComponent<EquipmentSystem>() != null && firstItem.itemType != secondItem.itemType)
                                    {
                                        firstItemGameObject.transform.SetParent(oldSlot.transform);
                                        firstItemRectTransform.localPosition = Vector3.zero;
                                    }
                                    //swapping for the rest of the inventorys
                                    else if (oldSlot.transform.parent.parent.GetComponent<EquipmentSystem>() == null)
                                    {
                                        if (firstItem.ClassType != secondItem.ClassType)
                                        {
                                            firstItemGameObject.transform.SetParent(oldSlot.transform);
                                            firstItemRectTransform.localPosition = Vector3.zero;
                                            return;
                                        }
                                        firstItemGameObject.transform.SetParent(secondItemGameObject.transform.parent);
                                        secondItemGameObject.transform.SetParent(oldSlot.transform);
                                        secondItemRectTransform.localPosition = Vector3.zero;
                                        firstItemRectTransform.localPosition = Vector3.zero;
                                    }
                                }

                            }

                        }
                        else
                        {
                            if (newSlot.tag != "Slot" && newSlot.tag != "ItemIcon" && oldSlot.tag != newSlot.tag || newSlot.tag != firstItem.ClassType + "Slot") 
                            {
                                firstItemGameObject.transform.SetParent(oldSlot.transform);
                                firstItemRectTransform.localPosition = Vector3.zero;
                            }
                            else
                            {                                
                                firstItemGameObject.transform.SetParent(newSlot.transform);
                                firstItemRectTransform.localPosition = Vector3.zero;

                                if (newSlot.transform.parent.parent.GetComponent<EquipmentSystem>() == null && oldSlot.transform.parent.parent.GetComponent<EquipmentSystem>() != null)
                                    oldSlot.transform.parent.parent.GetComponent<Inventory>().UnEquipItem1(firstItem);
                            }
                        }
                    }
                }
                Debug.Log(newSlot.parent.gameObject.name);
                Debug.Log(newSlot.tag);
                if (newSlot.parent.gameObject.name == "Slots - EquipmentSystem" && newSlot.tag == "WarriorSlot" 
                    || newSlot.parent.gameObject.name == "Slots - EquipmentSystem" && newSlot.tag == "WizardSlot" 
                    || newSlot.parent.gameObject.name == "Slots - EquipmentSystem" && newSlot.tag == "AssassinSlot"
                    || newSlot.parent.gameObject.name == "Slots - EquipmentSystem" && newSlot.tag == "InherenceSlot" 
                    || newSlot.parent.gameObject.name == "Slots - EquipmentSystem" && newSlot.tag == "Slot")
                {
                    int count = 0;
                    bool isItemID = false;
                    // 갯수가 한 개 인데 장착슬롯에 같은 아이템이 있는 경우 리턴
                    if (dragItem.item.itemValue == 1)
                    {
                        // 스킬 카드
                        if (dragItem.item.itemType == ItemType.Skill)
                        {
                            if (skill_stack != 0)
                            {
                                for (int i = 1; i < 4; i++)
                                {
                                    if (GameObject.FindGameObjectWithTag("EquipmentSystem").transform.GetChild(1).GetChild(i).childCount != 0)
                                    {
                                        // 해당칸은 제외한다. 조건 넣기
                                        if (dragItem.item.itemID == GameObject.FindGameObjectWithTag("EquipmentSystem").transform.GetChild(1).GetChild(i).GetChild(0).GetComponent<ItemOnObject>().item.itemID)
                                        {
                                            count++;

                                            if (count == 1)
                                                continue;
                                            if (count == 2)
                                                isItemID = true;
                                            count = 0;
                                        }
                                    }
                                }
                            }
                        }
                        
                        // 타워 카드
                        if (dragItem.item.itemType == ItemType.Tower)
                        {
                            if (tower_stack != 0)
                            {
                                for (int i = 4; i < 8; i++)
                                {
                                    if (GameObject.FindGameObjectWithTag("EquipmentSystem").transform.GetChild(1).GetChild(i).childCount != 0)
                                    {
                                        // 해당칸은 제외한다. 조건 넣기
                                        if (dragItem.item.itemID == GameObject.FindGameObjectWithTag("EquipmentSystem").transform.GetChild(1).GetChild(i).GetChild(0).GetComponent<ItemOnObject>().item.itemID)
                                        {
                                            count++;

                                            if (count == 1)
                                                continue;
                                            if (count == 2)
                                                isItemID = true;
                                            count = 0;
                                        }
                                        // 미니언 타워 카드인 경우
                                        if (firstItem.objType == "Minion_T")
                                        {
                                            // 사거리를 확인해서 근거리, 원거리카드가 장착되어 있는지 판단한다.
                                            if (CSVtest.Instance.MinionDic[GameObject.FindGameObjectWithTag("EquipmentSystem").transform.GetChild(1).GetChild(i).GetChild(0).GetComponent<ItemOnObject>().item.towerData.MinionID].Range
                                                == CSVtest.Instance.MinionDic[firstItem.towerData.MinionID].Range)
                                            {
                                                firstItemGameObject.transform.SetParent(oldSlot.transform);
                                                firstItemRectTransform.localPosition = Vector3.zero;
                                                return;
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                        
                    if (isItemID)
                    {
                        firstItemGameObject.transform.SetParent(oldSlot.transform);
                        firstItemRectTransform.localPosition = Vector3.zero;
                        return;
                    }

                    // 갯수가 1보다 크면
                    if (dragItem.item.itemValue > 1)
                    {
                        if (newSlot.tag != "Slot" && newSlot.tag != "ItemIcon" && oldSlot.tag != newSlot.tag || newSlot.tag != firstItem.ClassType + "Slot")
                        {
                            firstItemGameObject.transform.SetParent(oldSlot.transform);
                            firstItemRectTransform.localPosition = Vector3.zero;
                            return;
                        }
                        // 같은 ID가 있으면 되돌아 온다.
                        if (dragItem.item.itemType == ItemType.Skill)
                        {
                            if (skill_stack != 0)
                            {
                                for (int i = 1; i < 4; i++)
                                {
                                    if (GameObject.FindGameObjectWithTag("EquipmentSystem").transform.GetChild(1).GetChild(i).childCount != 0)
                                    {
                                        // 해당칸은 제외한다. 조건 넣기
                                        if (dragItem.item.itemID == GameObject.FindGameObjectWithTag("EquipmentSystem").transform.GetChild(1).GetChild(i).GetChild(0).GetComponent<ItemOnObject>().item.itemID)
                                        {
                                            count++;

                                            if (count == 1)
                                                continue;
                                            if (count == 2)
                                                isItemID = true;
                                            count = 0;
                                        }
                                    }
                                }
                            }
                        }

                        if (dragItem.item.itemType == ItemType.Tower)
                        {
                            if (tower_stack != 0)
                            {
                                for (int i = 4; i < 8; i++)
                                {
                                    if (GameObject.FindGameObjectWithTag("EquipmentSystem").transform.GetChild(1).GetChild(i).childCount != 0)
                                    {
                                        // 해당칸은 제외한다. 조건 넣기
                                        if (dragItem.item.itemID == GameObject.FindGameObjectWithTag("EquipmentSystem").transform.GetChild(1).GetChild(i).GetChild(0).GetComponent<ItemOnObject>().item.itemID)
                                        {
                                            count++;

                                            if (count == 1)
                                                continue;
                                            if (count == 2)
                                                isItemID = true;
                                            count = 0;
                                        }
                                        // 미니언 타워 카드인 경우
                                        if (firstItem.objType == "Minion_T")
                                        {
                                            // 사거리를 확인해서 근거리, 원거리카드가 장착되어 있는지 판단한다.
                                            if (CSVtest.Instance.MinionDic[GameObject.FindGameObjectWithTag("EquipmentSystem").transform.GetChild(1).GetChild(i).GetChild(0).GetComponent<ItemOnObject>().item.towerData.MinionID].Range
                                                == CSVtest.Instance.MinionDic[firstItem.towerData.MinionID].Range)
                                            {
                                                firstItemGameObject.transform.SetParent(oldSlot.transform);
                                                firstItemRectTransform.localPosition = Vector3.zero;
                                                return;
                                            }
                                        }
                                    }
                                }
                            }
                        }

                        if (isItemID)
                        {
                            firstItemGameObject.transform.SetParent(oldSlot.transform);
                            firstItemRectTransform.localPosition = Vector3.zero;
                            return;
                        }

                        if (classType != dragItem.item.ClassType && classType != null)
                        {
                            firstItemGameObject.transform.SetParent(oldSlot.transform);
                            firstItemRectTransform.localPosition = Vector3.zero;
                            return;
                        }

                        int itemValueCount;
                        itemValueCount = dragItem.item.itemValue;
                        // 수량을 1로 변화 시키고 장착 슬롯으로 복사
                        GameObject item = (GameObject)Instantiate(inventory.prefabItem);
                        ItemOnObject itemOnObject = item.GetComponent<ItemOnObject>();
                        dragItem.item.itemValue = 1;
                        itemOnObject.item = dragItem.item;
                        Instantiate(item, newSlot);
                        item.GetComponent<RectTransform>().localPosition = Vector3.zero;
                        item.GetComponent<RectTransform>().localScale = new Vector3(0.7f, 0.8f, 0f);
                        eS.gameObject.GetComponent<Inventory>().updateItemList();


                        // 남은 아이템은 -1를 시켜서 인벤토리로 이동
                        dragItem.item.itemValue = itemValueCount - 1;
                        firstItemGameObject.transform.SetParent(oldSlot.transform);
                        firstItemRectTransform.localPosition = Vector3.zero;
                    }
                }

                //dragging into a equipmentsystem/charactersystem
                if (Inventory.GetComponent<EquipmentSystem>() != null)
                {
                    ItemType[] itemTypeOfSlots = GameObject.FindGameObjectWithTag("EquipmentSystem").GetComponent<EquipmentSystem>().itemTypeOfSlots;
                    int newSlotChildCount = newSlot.transform.parent.childCount;
                    bool isOnSlot = newSlot.transform.parent.GetChild(0).tag == "ItemIcon";
                    bool sameItemType = firstItem.itemType == secondItem.itemType;

                    //dragging on a slot where allready is an item on
                    if (newSlotChildCount != 0 && isOnSlot)
                    {
                        //items getting swapped if they are the same itemtype
                        if (sameItemType && !sameItemRerferenced) //
                        {
                            Transform temp1 = secondItemGameObject.transform.parent.parent.parent;
                            Transform temp2 = oldSlot.transform.parent.parent;

                            firstItemGameObject.transform.SetParent(secondItemGameObject.transform.parent);
                            secondItemGameObject.transform.SetParent(oldSlot.transform);
                            secondItemRectTransform.localPosition = Vector3.zero;
                            firstItemRectTransform.localPosition = Vector3.zero;

                            if (!temp1.Equals(temp2))
                            {
                                if (firstItem.itemType == ItemType.UFPS_Weapon)
                                {
                                    Inventory.GetComponent<Inventory>().UnEquipItem1(secondItem);
                                    Inventory.GetComponent<Inventory>().EquiptItem(firstItem);
                                }
                                else
                                {
                                    Inventory.GetComponent<Inventory>().EquiptItem(firstItem);
                                    if (secondItem.itemType != ItemType.Backpack)
                                        Inventory.GetComponent<Inventory>().UnEquipItem1(secondItem);
                                }
                            }
                        }
                        //if they are not from the same Itemtype the dragged one getting placed back
                        else
                        {
                            firstItemGameObject.transform.SetParent(oldSlot.transform);
                            firstItemRectTransform.localPosition = Vector3.zero;
                        }

                    }
                    //if the slot is empty
                    else
                    {
                        for (int i = 0; i < newSlot.parent.childCount; i++)
                        {
                            if (newSlot.Equals(newSlot.parent.GetChild(i)))
                            {
                                //checking if it is the right slot for the item
                                if (itemTypeOfSlots[i] == transform.GetComponent<ItemOnObject>().item.itemType)
                                {
                                    transform.SetParent(newSlot);
                                    rectTransform.localPosition = Vector3.zero;

                                    if (!oldSlot.transform.parent.parent.Equals(newSlot.transform.parent.parent))
                                        Inventory.GetComponent<Inventory>().EquiptItem(firstItem);

                                }
                                //else it get back to the old slot
                                else
                                {
                                    transform.SetParent(oldSlot.transform);
                                    rectTransform.localPosition = Vector3.zero;
                                }
                            }
                        }
                    }

                }

                /*if (Inventory.GetComponent<CraftSystem>() != null)
                {
                    CraftSystem cS = Inventory.GetComponent<CraftSystem>();
                    int newSlotChildCount = newSlot.transform.parent.childCount;


                    bool isOnSlot = newSlot.transform.parent.GetChild(0).tag == "ItemIcon";
                    //dragging on a slot where allready is an item on
                    if (newSlotChildCount != 0 && isOnSlot)
                    {
                        //check if the items fits into the other item
                        bool fitsIntoStack = false;
                        if (sameItem)
                            fitsIntoStack = (firstItem.itemValue + secondItem.itemValue) <= firstItem.maxStack;
                        //if the item is stackable checking if the firstitemstack and seconditemstack is not full and check if they are the same items

                        if (inventory.stackable && sameItem && firstItemStack && secondItemStack)
                        {
                            //if the item does not fit into the other item
                            if (fitsIntoStack && !sameItemRerferenced)
                            {
                                secondItem.itemValue = firstItem.itemValue + secondItem.itemValue;
                                secondItemGameObject.transform.SetParent(newSlot.parent.parent);
                                Destroy(firstItemGameObject);
                                secondItemRectTransform.localPosition = Vector3.zero;


                                if (secondItemGameObject.GetComponent<ConsumeItem>().duplication != null)
                                {
                                    GameObject dup = secondItemGameObject.GetComponent<ConsumeItem>().duplication;
                                    dup.GetComponent<ItemOnObject>().item.itemValue = secondItem.itemValue;
                                    dup.transform.parent.parent.parent.GetComponent<Inventory>().updateItemList();
                                }
                                cS.ListWithItem();
                            }

                            else
                            {
                                //creates the rest of the item
                                int rest = (firstItem.itemValue + secondItem.itemValue) % firstItem.maxStack;

                                //fill up the other stack and adds the rest to the other stack 
                                if (!fitsIntoStack && rest > 0)
                                {
                                    firstItem.itemValue = firstItem.maxStack;
                                    secondItem.itemValue = rest;

                                    firstItemGameObject.transform.SetParent(secondItemGameObject.transform.parent);
                                    secondItemGameObject.transform.SetParent(oldSlot.transform);

                                    firstItemRectTransform.localPosition = Vector3.zero;
                                    secondItemRectTransform.localPosition = Vector3.zero;
                                    cS.ListWithItem();


                                }
                            }

                        }
                        //if does not fit
                        else
                        {
                            //creates the rest of the item
                            int rest = 0;
                            if (sameItem)
                                rest = (firstItem.itemValue + secondItem.itemValue) % firstItem.maxStack;

                            //fill up the other stack and adds the rest to the other stack 
                            if (!fitsIntoStack && rest > 0)
                            {
                                secondItem.itemValue = firstItem.maxStack;
                                firstItem.itemValue = rest;

                                firstItemGameObject.transform.SetParent(secondItemGameObject.transform.parent);
                                secondItemGameObject.transform.SetParent(oldSlot.transform);

                                firstItemRectTransform.localPosition = Vector3.zero;
                                secondItemRectTransform.localPosition = Vector3.zero;
                                cS.ListWithItem();

                            }
                            //if they are different items or the stack is full, they get swapped
                            else if (!fitsIntoStack && rest == 0)
                            {
                                //if you are dragging an item from equipmentsystem to the inventory and try to swap it with the same itemtype
                                if (oldSlot.transform.parent.parent.GetComponent<EquipmentSystem>() != null && firstItem.itemType == secondItem.itemType)
                                {

                                    firstItemGameObject.transform.SetParent(secondItemGameObject.transform.parent);
                                    secondItemGameObject.transform.SetParent(oldSlot.transform);
                                    secondItemRectTransform.localPosition = Vector3.zero;
                                    firstItemRectTransform.localPosition = Vector3.zero;

                                    oldSlot.transform.parent.parent.GetComponent<Inventory>().EquiptItem(secondItem);
                                    newSlot.transform.parent.parent.parent.parent.GetComponent<Inventory>().UnEquipItem1(firstItem);
                                }
                                //if you are dragging an item from the equipmentsystem to the inventory and they are not from the same itemtype they do not get swapped.                                    
                                else if (oldSlot.transform.parent.parent.GetComponent<EquipmentSystem>() != null && firstItem.itemType != secondItem.itemType)
                                {
                                    firstItemGameObject.transform.SetParent(oldSlot.transform);
                                    firstItemRectTransform.localPosition = Vector3.zero;
                                }
                                //swapping for the rest of the inventorys
                                else if (oldSlot.transform.parent.parent.GetComponent<EquipmentSystem>() == null)
                                {
                                    firstItemGameObject.transform.SetParent(secondItemGameObject.transform.parent);
                                    secondItemGameObject.transform.SetParent(oldSlot.transform);
                                    secondItemRectTransform.localPosition = Vector3.zero;
                                    firstItemRectTransform.localPosition = Vector3.zero;
                                }
                            }

                        }
                    }
                    else
                    {
                        if (newSlot.tag != "Slot" && newSlot.tag != "ItemIcon")
                        {
                            firstItemGameObject.transform.SetParent(oldSlot.transform);
                            firstItemRectTransform.localPosition = Vector3.zero;
                        }
                        else
                        {
                            firstItemGameObject.transform.SetParent(newSlot.transform);
                            firstItemRectTransform.localPosition = Vector3.zero;

                            if (newSlot.transform.parent.parent.GetComponent<EquipmentSystem>() == null && oldSlot.transform.parent.parent.GetComponent<EquipmentSystem>() != null)
                                oldSlot.transform.parent.parent.GetComponent<Inventory>().UnEquipItem1(firstItem);
                        }
                    }

                }*/
            }
        }
        inventory.OnUpdateItemList();
        // 카드 현황 업데이트
        EquimentInventory.instance.cardMonitorUpdate();
    }

}