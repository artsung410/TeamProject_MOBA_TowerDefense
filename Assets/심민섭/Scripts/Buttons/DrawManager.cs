using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class DrawManager : MonoBehaviour
{
    // ###############################################
    //             NAME : Simstealer                      
    //             MAIL : minsub4400@gmail.com         
    // ###############################################
    // ��ư ���� ���� ����
    public static DrawManager instance;

    [Header("���õ� ������ ����")]
    // ������ �̹���
    public Sprite selectImage;
    // ������ ������ ��
    public string selectNameText;
    // ������ �������� ����
    public string selectExplanationText;

    [Header("������ ������ ����")]
    // ������ �������� ��ȭ��
    public string buyCurencyName;
    // ������ ī�� ����
    public int buyCount; // 0�� �ƴϸ� ������ �� - ������ ȹ�� ó�� �� 0���� �ʱ�ȭ\
    // ������ �������� �̹���
    public Image buyItemImage;
    // ������ ������ ��
    public string buyItemName;

    [SerializeField]
    private GameObject prefabItem;

    // Other �κ��丮
    private GameObject otherInventory;
    // Warrior �κ��丮
    private GameObject warriorInventory;

    // ���� ��� �ִ� �ڽ� ���� ����
    [Header("Box ����")]
    public ItemOnObject boxItem;
    public Sprite boxImage;
    public string boxName;
    public int boxCount; // ����� ����

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        buyCount = 1;
        boxCount = 1;
        buyCurencyName = "Zera";

        // Other �κ��丮
        otherInventory = GameObject.FindGameObjectWithTag("Canvas").transform.GetChild(3).GetChild(2).GetChild(0).GetChild(0).GetChild(0).GetChild(0).gameObject; // Slots
        // Warrior �κ��丮
        warriorInventory = GameObject.FindGameObjectWithTag("Canvas").transform.GetChild(3).GetChild(1).GetChild(0).GetChild(0).GetChild(0).GetChild(0).GetChild(0).GetChild(0).gameObject;

        // ������ DB�� ���� id�� ���� �������� ������ �ش� �������� ������Ʈ�� �����ؼ� itemOnObject.item�� �־� �ش�.
        //itemOnObject.item = itemDatabase.getItemByID(id);
    }

    // �̹� �ִ� ������ ������
    private ItemOnObject firstItem;
    // ���� ���� ������
    private ItemOnObject secondItem;
    // ���� ���� ������ ���� ��� ����
    private ItemOnObject drawItem;
    // ���� ���� ������ ������Ʈ ��� ����
    private GameObject drawItemObj;

    public void PutCardItem_AfterBuy()
    {
        // ���� �� ��ŭ �ݺ��ؼ� ����ִ� ���� Ȯ��
        for (int i = 0; i < otherInventory.transform.childCount; i++)
        {
            if (otherInventory.transform.GetChild(i).childCount > 0) // �������� �̹� �ִ� ���
            {
                firstItem = otherInventory.transform.GetChild(i).GetChild(0).GetComponent<ItemOnObject>();
                //Debug.Log(otherInventory.transform.GetChild(i).GetChild(0).name); // DrawBox
                /*drawItem = drawItemObj.GetComponent<DrawItem>();
                secondItem.drawBox.BoxName = selectNameText;*/

                // �̹� �ִ� ������ ��� ������ ������ �� ��
                if (firstItem.item.itemName == buyItemName)
                {
                    // Value�� �÷��ش�.
                    /*Text text = firstItem.transform.GetChild(1).GetComponent<Text>();
                    text.text = (int.Parse(text.text) + buyCount).ToString();*/

                    firstItem.item.itemValue += buyCount;

                    Debug.Log("������ ���� ����");
                    return;
                }
                else // �̸��� ���� ���� ���
                {
                    if (otherInventory.transform.GetChild(i).childCount == 0)
                    {
                        Debug.Log("������ ���� �ٸ�");
                        drawItemObj = (GameObject)Instantiate(prefabItem);
                        drawItem = drawItemObj.GetComponent<ItemOnObject>();
                        drawItem.item.itemIcon = buyItemImage.sprite; // �ڽ� �̹���
                        drawItem.item.itemName = buyItemName; // �ڽ� ��
                        drawItem.item.itemValue = buyCount;
                        drawItem.item.itemType = ItemType.Consumable;
                        //drawItem.item.ClassType = "Box";

                        drawItemObj.transform.SetParent(otherInventory.transform.GetChild(i));
                        drawItemObj.transform.GetChild(0).GetComponent<Image>().sprite =
                            drawItemObj.GetComponent<ItemOnObject>().item.itemIcon;
                        //drawItemObj.transform.GetChild(1).GetComponent<Text>().text = buyCount.ToString();
                        drawItemObj.transform.localPosition = Vector3.zero;
                        drawItemObj.transform.localScale = new Vector3(1.5f, 1.5f, 1.5f);
                    }
                }
            }
            // ������ ������� ���
            if (otherInventory.transform.GetChild(i).childCount == 0)
            {
                drawItemObj = (GameObject)Instantiate(prefabItem);
                drawItem = drawItemObj.GetComponent<ItemOnObject>();
                drawItem.item.itemIcon = buyItemImage.sprite; // �ڽ� �̹���
                drawItem.item.itemName = buyItemName; // �ڽ� ��
                drawItem.item.itemValue = buyCount;
                drawItem.item.itemType = ItemType.Consumable;
                //drawItem.item.ClassType = "Box";

                drawItemObj.transform.SetParent(otherInventory.transform.GetChild(i));
                drawItemObj.transform.GetChild(0).GetComponent<Image>().sprite =
                    drawItemObj.GetComponent<ItemOnObject>().item.itemIcon;
                //drawItemObj.transform.GetChild(1).GetComponent<Text>().text = buyCount.ToString();
                drawItemObj.transform.localPosition = Vector3.zero;
                drawItemObj.transform.localScale = new Vector3(1.5f, 1.5f, 1.5f);
                // �������� �־��ٸ� 
                return;
            }
            
        }
    }

    // ������ �Ҹ� �Լ�
    public void OpenBoxDisCount()
    {
        // ����
        if (boxItem.item.itemValue <= 0)
        {
            Destroy(boxItem.gameObject);
        }
        else
        {
            boxItem.item.itemValue -= boxCount;
            if (boxItem.item.itemValue == 0)
            {
                Destroy(boxItem.gameObject);
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
            // ������ 0���� ������ �����
            if (item.itemValue <= 0)
            {
                if (tooltip != null)
                    tooltip.deactivateTooltip();
                inventory.deleteItemFromInventory(item);
                Destroy(this.gameObject);
            }
        }*/
    }

    // ���� 10�� ���Ϸ� ���� ��ư ��Ȱ��ȭ �Լ�
    [SerializeField]
    private GameObject openCard_tenButton;
    public void ButtonDisable()
    {
        openCard_tenButton.GetComponent<Button>().interactable = false;
    }
    public void ButtonEnable()
    {
        openCard_tenButton.GetComponent<Button>().interactable = true;
    }

    // ȹ���� ������ �ε����� ���� ����
    public List<int> getDrawResult = new List<int>();

    // ȹ���� ������ ������Ʈ�� ���� ����(RandomSelect -> DrawManager)
    public List<Item> getDrawResultItem = new List<Item>();

    // �̰� ��¥ ��¥ ��ο��ؼ� ���� ī����!!
    public List<Item> ResultItem = new List<Item>();

    // List -> Struct ��ȯ
    public List<ItemStruct> StructResultItem = new List<ItemStruct>();

    // ī�� ���� �� �������� �κ��丮�� ����.
    // 1. ī��� ���� �� getItemList�� �����͸� �ִ´�.
    // 2. �κ��丮�� ���� ī�嵥���Ͱ� �ִ��� Ȯ�� �Ѵ�.
    // 3. ���� ī�嵥���Ͱ� ������ Value�� �ø���.
    // 4. ���� ī�嵥���Ͱ� ������ �� ���Կ� ������Ʈ�� �����Ѵ�.

    [SerializeField]
    private GameObject alreadyCardDraw;
    // Back ��ư�� Ŭ�� �� �̱� ī�� ������ �ʱ�ȭ�� �Բ� â�� ������.
    public void CardDataInit()
    {
        //Debug.Log(alreadyCardDraw.transform.childCount);
        for (int i = 0; i < alreadyCardDraw.transform.childCount; i++)
        {
            //Debug.Log(alreadyCardDraw.transform.GetChild(i).name);
            Destroy(alreadyCardDraw.transform.GetChild(i).gameObject);
        }
    }

    // ���� ī�� ����(�ڷᱸ������ �ߺ� �� ã��)
    // �������� ���� ��� ����
    private void DrawCardOrganize_one()
    {
        /*var duplicates = getDrawResultItem.GroupBy(x => x)
                                        .SelectMany(g => g.Skip(1))
                                        .Distinct()
                                        .ToList();
        List<Item> dupleicatesList = new List<Item>();
        dupleicatesList.AddRange(duplicates);*/

        //var duplicatesCnt = dupleicatesList.Count;

        ResultItem = getDrawResultItem.Distinct().ToList();

        // List -> Struct ��ȯ
        for (int i = 0; i < ResultItem.Count; i++)
        {
            ItemStruct itemStruct = new ItemStruct();
            itemStruct.itemName = ResultItem[i].itemName;
            itemStruct.ClassType = ResultItem[i].ClassType;
            itemStruct.itemID = ResultItem[i].itemID;
            itemStruct.itemDesc = ResultItem[i].itemDesc;
            itemStruct.itemIcon = ResultItem[i].itemIcon;
            itemStruct.itemModel = ResultItem[i].itemModel;
            itemStruct.inGameData = ResultItem[i].inGameData;
            itemStruct.itemValue = 0;
            if (getDrawResultItem.Count != 0)
            {
                for (int j = 0; j < getDrawResultItem.Count; j++)
                {
                    if (ResultItem[i].itemID == getDrawResultItem[j].itemID)
                    {
                        itemStruct.itemValue += 1;
                        getDrawResultItem.RemoveAt(j);
                        j -= 1;
                    }
                }
            }
            itemStruct.itemType = ResultItem[i].itemType;
            itemStruct.itemWeight = ResultItem[i].itemWeight;
            itemStruct.maxStack = ResultItem[i].maxStack;
            itemStruct.indexItemInList = ResultItem[i].indexItemInList;
            itemStruct.rarity = ResultItem[i].rarity;
            itemStruct.itemAttributes = ResultItem[i].itemAttributes;
            itemStruct.specialPrefabs = ResultItem[i].specialPrefabs;
            itemStruct.buffDatas = ResultItem[i].buffDatas;
            StructResultItem.Add(itemStruct);
        }
    }

    private void DrawCardOrganize_two()
    {
        // ���� �������� �ִ��� Ȯ���Ѵ�.
        // ���� �������� ������ Value�� ���Ѵ�.
        // ���� �������� ������ �� ���Կ� �������� �����Ѵ�.
        ResultItem = getDrawResultItem;

        // �κ��丮�� �ִ� �������� �����Ѵ�.
        List<Item> myInven_warrior = new List<Item>();
        for (int i = 0; i < warriorInventory.transform.childCount; i++)
        {
            // ���Կ� �������� ������ ���� ���� �߰�
            if (warriorInventory.transform.GetChild(i).childCount != 0)
            {
                myInven_warrior.Add(warriorInventory.transform.GetChild(i).GetChild(0).GetComponent<ItemOnObject>().item);
            }
        }

        List<Item> sameItem = new List<Item>();
        // ���� �������� �ִ��� �Ǻ��ϰ� value ���� ���Ѵ�.
        for (int i = 0; i <= ResultItem.Count; i++) // 10�� �ݺ� (�̱��� �������� ����ŭ)
        {
            int stack = 0;
            for (int j = 0; j < myInven_warrior.Count; j++) // �κ��丮�� �ִ� �������� ����ŭ �ݺ�
            {
                // ���� �������� �����ϸ�
                if (ResultItem[i].itemID == myInven_warrior[j].itemID)
                {
                    // value�� �ø���.
                    myInven_warrior[j].itemValue += 1;
                    ResultItem.RemoveAt(i);
                    if (ResultItem.Count == 0)
                        break;
                    if (i == 0)
                        continue;
                    i--;
                }
                else
                {
                    // �ٸ��� ������ �״´�.
                    stack++;
                    // ������ myInven�� ������
                    if (stack == myInven_warrior.Count)
                    {
                        // Add �Ѵ�.
                        sameItem.Add(ResultItem[i]);
                        ResultItem.RemoveAt(i);
                        if (i == 0)
                            continue;
                        i--;
                    }
                }
            }
            if (ResultItem.Count <= 1)
            {
                if (ResultItem.Count == 0)
                    break;
                i = -1;
            }
        }


        //int putInItems = 0;

            // ���Կ� �������� �������� ������
            /*if (warriorInventory.transform.GetChild(i).childCount == 0)
            {
                if (ResultItem.Count == 0)
                {
                    Debug.Log("ResultItem.Count : 0");
                    return;
                }
                // �������� �����ؼ� �ִ´�.
                // ������ ������Ʈ�� �����Ѵ�.
                GameObject itemObjProduce = (GameObject)Instantiate(prefabItem);
                // ItemOnObject ��ũ��Ʈ�� �����´�.
                ItemOnObject itemProduce = itemObjProduce.GetComponent<ItemOnObject>();
                itemProduce.item = ResultItem[putInItems];
                ResultItem.RemoveAt(putInItems);
                //putInItems++;
                itemObjProduce.transform.SetParent(warriorInventory.transform.GetChild(i));
                itemObjProduce.transform.localPosition = Vector3.zero;
                itemObjProduce.transform.localScale = new Vector3(0.4f, 0.4f, 0.4f);

                ResultItem.RemoveAt(i);
                if (ResultItem.Count == 0)
                {
                    return;
                }
            }*/

        


    }

    // Retry ��ư�� Ŭ�� �� �ٽ� ���� ������� �̴´�. 
    public void RandomCardDraw()
    {

    }


    // ������ ������Ʈ ���� �� �̵� �Լ�
    public void ItemProduceAndIventoryMove()
    {

        // �κ��丮�� �������� ������ ��� ����
        int count = warriorInventory.transform.childCount;
        for (int i = 0; i < warriorInventory.transform.childCount; i++)
        {
            if (warriorInventory.transform.GetChild(i).childCount == 0)
            {
                count -= 1; // �� ������ ������ �߰��ȴ�.
            }
            if (count == 0) // �������� �κ��丮�� �ϳ��� ���� ���
            {
                DrawCardOrganize_one();
                // ������ ����/�̵�
                for (int j = 0; j < StructResultItem.Count; j++) // 10
                {
                    // ������ ������Ʈ�� �����Ѵ�.
                    GameObject itemObjProduce = (GameObject)Instantiate(prefabItem);
                    // ItemOnObject ��ũ��Ʈ�� �����´�.
                    ItemOnObject itemProduce = itemObjProduce.GetComponent<ItemOnObject>();
                    // ������ ������ �����Ѵ�.
                    itemProduce.item.itemName = StructResultItem[j].itemName;
                    itemProduce.item.ClassType = StructResultItem[j].ClassType;
                    itemProduce.item.itemID = StructResultItem[j].itemID;
                    itemProduce.item.itemDesc = StructResultItem[j].itemDesc;
                    itemProduce.item.itemIcon = StructResultItem[j].itemIcon;
                    itemProduce.item.itemModel = StructResultItem[j].itemModel;
                    itemProduce.item.inGameData = StructResultItem[j].inGameData;
                    itemProduce.item.itemValue = StructResultItem[j].itemValue;
                    itemProduce.item.itemType = StructResultItem[j].itemType;
                    itemProduce.item.itemWeight = StructResultItem[j].itemWeight;
                    itemProduce.item.maxStack = StructResultItem[j].maxStack;
                    itemProduce.item.indexItemInList = StructResultItem[j].indexItemInList;
                    itemProduce.item.rarity = StructResultItem[j].rarity;
                    itemProduce.item.itemAttributes = StructResultItem[j].itemAttributes;
                    itemProduce.item.specialPrefabs = StructResultItem[j].specialPrefabs;
                    itemProduce.item.buffDatas = StructResultItem[j].buffDatas;

                    itemObjProduce.transform.SetParent(warriorInventory.transform.GetChild(j));
                    itemObjProduce.transform.localPosition = Vector3.zero;
                    itemObjProduce.transform.localScale = new Vector3(0.4f, 0.4f, 0.4f);

                }
            }
        }
        // �κ��丮�� �������� �ϳ��� ���� ���
        if (count != 0)
        {
            // ���� �������� �ִ��� Ȯ���Ѵ�.
            // ���� �������� ������ Value�� ���Ѵ�.
            // ���� �������� ������ �� ���Կ� �������� �����Ѵ�.
            DrawCardOrganize_two();
        }
    }

    // ī�� ���� �� ��ȭ ������Ʈ ���� --------------------------------------------

    // ----------------------------------------------------------------------------


}

