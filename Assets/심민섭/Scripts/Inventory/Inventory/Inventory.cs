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
        // ???? ???? ?? ???????? ????
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
    /// ?????????? ?????? ???????? ???? ???????? ???????? ???????? ???? ???????? ?????? ?????? ???????????? ????
    /// ?????????? ?? ???????? true
    /// </summary>
    /// <param name="itemID"></param>
    /// <param name="itemValue"></param>
    /// <returns></returns>
    public bool checkIfItemAllreadyExist(int itemID, int itemValue)
    {
        // ?????? ?????????? ????
        updateItemList();
        int stack;
        for (int i = 0; i < ItemsInInventory.Count; i++) // ?????? ???? ??????List?? ???? ???? ????
        {
            if (ItemsInInventory[i].itemID == itemID) // ?????? ???? ???????? ID?? ?????? ID?? ??????
            {   
                // ?????? ???? ???????? ?????? ?????? ???????? ?????? ?????? ????
                stack = ItemsInInventory[i].itemValue + itemValue; 
                // stack?? ?????? ???? ?? ???? ???? ???????? ?????? ??????
                if (stack <= ItemsInInventory[i].maxStack)
                {
                    // stack?? ????????.
                    ItemsInInventory[i].itemValue = stack;
                    // temp ???????? Item ?????????? ???????? ?????? ?????? ???????? ?????? Item ?????????? ??????????.
                    GameObject temp = getItemGameObject(ItemsInInventory[i]);
                    // temp?? null?? ?????? duplication?? null ??????
                    if (temp != null && temp.GetComponent<ConsumeItem>().duplication != null)
                    {
                        // ?????? ???????? ???? stack?? ?????? ????????.
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
        // SlotContainer.transform.childCount?? Slots?? Grid Layout Group ?????????? ???????? ?? ???? ???? ?????? ???????? ????
        for (int k = 0; k < SlotContainer.transform.childCount; k++)
        {
            // ???? ???? ???????? ??????
            if (SlotContainer.transform.GetChild(k).childCount != 0)
            {
                // Item ?????????? itemGameObject?? ????
                GameObject itemGameObject = SlotContainer.transform.GetChild(k).GetChild(0).gameObject;
                // ItemOnObject?? ???????? ???? item?? itemObject?? ????
                Item itemObject = itemGameObject.GetComponent<ItemOnObject>().item;
                // ?????? ?????? ???? ?????? ?????? ??????????. itemObject == or != item
                if (itemObject.Equals(item))
                {
                    // ?????? Item ?????????? ????????.
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
    /// ?????? ?????????? ????????????
    /// </summary>
    public void updateItemList()
    {
        ItemsInInventory.Clear();
        for (int i = 0; i < SlotContainer.transform.childCount; i++)
        {
            // ?????? Trans?? trans?? ????
            Transform trans = SlotContainer.transform.GetChild(i);
            // ???????? ???????? ????????
            if (trans.childCount != 0)
            {
                // ?????????? ItemOnObject.item?? ????????.
                ItemsInInventory.Add(trans.GetChild(0).GetComponent<ItemOnObject>().item);
            }
        }
    }

    /// <summary>
    /// ?????? Item ???????? ????
    /// </summary>
    /// <param name="id"></param>
    /// <param name="value"></param>
    /// <returns></returns>
    public GameObject addItemToInventory(int id, int value)
    {
        // ???? ???? ???? ????
        for (int i = 0; i < SlotContainer.transform.childCount; i++)
        {
            // ?????? ???????? ??????, 0????
            if (SlotContainer.transform.GetChild(i).childCount == 0)
            {
                // ???????? ???????? item ?????? ??????.
                GameObject item = (GameObject)Instantiate(prefabItem);
                // ItemOnObject ?????????? ????????
                ItemOnObject itemOnObject = item.GetComponent<ItemOnObject>();
                // ?????? DB?? ?????? id?? ???? ???????? ?????? ???? ???????? ?????????? ???????? itemOnObject.item?? ???? ????.
                itemOnObject.item = itemDatabase.getItemByID(id);
                // ?????? ?????? ?????? ?????? ???? ?????? ????, ?????? ???????? ??????
                if (itemOnObject.item.itemValue <= itemOnObject.item.maxStack && value <= itemOnObject.item.maxStack)
                {
                    itemOnObject.item.itemValue = value;
                }
                else
                {
                    // maxStack???? ???? ?????? ?????? 1?? ????????
                    itemOnObject.item.itemValue = 1;
                }
                // ?????? Item ?????????? ?????????? ??
                item.transform.SetParent(SlotContainer.transform.GetChild(i));
                item.GetComponent<RectTransform>().localPosition = Vector3.zero;
                item.GetComponent<RectTransform>().localScale = new Vector3(0.55f, 0.7f, 0f);
                // Item ???????? ???? 0???? ?????????? Image?? ??????????????
                item.transform.GetChild(0).GetComponent<Image>().sprite = itemOnObject.item.itemIcon;
                // ???????? ???? ???????? ???? ?????? ???? ?? ???? ?????? -1??????.
                itemOnObject.item.indexItemInList = ItemsInInventory.Count - 1;
                // InputManager?? ????????.
                if (inputManagerDatabase == null)
                    inputManagerDatabase = (InputManager)Resources.Load("InputManager");
                return item;
            }
        }

        stackableSettings();

        // ?????????? ???????? ?????? ?????? ???? ????????
        updateItemList();
        return null;

    }

    /// <summary>
    /// ???? ?????? ????
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
    /// ?????? ????
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
    /// ???????? MaxStack?? 1???????? ???????? ???????? ???????? ???????? ??????.
    /// </summary>
    public void stackableSettings()
    {
        // ???? ???????? ????
        for (int i = 0; i < SlotContainer.transform.childCount; i++)
        {
            // Item ?????????? ???? ?????????? 0?? ????????
            if (SlotContainer.transform.GetChild(i).childCount > 0)
            {
                // ???? Item?? ItemOnObject ?????????? ????????
                ItemOnObject item = SlotContainer.transform.GetChild(i).GetChild(0).GetComponent<ItemOnObject>();

                // ???? ???????? MaxStack?? 1 ????????
                if (item.item.maxStack > 1)
                {
                    // ?????? ???????? Tect, ?????? ????????.
                    RectTransform textRectTransform = SlotContainer.transform.GetChild(i).GetChild(0).GetChild(1).GetComponent<RectTransform>();
                    // ?????? ?????????? ????????.
                    Text text = SlotContainer.transform.GetChild(i).GetChild(0).GetChild(1).GetComponent<Text>();
                    // Value?? ????
                    text.text = "" + item.item.itemValue;
                    // ?????? ?????????? true?? ????????.
                    text.enabled = stackable;
                    // ???????? ?????? ??????????.
                    textRectTransform.localPosition = new Vector3(positionNumberX, positionNumberY, 0);
                }
                else // ???? ???????? MaxStack?? 1??????
                {
                    // ?????? ?????????? ???????? ??????.
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
    /// ???? ???????? ?????????? ???????? Rect, GridLayoutGroup?? ?? ?????? ????
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
