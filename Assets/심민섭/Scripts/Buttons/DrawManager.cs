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
    // 버튼 정보 저장 변수
    public static DrawManager instance;

    // 선택한 이미지
    public Sprite selectImage;
    // 선택한 아이템 명
    public string selectNameText;
    // 선택한 아이템의 설명
    public string selectExplanationText;


    // 구매한 아이템의 재화명
    public string buyCurencyName;
    // 구매한 카드 개수
    public int buyCount; // 0이 아니면 구매한 것 - 아이템 획득 처리 후 0으로 초기화\
    // 구매한 아이템의 이미지
    public Image buyItemImage;
    // 구매한 아이템 명
    public string buyItemName;

    [SerializeField]
    private GameObject prefabItem;

    // Other 인벤토리
    private GameObject otherInventory;

    // 현재 까고 있는 박스 정보 저장
    [Header("Box 정보")]
    public ItemOnObject boxItem;
    public Sprite boxImage;
    public string boxName;

    // 획득한 아이템을 담을 공간
    public List<GameObject> getItemList = new List<GameObject>();

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        buyCurencyName = "Zera";

        // Other 인벤토리
        otherInventory = GameObject.FindGameObjectWithTag("Canvas").transform.GetChild(3).GetChild(2).GetChild(0).GetChild(0).GetChild(0).GetChild(0).gameObject; // Slots

        // 아이템 DB에 들어온 id와 같은 아이템이 있으면 해당 아이템의 오브젝트를 복사해서 itemOnObject.item에 넣어 준다.
        //itemOnObject.item = itemDatabase.getItemByID(id);
    }

    // 이미 있는 아이템 아이템
    private ItemOnObject firstItem;
    // 새로 들어온 아이템
    private ItemOnObject secondItem;
    // 새로 들어온 아이템 정보 담는 변수
    private ItemOnObject drawItem;
    // 새로 들어온 아이템 오브젝트 담는 변수
    private GameObject drawItemObj;

    public void PutCardItem_AfterBuy()
    {
        // 슬롯 수 만큼 반복해서 비어있는 곳을 확인
        for (int i = 0; i < otherInventory.transform.childCount; i++)
        {
            if (otherInventory.transform.GetChild(i).childCount > 0) // 아이템이 이미 있는 경우
            {
                firstItem = otherInventory.transform.GetChild(i).GetChild(0).GetComponent<ItemOnObject>();
                //Debug.Log(otherInventory.transform.GetChild(i).GetChild(0).name); // DrawBox
                /*drawItem = drawItemObj.GetComponent<DrawItem>();
                secondItem.drawBox.BoxName = selectNameText;*/

                // 이미 있는 아이템 명과 구매한 아이템 명 비교
                if (firstItem.item.itemName == buyItemName)
                {
                    // Value만 올려준다.
                    /*Text text = firstItem.transform.GetChild(1).GetComponent<Text>();
                    text.text = (int.Parse(text.text) + buyCount).ToString();*/

                    firstItem.item.itemValue += buyCount;

                    Debug.Log("아이템 명이 같음");
                    return;
                }
                else // 이름이 같지 않은 경우
                {
                    if (otherInventory.transform.GetChild(i).childCount == 0)
                    {
                        Debug.Log("아이템 명이 다름");
                        drawItemObj = (GameObject)Instantiate(prefabItem);
                        drawItem = drawItemObj.GetComponent<ItemOnObject>();
                        drawItem.item.itemIcon = buyItemImage.sprite; // 박스 이미지
                        drawItem.item.itemName = buyItemName; // 박스 명
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
            // 슬롯이 비어있을 경우
            if (otherInventory.transform.GetChild(i).childCount == 0)
            {
                drawItemObj = (GameObject)Instantiate(prefabItem);
                drawItem = drawItemObj.GetComponent<ItemOnObject>();
                drawItem.item.itemIcon = buyItemImage.sprite; // 박스 이미지
                drawItem.item.itemName = buyItemName; // 박스 명
                drawItem.item.itemType = ItemType.Consumable;
                //drawItem.item.ClassType = "Box";

                drawItemObj.transform.SetParent(otherInventory.transform.GetChild(i));
                drawItemObj.transform.GetChild(0).GetComponent<Image>().sprite =
                    drawItemObj.GetComponent<ItemOnObject>().item.itemIcon;
                drawItemObj.transform.GetChild(1).GetComponent<Text>().text = buyCount.ToString();
                drawItemObj.transform.localPosition = Vector3.zero;
                drawItemObj.transform.localScale = new Vector3(1.5f, 1.5f, 1.5f);
                // 아이템을 넣었다면 
                return;
            }
            
        }
    }

    // 아이템 소모 함수
    public void OpenBoxDisCount()
    {
        // 수량
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
            // 수량이 0보다 작으면 사라짐
            if (item.itemValue <= 0)
            {
                if (tooltip != null)
                    tooltip.deactivateTooltip();
                inventory.deleteItemFromInventory(item);
                Destroy(this.gameObject);
            }
        }*/
    }

    // 카드 뽑기 함수
    public void RandomCardDraw()
    {

    }
    // 카드 구매 후 재화 업데이트 예정 --------------------------------------------

    // ----------------------------------------------------------------------------
}

