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
    private GameObject prefabItem;
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
        // ���� ���� �� �κ��丮 �ݱ�
        if (transform.GetComponent<Hotbar>() == null)
            this.gameObject.SetActive(false);

        updateItemList();

        inputManagerDatabase = (InputManager)Resources.Load("InputManager");
    }

    
    void Update()
    {
        updateItemIndex();
        Debug.Log(SlotContainer.transform.childCount);
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
    /// �κ��丮�� ���� �����۰� ���� �������� �ִ°��� �Ǻ��ϰ� ���� �������� ������ ������ ������Ʈ�ϴ� �Լ�
    /// ������Ʈ�� �� �Ǿ��ٸ� true
    /// </summary>
    /// <param name="itemID"></param>
    /// <param name="itemValue"></param>
    /// <returns></returns>
    public bool checkIfItemAllreadyExist(int itemID, int itemValue)
    {
        // ������ �κ��丮�� �ֱ�
        updateItemList();
        int stack;
        for (int i = 0; i < ItemsInInventory.Count; i++) // ������ �ִ� ������List�� ���� ��ŭ �ݺ�
        {
            if (ItemsInInventory[i].itemID == itemID) // ������ �ִ� �������� ID�� ���� ID�� ������
            {   
                // ������ �ִ� �������� ������ ���� �������� ������ ���ؼ� ����
                stack = ItemsInInventory[i].itemValue + itemValue; 
                // stack�� ������ ���� �� �ִ� �ִ� �������� �۰ų� ������
                if (stack <= ItemsInInventory[i].maxStack)
                {
                    // stack�� �����Ѵ�.
                    ItemsInInventory[i].itemValue = stack;
                    // temp �������� Item ������Ʈ�� ���Ͽ� ������ �ƴ��� ���ϰ� ������ Item ������Ʈ�� ����˴ϴ�.
                    GameObject temp = getItemGameObject(ItemsInInventory[i]);
                    // temp�� null�� �ƴϰ� duplication�� null �ƴϸ�
                    if (temp != null && temp.GetComponent<ConsumeItem>().duplication != null)
                    {
                        // ����� ����ϴ� ���� stack�� ������ �ٲ��ش�.
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
        // SlotContainer.transform.childCount�� Slots�� Grid Layout Group ������Ʈ�� �������� �� �Ʒ� �ִ� ������ ������ŭ �ݺ�
        for (int k = 0; k < SlotContainer.transform.childCount; k++)
        {
            // ���� �ȿ� �������� ������
            if (SlotContainer.transform.GetChild(k).childCount != 0)
            {
                // Item ������Ʈ�� itemGameObject�� ����
                GameObject itemGameObject = SlotContainer.transform.GetChild(k).GetChild(0).gameObject;
                // ItemOnObject�� ����Ǿ� �ִ� item�� itemObject�� ����
                Item itemObject = itemGameObject.GetComponent<ItemOnObject>().item;
                // ������ ��ü�� ���� ��ü�� ������ Ȯ���մϴ�. itemObject == or != item
                if (itemObject.Equals(item))
                {
                    // ������ Item ������Ʈ�� ��ȯ�Ѵ�.
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

    public void setAsMain()
    {
        if (mainInventory)
            this.gameObject.tag = "Untagged";
        else if (!mainInventory)
            this.gameObject.tag = "MainInventory";
    }


    /// <summary>
    /// ������ �κ��丮�� ������Ʈ�ϱ�
    /// </summary>
    public void updateItemList()
    {
        ItemsInInventory.Clear();
        for (int i = 0; i < SlotContainer.transform.childCount; i++)
        {
            // ������ Trans�� trans�� �ְ�
            Transform trans = SlotContainer.transform.GetChild(i);
            // �������� ���Ծȿ� �����ϸ�
            if (trans.childCount != 0)
            {
                // �κ��丮�� ItemOnObject.item�� �߰��Ѵ�.
                ItemsInInventory.Add(trans.GetChild(0).GetComponent<ItemOnObject>().item);
            }
        }
    }

    /// <summary>
    /// ���Կ� Item ������Ʈ ����
    /// </summary>
    /// <param name="id"></param>
    /// <param name="value"></param>
    /// <returns></returns>
    public GameObject addItemToInventory(int id, int value)
    {
        // ���� ���� ��ŭ �ݺ�
        for (int i = 0; i < SlotContainer.transform.childCount; i++)
        {
            // ���Կ� �������� ������, 0�̸�
            if (SlotContainer.transform.GetChild(i).childCount == 0)
            {
                // �������� �����ؼ� item ������ �ִ´�.
                GameObject item = (GameObject)Instantiate(prefabItem);
                // ItemOnObject ��ũ��Ʈ�� ��������
                ItemOnObject itemOnObject = item.GetComponent<ItemOnObject>();
                // ������ DB�� ���� id�� ���� �������� ������ �ش� �������� ������Ʈ�� �����ؼ� itemOnObject.item�� �־� �ش�.
                itemOnObject.item = itemDatabase.getItemByID(id);
                // ���� ������ ������ ������ �ִ� ������ ��, �ѹ��� �����ϴ� ������
                if (itemOnObject.item.itemValue <= itemOnObject.item.maxStack && value <= itemOnObject.item.maxStack)
                {
                    itemOnObject.item.itemValue = value;
                }
                else
                {
                    // maxStack���� ���� ������ ũ�ٸ� 1�� ������Ʈ
                    itemOnObject.item.itemValue = 1;
                }
                // ������ Item ������Ʈ�� �������ִ� ��
                item.transform.SetParent(SlotContainer.transform.GetChild(i));
                item.GetComponent<RectTransform>().localPosition = Vector3.zero;
                // Item ������Ʈ ���� 0��° ������Ʈ�� Image�� ������Ʈ���ְ�
                item.transform.GetChild(0).GetComponent<Image>().sprite = itemOnObject.item.itemIcon;
                // �������� ��� �����ϱ� �ִ� ������ ���� �� �ִ� ������ -1���ش�.
                itemOnObject.item.indexItemInList = ItemsInInventory.Count - 1;
                // InputManager�� �����´�.
                if (inputManagerDatabase == null)
                    inputManagerDatabase = (InputManager)Resources.Load("InputManager");
                return item;
            }
        }

        stackableSettings();

        // �κ��丮�� �������� ������ ������ ��� ������Ʈ
        updateItemList();
        return null;

    }

    /// <summary>
    /// ���� ������ ����
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
    /// ������ ����
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
    /// �������� MaxStack�� 1�������� �̻����� �Ǻ��ϰ� �ؽ�Ʈ�� �������� ���Ѵ�.
    /// </summary>
    public void stackableSettings()
    {
        // ���� ������ŭ �ݺ�
        for (int i = 0; i < SlotContainer.transform.childCount; i++)
        {
            // Item ������Ʈ�� ���� ������Ʈ�� 0�� �̻��̸�
            if (SlotContainer.transform.GetChild(i).childCount > 0)
            {
                // �ش� Item�� ItemOnObject ��ũ��Ʈ�� ��������
                ItemOnObject item = SlotContainer.transform.GetChild(i).GetChild(0).GetComponent<ItemOnObject>();

                // �ش� �������� MaxStack�� 1 �̻��̸�
                if (item.item.maxStack > 1)
                {
                    // ����� �ؽ�Ʈ�� Tect, ��ġ�� �����´�.
                    RectTransform textRectTransform = SlotContainer.transform.GetChild(i).GetChild(0).GetChild(1).GetComponent<RectTransform>();
                    // �ؽ�Ʈ ������Ʈ�� �����´�.
                    Text text = SlotContainer.transform.GetChild(i).GetChild(0).GetChild(1).GetComponent<Text>();
                    // Value�� ����
                    text.text = "" + item.item.itemValue;
                    // �ؽ�Ʈ ������Ʈ�� true�� �ٲ��ش�.
                    text.enabled = stackable;
                    // �ؽ�Ʈ�� ��ġ�� �������ش�.
                    textRectTransform.localPosition = new Vector3(positionNumberX, positionNumberY, 0);
                }
                else // �ش� �������� MaxStack�� 1���ϸ�
                {
                    // �ؽ�Ʈ ������Ʈ�� ��Ȱ��ȭ ��Ų��.
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
        adjustInventorySize();
        updateSlotAmount(width, height);
        updateSlotSize();
        updatePadding(paddingBetweenX, paddingBetweenY);

    }

    public void updatePadding(int spacingBetweenX, int spacingBetweenY)
    {
        SlotGridLayout.spacing = new Vector2(paddingBetweenX, paddingBetweenY);
        SlotGridLayout.padding.bottom = paddingBottom;
        SlotGridLayout.padding.right = paddingRight;
        SlotGridLayout.padding.left = paddingLeft;
        SlotGridLayout.padding.top = paddingTop;
    }

    public void updateSlotSize()
    {
        SlotGridLayout.cellSize = new Vector2(slotSize, slotSize);

        updateItemSize();
    }
    void updateItemSize()
    {
        for (int i = 0; i < SlotContainer.transform.childCount; i++)
        {
            if (SlotContainer.transform.GetChild(i).childCount > 0)
            {
                SlotContainer.transform.GetChild(i).GetChild(0).GetComponent<RectTransform>().sizeDelta = new Vector2(slotSize, slotSize);
                SlotContainer.transform.GetChild(i).GetChild(0).GetChild(2).GetComponent<RectTransform>().sizeDelta = new Vector2(slotSize, slotSize);
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
        if (GetComponent<EquipmentSystem>() == null && GetComponent<Hotbar>() == null && GetComponent<CraftSystem>() == null)
        {
            height = 5;
            width = 5;

            slotSize = 50;
            iconSize = 45;

            paddingBetweenX = 5;
            paddingBetweenY = 5;
            paddingTop = 35;
            paddingBottom = 10;
            paddingLeft = 10;
            paddingRight = 10;
        }
        else if (GetComponent<Hotbar>() != null)
        {
            height = 1;
            width = 9;

            slotSize = 50;
            iconSize = 45;

            paddingBetweenX = 5;
            paddingBetweenY = 5;
            paddingTop = 10;
            paddingBottom = 10;
            paddingLeft = 10;
            paddingRight = 10;
        }
        else if (GetComponent<CraftSystem>() != null)
        {
            height = 3;
            width = 3;
            slotSize = 55;
            iconSize = 45;

            paddingBetweenX = 5;
            paddingBetweenY = 5;
            paddingTop = 35;
            paddingBottom = 95;
            paddingLeft = 25;
            paddingRight = 25;
        }
        else
        {
            height = 4;
            width = 2;

            slotSize = 50;
            iconSize = 45;

            paddingBetweenX = 100;
            paddingBetweenY = 20;
            paddingTop = 35;
            paddingBottom = 10;
            paddingLeft = 10;
            paddingRight = 10;
        }
    }

    public void adjustInventorySize()
    {
        int x = (((width * slotSize) + ((width - 1) * paddingBetweenX)) + paddingLeft + paddingRight);
        int y = (((height * slotSize) + ((height - 1) * paddingBetweenY)) + paddingTop + paddingBottom);
        PanelRectTransform.sizeDelta = new Vector2(x, y);

        SlotGridRectTransform.sizeDelta = new Vector2(x, y);
    }

    /// <summary>
    /// ���� �����̳� ������Ʈ�� �������� Rect, GridLayoutGroup�� �� ������ ����
    /// </summary>
    public void setImportantVariables()
    {
        PanelRectTransform = GetComponent<RectTransform>();
        SlotContainer = transform.GetChild(1).gameObject;
        SlotGridLayout = SlotContainer.GetComponent<GridLayoutGroup>();
        SlotGridRectTransform = SlotContainer.GetComponent<RectTransform>();
    }

    public void stackableSettings(bool stackable, Vector3 posi)
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
    }


}
