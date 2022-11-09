using System.Collections;
using System.Collections.Generic;
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

    // ������ �̹���
    public Sprite selectImage;
    // ������ ������ ��
    public string selectNameText;
    // ������ �������� ����
    public string selectExplanationText;


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

    // ���� ��� �ִ� �ڽ� ���� ����
    [Header("Box ����")]
    public ItemOnObject boxItem;
    public Sprite boxImage;
    public string boxName;

    // ȹ���� �������� ���� ����
    public List<GameObject> getItemList = new List<GameObject>();

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        buyCurencyName = "Zera";

        // Other �κ��丮
        otherInventory = GameObject.FindGameObjectWithTag("Canvas").transform.GetChild(3).GetChild(2).GetChild(0).GetChild(0).GetChild(0).GetChild(0).gameObject; // Slots

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
                        drawItem.item.itemType = ItemType.Consumable;
                        //drawItem.item.ClassType = "Box";

                        drawItemObj.transform.SetParent(otherInventory.transform.GetChild(i));
                        drawItemObj.transform.GetChild(0).GetComponent<Image>().sprite =
                            drawItemObj.GetComponent<ItemOnObject>().item.itemIcon;
                        drawItemObj.transform.GetChild(1).GetComponent<Text>().text = buyCount.ToString();
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
                drawItem.item.itemType = ItemType.Consumable;
                //drawItem.item.ClassType = "Box";

                drawItemObj.transform.SetParent(otherInventory.transform.GetChild(i));
                drawItemObj.transform.GetChild(0).GetComponent<Image>().sprite =
                    drawItemObj.GetComponent<ItemOnObject>().item.itemIcon;
                drawItemObj.transform.GetChild(1).GetComponent<Text>().text = buyCount.ToString();
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
            boxItem.item.itemValue -= 1;
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

    // ī�� �̱� �Լ�
    public void RandomCardDraw()
    {

    }
    // ī�� ���� �� ��ȭ ������Ʈ ���� --------------------------------------------

    // ----------------------------------------------------------------------------
}

