using UnityEngine;
using System.Collections;
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine.UI;
using System.Collections.Generic;

public class Inventory : MonoBehaviour
{

    //event delegates for consuming, gearing
    public delegate void ItemDelegate(Item item);
    public static event ItemDelegate ItemConsumed;
    public static event ItemDelegate ItemEquip;
    public static event ItemDelegate UnEquipItem;

    public delegate void InventoryOpened();
    public static event InventoryOpened InventoryOpen;
    public static event InventoryOpened AllInventoriesClosed;

    //Prefabs
    [SerializeField]
    private GameObject prefabCanvasWithPanel;
    [SerializeField]
    private GameObject prefabSlot;
    [SerializeField]
    private GameObject prefabSlotContainer;
    [SerializeField]
    public GameObject prefabItem;
    [SerializeField]
    private GameObject prefabDraggingItemContainer;
    [SerializeField]
    private GameObject prefabPanel;

    //GUI Settings
    [SerializeField]
    public int slotSize;
    [SerializeField]
    public int iconSize;
    [SerializeField]
    public int paddingBetweenX;
    [SerializeField]
    public int paddingBetweenY;
    [SerializeField]
    public int paddingLeft;
    [SerializeField]
    public int paddingRight;
    [SerializeField]
    public int paddingBottom;
    [SerializeField]
    public int paddingTop;
    [SerializeField]
    public int positionNumberX;
    [SerializeField]
    public int positionNumberY;

    InputManager inputManagerDatabase;

    //Itemdatabase
    [SerializeField]
    private ItemDataBaseList itemDatabase;

    //GameObjects which are alive
    [SerializeField]
    private string inventoryTitle;
    [SerializeField]
    private RectTransform PanelRectTransform;
    [SerializeField]
    private Image PanelImage;
    [SerializeField]
    private GameObject SlotContainer;
    [SerializeField]
    private GameObject DraggingItemContainer;
    [SerializeField]
    private RectTransform SlotContainerRectTransform;
    [SerializeField]
    private GridLayoutGroup SlotGridLayout;
    [SerializeField]
    private RectTransform SlotGridRectTransform;

    //Inventory Settings
    [SerializeField]
    public bool mainInventory;
    [SerializeField]
    public List<Item> ItemsInInventory = new List<Item>();
    [SerializeField]
    public int height;
    [SerializeField]
    public int width;
    [SerializeField]
    public bool stackable;
    [SerializeField]
    public static bool inventoryOpen;

    void Start()
    {
        // 게임 시작 시 인벤토리 닫기
        this.gameObject.SetActive(false);

        updateItemList();

        inputManagerDatabase = (InputManager)Resources.Load("InputManager");
    }

    
    void Update()
    {
        updateItemIndex();
    }

    public void updateItemIndex()
    {
        for (int i = 0; i < ItemsInInventory.Count; i++)
        {
            ItemsInInventory[i].indexItemInList = i;
        }
    }

    public void openInventory()
    {
        this.gameObject.SetActive(true);
        if (InventoryOpen != null)
            InventoryOpen();
    }

    public void closeInventory()
    {
        this.gameObject.SetActive(false);
        checkIfAllInventoryClosed();
    }

    public void checkIfAllInventoryClosed()
    {
        GameObject canvas = GameObject.FindGameObjectWithTag("Canvas");

        for (int i = 0; i < canvas.transform.childCount; i++)
        {
            GameObject child = canvas.transform.GetChild(i).gameObject;
            if (!child.activeSelf && (child.tag == "EquipmentSystem" || child.tag == "Panel" || child.tag == "MainInventory" || child.tag == "CraftSystem"))
            {
                if (AllInventoriesClosed != null && i == canvas.transform.childCount - 1)
                    AllInventoriesClosed();
            }
            else if (child.activeSelf && (child.tag == "EquipmentSystem" || child.tag == "Panel" || child.tag == "MainInventory" || child.tag == "CraftSystem"))
                break;

            else if (i == canvas.transform.childCount - 1)
            {
                if (AllInventoriesClosed != null)
                    AllInventoriesClosed();
            }
        }
    }

    public bool characterSystem()
    {
        if (GetComponent<EquipmentSystem>() != null)
            return true;
        else
            return false;
    }

    /// <summary>
    /// 인벤토리에 들어온 아이템과 같은 아이템이 있는가를 판별하고 같은 아이템이 있으면 갯수를 업데이트하는 함수
    /// 업데이트가 잘 되었다면 true
    /// </summary>
    /// <param name="itemID"></param>
    /// <param name="itemValue"></param>
    /// <returns></returns>
    public bool checkIfItemAllreadyExist(int itemID, int itemValue)
    {
        // 아이템 인벤토리에 넣기
        updateItemList();
        int stack;
        for (int i = 0; i < ItemsInInventory.Count; i++) // 가지고 있는 아이템List의 갯수 만큼 반복
        {
            if (ItemsInInventory[i].itemID == itemID) // 가지고 있는 아이템의 ID와 들어온 ID가 같으면
            {   
                // 가지고 있는 아이템의 개수와 들어온 아이템의 갯수를 더해서 저장
                stack = ItemsInInventory[i].itemValue + itemValue; 
                // stack의 갯수가 가질 수 있는 최대 갯수보다 작거나 같으면
                if (stack <= ItemsInInventory[i].maxStack)
                {
                    // stack를 대입한다.
                    ItemsInInventory[i].itemValue = stack;
                    // temp 변수에는 Item 오브젝트를 비교하여 같은지 아닌지 비교하고 같으면 Item 오브젝트가 저장됩니다.
                    GameObject temp = getItemGameObject(ItemsInInventory[i]);
                    // temp가 null이 아니고 duplication이 null 아니면
                    if (temp != null && temp.GetComponent<ConsumeItem>().duplication != null)
                    {
                        // 출력을 담당하는 곳에 stack의 값으로 바꿔준다.
                        temp.GetComponent<ConsumeItem>().duplication.GetComponent<ItemOnObject>().item.itemValue = stack;
                    }
                    return true;
                }
            }
        }
        return false;
    }

    public GameObject getItemGameObject(Item item)
    {
        // SlotContainer.transform.childCount는 Slots에 Grid Layout Group 컴포넌트를 가져오고 그 아래 있는 슬롯의 갯수만큼 반복
        for (int k = 0; k < SlotContainer.transform.childCount; k++)
        {
            // 슬롯 안에 아이템이 있으면
            if (SlotContainer.transform.GetChild(k).childCount != 0)
            {
                // Item 오브젝트를 itemGameObject에 저장
                GameObject itemGameObject = SlotContainer.transform.GetChild(k).GetChild(0).gameObject;
                // ItemOnObject의 저장되어 있는 item을 itemObject에 저장
                Item itemObject = itemGameObject.GetComponent<ItemOnObject>().item;
                // 지정된 개체가 현재 개체와 같은지 확인합니다. itemObject == or != item
                if (itemObject.Equals(item))
                {
                    // 같으면 Item 오브젝트를 반환한다.
                    return itemGameObject;
                }
            }
        }
        return null;
    }

    public void updateSlotAmount()
    {

        if (prefabSlot == null)
            prefabSlot = Resources.Load("Prefabs/Slot - Inventory") as GameObject;

        if (SlotContainer == null)
        {
            SlotContainer = (GameObject)Instantiate(prefabSlotContainer);
            SlotContainer.transform.SetParent(PanelRectTransform.transform);
            SlotContainerRectTransform = SlotContainer.GetComponent<RectTransform>();
            SlotGridRectTransform = SlotContainer.GetComponent<RectTransform>();
            SlotGridLayout = SlotContainer.GetComponent<GridLayoutGroup>();
        }

        if (SlotContainerRectTransform == null)
            SlotContainerRectTransform = SlotContainer.GetComponent<RectTransform>();
        SlotContainerRectTransform.localPosition = Vector3.zero;

        List<Item> itemsToMove = new List<Item>();
        List<GameObject> slotList = new List<GameObject>();
        foreach (Transform child in SlotContainer.transform)
        {
            if (child.tag == "Slot") { slotList.Add(child.gameObject); }
        }

        while (slotList.Count > width * height)
        {
            GameObject go = slotList[slotList.Count - 1];
            ItemOnObject itemInSlot = go.GetComponentInChildren<ItemOnObject>();

            Debug.Log($"itemInSlot.item.itemName : {itemInSlot.item.itemName}");
            Debug.Log($"slotList.Count : {slotList.Count}");
            Debug.Log($"itemsToMove.Count : {itemsToMove.Count}");

            if (itemInSlot != null)
            {
                itemsToMove.Add(itemInSlot.item);
                ItemsInInventory.Remove(itemInSlot.item);
            }
            slotList.Remove(go);
            DestroyImmediate(go);
        }

        if (slotList.Count < width * height)
        {
            for (int i = slotList.Count; i < (width * height); i++)
            {
                GameObject Slot = (GameObject)Instantiate(prefabSlot);
                Slot.name = (slotList.Count + 1).ToString();
                Slot.transform.SetParent(SlotContainer.transform);
                slotList.Add(Slot);
            }
        }

        if (itemsToMove != null && ItemsInInventory.Count < width * height)
        {
            foreach (Item i in itemsToMove)
            {
                addItemToInventory(i.itemID);
            }
        }

        setImportantVariables();
    }

    public void updateSlotSize(int slotSize)
    {
        SlotGridLayout.cellSize = new Vector2(slotSize, slotSize);

        updateItemSize();
    }

    public void updateIconSize(int iconSize)
    {
        for (int i = 0; i < SlotContainer.transform.childCount; i++)
        {
            if (SlotContainer.transform.GetChild(i).childCount > 0)
            {
                SlotContainer.transform.GetChild(i).GetChild(0).GetChild(0).GetComponent<RectTransform>().sizeDelta = new Vector2(iconSize, iconSize);
            }
        }
        updateItemSize();
    }

    /*public void setAsMain()
    {
        if (mainInventory)
            this.gameObject.tag = "Untagged";
        else if (!mainInventory)
            this.gameObject.tag = "MainInventory";
    }*/


    /// <summary>
    /// 아이템 인벤토리에 업데이트하기
    /// </summary>
    public void updateItemList()
    {
        ItemsInInventory.Clear();
        for (int i = 0; i < SlotContainer.transform.childCount; i++)
        {
            // 슬롯의 Trans를 trans에 넣고
            Transform trans = SlotContainer.transform.GetChild(i);
            // 아이템이 슬롯안에 존재하면
            if (trans.childCount != 0)
            {
                // 인벤토리에 ItemOnObject.item를 추가한다.
                ItemsInInventory.Add(trans.GetChild(0).GetComponent<ItemOnObject>().item);
            }
        }
    }

    /// <summary>
    /// 슬롯에 Item 오브젝트 생성
    /// </summary>
    /// <param name="id"></param>
    /// <param name="value"></param>
    /// <returns></returns>
    public GameObject addItemToInventory(int id, int value)
    {
        // 슬롯 갯수 만큼 반복
        for (int i = 0; i < SlotContainer.transform.childCount; i++)
        {
            // 슬롯에 아이템이 없으면, 0이면
            if (SlotContainer.transform.GetChild(i).childCount == 0)
            {
                // 프리펩을 복제해서 item 변수에 넣는다.
                GameObject item = (GameObject)Instantiate(prefabItem);
                // ItemOnObject 스크립트를 가져오고
                ItemOnObject itemOnObject = item.GetComponent<ItemOnObject>();
                // 아이템 DB에 들어온 id와 같은 아이템이 있으면 해당 아이템의 오브젝트를 복사해서 itemOnObject.item에 넣어 준다.
                itemOnObject.item = itemDatabase.getItemByID(id);
                // 들어온 갯수와 기존에 가지고 있는 갯수와 비교, 한번더 필터하는 구문임
                if (itemOnObject.item.itemValue <= itemOnObject.item.maxStack && value <= itemOnObject.item.maxStack)
                {
                    itemOnObject.item.itemValue = value;
                }
                else
                {
                    // maxStack보다 기존 갯수가 크다면 1로 업데이트
                    itemOnObject.item.itemValue = 1;
                }
                // 실제로 Item 오브젝트를 생성해주는 곳
                item.transform.SetParent(SlotContainer.transform.GetChild(i));
                item.GetComponent<RectTransform>().localPosition = Vector3.zero;
                item.GetComponent<RectTransform>().localScale = new Vector3(0.55f, 0.7f, 0f);
                // Item 오브젝트 하위 0번째 오브젝트의 Image를 업데이트해주고
                item.transform.GetChild(0).GetComponent<Image>().sprite = itemOnObject.item.itemIcon;
                // 아이템이 들어 왔으니까 최대 가지고 있을 수 있는 갯수를 -1해준다.
                itemOnObject.item.indexItemInList = ItemsInInventory.Count - 1;
                // InputManager도 가져온다.
                if (inputManagerDatabase == null)
                    inputManagerDatabase = (InputManager)Resources.Load("InputManager");
                return item;
            }
        }

        stackableSettings();

        // 인벤토리에 아이템이 같은게 존재할 경우 업데이트
        updateItemList();
        return null;

    }

    /// <summary>
    /// 장착 아이템 해제
    /// </summary>
    /// <param name="item"></param>
    public void UnEquipItem1(Item item)
    {
        if (UnEquipItem != null)
            UnEquipItem(item);
    }

    public void OnUpdateItemList()
    {
        updateItemList();
    }

    public void deleteItemFromInventory(Item item)
    {
        for (int i = 0; i < ItemsInInventory.Count; i++)
        {
            if (item.Equals(ItemsInInventory[i]))
                ItemsInInventory.RemoveAt(i);
        }
    }

    public void ConsumeItem(Item item)
    {
        if (ItemConsumed != null)
            ItemConsumed(item);
    }

    /// <summary>
    /// 아이템 장착
    /// </summary>
    /// <param name="item"></param>
    public void EquiptItem(Item item)
    {
        if (ItemEquip != null)
            ItemEquip(item);
    }

    public void addItemToInventory(int id)
    {
        for (int i = 0; i < SlotContainer.transform.childCount; i++)
        {
            if (SlotContainer.transform.GetChild(i).childCount == 0)
            {
                GameObject item = (GameObject)Instantiate(prefabItem);
                item.GetComponent<ItemOnObject>().item = itemDatabase.getItemByID(id);
                item.transform.SetParent(SlotContainer.transform.GetChild(i));
                item.GetComponent<RectTransform>().localPosition = Vector3.zero;
                item.transform.GetChild(0).GetComponent<Image>().sprite = item.GetComponent<ItemOnObject>().item.itemIcon;
                item.GetComponent<ItemOnObject>().item.indexItemInList = ItemsInInventory.Count - 1;
                break;
            }
        }
        stackableSettings();
        updateItemList();
    }

    /// <summary>
    /// 아이템의 MaxStack이 1이하인지 이상인지 판별하고 텍스트로 보여줄지 정한다.
    /// </summary>
    public void stackableSettings()
    {
        // 슬롯 갯수만큼 반복
        for (int i = 0; i < SlotContainer.transform.childCount; i++)
        {
            // Item 오브젝트의 하위 오브젝트가 0개 이상이면
            if (SlotContainer.transform.GetChild(i).childCount > 0)
            {
                // 해당 Item의 ItemOnObject 스크립트를 가져오고
                ItemOnObject item = SlotContainer.transform.GetChild(i).GetChild(0).GetComponent<ItemOnObject>();

                // 해당 아이템의 MaxStack이 1 이상이면
                if (item.item.maxStack > 1)
                {
                    // 출력할 텍스트의 Tect, 위치을 가져온다.
                    RectTransform textRectTransform = SlotContainer.transform.GetChild(i).GetChild(0).GetChild(1).GetComponent<RectTransform>();
                    // 텍스트 컴포넌트도 가져온다.
                    Text text = SlotContainer.transform.GetChild(i).GetChild(0).GetChild(1).GetComponent<Text>();
                    // Value를 적용
                    text.text = "" + item.item.itemValue;
                    // 텍스트 컴포넌트도 true로 바꿔준다.
                    text.enabled = stackable;
                    // 텍스트의 위치도 지정해준다.
                    textRectTransform.localPosition = new Vector3(positionNumberX, positionNumberY, 0);
                }
                else // 해당 아이템의 MaxStack이 1이하면
                {
                    // 텍스트 컴포넌트를 비활성화 시킨다.
                    Text text = SlotContainer.transform.GetChild(i).GetChild(0).GetChild(1).GetComponent<Text>();
                    text.enabled = false;
                }
            }
        }

    }

    public void getPrefabs()
    {
        if (prefabCanvasWithPanel == null)
            prefabCanvasWithPanel = Resources.Load("Prefabs/Canvas - Inventory") as GameObject;
        if (prefabSlot == null)
            prefabSlot = Resources.Load("Prefabs/Slot - Inventory") as GameObject;
        if (prefabSlotContainer == null)
            prefabSlotContainer = Resources.Load("Prefabs/Slots - Inventory") as GameObject;
        if (prefabItem == null)
            prefabItem = Resources.Load("Prefabs/Item") as GameObject;
        if (itemDatabase == null)
            itemDatabase = (ItemDataBaseList)Resources.Load("ItemDatabase");
        if (prefabDraggingItemContainer == null)
            prefabDraggingItemContainer = Resources.Load("Prefabs/DraggingItem") as GameObject;
        if (prefabPanel == null)
            prefabPanel = Resources.Load("Prefabs/Panel - Inventory") as GameObject;

        setImportantVariables();
        setDefaultSettings();
        //adjustInventorySize();
        updateSlotAmount(width, height);
        //updateSlotSize();
        //updatePadding(paddingBetweenX, paddingBetweenY);

    }

    /*public void updatePadding(int spacingBetweenX, int spacingBetweenY)
    {
        SlotGridLayout.spacing = new Vector2(paddingBetweenX, paddingBetweenY);
        SlotGridLayout.padding.bottom = paddingBottom;
        SlotGridLayout.padding.right = paddingRight;
        SlotGridLayout.padding.left = paddingLeft;
        SlotGridLayout.padding.top = paddingTop;
    }*/

    /*public void updateSlotSize()
    {
        SlotGridLayout.cellSize = new Vector2(slotSize, slotSize);

        updateItemSize();
    }*/
    void updateItemSize()
    {
        for (int i = 0; i < SlotContainer.transform.childCount; i++)
        {
            if (SlotContainer.transform.GetChild(i).childCount > 0)
            {
                SlotContainer.transform.GetChild(i).GetChild(0).GetComponent<RectTransform>().sizeDelta = new Vector2(slotSize, slotSize + 40);
                SlotContainer.transform.GetChild(i).GetChild(0).GetChild(2).GetComponent<RectTransform>().sizeDelta = new Vector2(slotSize, slotSize + 40);
            }
        }
    }

    public void updateSlotAmount(int width, int height)
    {
        if (prefabSlot == null)
            prefabSlot = Resources.Load("Prefabs/Slot - Inventory") as GameObject;

        if (SlotContainer == null)
        {
            SlotContainer = (GameObject)Instantiate(prefabSlotContainer);
            SlotContainer.transform.SetParent(PanelRectTransform.transform);
            SlotContainerRectTransform = SlotContainer.GetComponent<RectTransform>();
            SlotGridRectTransform = SlotContainer.GetComponent<RectTransform>();
            SlotGridLayout = SlotContainer.GetComponent<GridLayoutGroup>();
        }

        if (SlotContainerRectTransform == null)
            SlotContainerRectTransform = SlotContainer.GetComponent<RectTransform>();

        SlotContainerRectTransform.localPosition = Vector3.zero;

        List<Item> itemsToMove = new List<Item>();
        List<GameObject> slotList = new List<GameObject>();
        foreach (Transform child in SlotContainer.transform)
        {
            if (child.tag == "Slot") { slotList.Add(child.gameObject); }
        }

        while (slotList.Count > width * height)
        {
            GameObject go = slotList[slotList.Count - 1];
            ItemOnObject itemInSlot = go.GetComponentInChildren<ItemOnObject>();
            if (itemInSlot != null)
            {
                itemsToMove.Add(itemInSlot.item);
                ItemsInInventory.Remove(itemInSlot.item);
            }
            slotList.Remove(go);
            DestroyImmediate(go);
        }

        if (slotList.Count < width * height)
        {
            for (int i = slotList.Count; i < (width * height); i++)
            {
                GameObject Slot = (GameObject)Instantiate(prefabSlot);
                Slot.name = (slotList.Count + 1).ToString();
                Slot.transform.SetParent(SlotContainer.transform);
                slotList.Add(Slot);
            }
        }

        if (itemsToMove != null && ItemsInInventory.Count < width * height)
        {
            foreach (Item i in itemsToMove)
            {
                addItemToInventory(i.itemID);
            }
        }

        setImportantVariables();
    }

    public void setDefaultSettings()
    {
        height = 2;
        width = 4;

        slotSize = 50;
        iconSize = 45;

        paddingBetweenX = 25;
        paddingBetweenY = 30;
        paddingTop = 0;
        paddingBottom = 0;
        paddingRight = 20;
        paddingLeft = 0;
    }

    public void adjustInventorySize()
    {
        int x = (((width * slotSize) + ((width - 1) * paddingBetweenX)) + paddingLeft + paddingRight);
        int y = (((height * slotSize) + ((height - 1) * paddingBetweenY)) + paddingTop + paddingBottom);
        PanelRectTransform.sizeDelta = new Vector2(x, y);

        SlotGridRectTransform.sizeDelta = new Vector2(x, y);
    }

    /// <summary>
    /// 슬롯 컨테이너 오브젝트를 가져오고 Rect, GridLayoutGroup을 각 변수에 저장
    /// </summary>
    public void setImportantVariables()
    {
        PanelRectTransform = GetComponent<RectTransform>();
        SlotContainer = transform.GetChild(1).gameObject;
        SlotGridLayout = SlotContainer.GetComponent<GridLayoutGroup>();
        SlotGridRectTransform = SlotContainer.GetComponent<RectTransform>();
    }

    /*public void stackableSettings(bool stackable, Vector3 posi)
    {
        for (int i = 0; i < SlotContainer.transform.childCount; i++)
        {
            if (SlotContainer.transform.GetChild(i).childCount > 0)
            {
                ItemOnObject item = SlotContainer.transform.GetChild(i).GetChild(0).GetComponent<ItemOnObject>();
                if (item.item.maxStack > 1)
                {
                    RectTransform textRectTransform = SlotContainer.transform.GetChild(i).GetChild(0).GetChild(1).GetComponent<RectTransform>();
                    Text text = SlotContainer.transform.GetChild(i).GetChild(0).GetChild(1).GetComponent<Text>();
                    text.text = "" + item.item.itemValue;
                    text.enabled = stackable;
                    textRectTransform.localPosition = posi;
                }
            }
        }
    }*/


}
