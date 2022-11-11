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
    // 버튼 정보 저장 변수
    public static DrawManager instance;

    [Header("선택된 아이템 정보")]
    // 선택한 이미지
    public Sprite selectImage;
    // 선택한 아이템 명
    public string selectNameText;
    // 선택한 아이템의 설명
    public string selectExplanationText;

    [Header("구매한 아이템 정보")]
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
    // Warrior 인벤토리
    private GameObject warriorInventory;

    // 현재 까고 있는 박스 정보 저장
    [Header("Box 정보")]
    public ItemOnObject boxItem;
    public Sprite boxImage;
    public string boxName;
    public int boxCount; // 까버릴 갯수

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        buyCount = 1;
        boxCount = 1;
        buyCurencyName = "Zera";

        // Other 인벤토리
        otherInventory = GameObject.FindGameObjectWithTag("Canvas").transform.GetChild(3).GetChild(2).GetChild(0).GetChild(0).GetChild(0).GetChild(0).gameObject; // Slots
        // Warrior 인벤토리
        warriorInventory = GameObject.FindGameObjectWithTag("Canvas").transform.GetChild(3).GetChild(1).GetChild(0).GetChild(0).GetChild(0).GetChild(0).GetChild(0).GetChild(0).gameObject;

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
            // 슬롯이 비어있을 경우
            if (otherInventory.transform.GetChild(i).childCount == 0)
            {
                drawItemObj = (GameObject)Instantiate(prefabItem);
                drawItem = drawItemObj.GetComponent<ItemOnObject>();
                drawItem.item.itemIcon = buyItemImage.sprite; // 박스 이미지
                drawItem.item.itemName = buyItemName; // 박스 명
                drawItem.item.itemValue = buyCount;
                drawItem.item.itemType = ItemType.Consumable;
                //drawItem.item.ClassType = "Box";

                drawItemObj.transform.SetParent(otherInventory.transform.GetChild(i));
                drawItemObj.transform.GetChild(0).GetComponent<Image>().sprite =
                    drawItemObj.GetComponent<ItemOnObject>().item.itemIcon;
                //drawItemObj.transform.GetChild(1).GetComponent<Text>().text = buyCount.ToString();
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

    // 개수 10개 이하로 인해 버튼 비활성화 함수
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

    // 획득한 아이템 인덱스을 담을 공간
    public List<int> getDrawResult = new List<int>();

    // 획득한 아이템 오브젝트을 담을 공간(RandomSelect -> DrawManager)
    public List<Item> getDrawResultItem = new List<Item>();

    // 이게 진짜 진짜 드로우해서 뽑은 카드임!!
    public List<Item> ResultItem = new List<Item>();

    // 카드 오픈 시 아이템이 인벤토리로 들어간다.
    // 1. 카드는 오픈 시 getItemList로 데이터를 넣는다.
    // 2. 인벤토리에 같은 카드데이터가 있는지 확인 한다.
    // 3. 같은 카드데이터가 있으면 Value만 올린다.
    // 4. 같은 카드데이터가 없으면 빈 슬롯에 오브젝트를 생성한다.

    [SerializeField]
    private GameObject alreadyCardDraw;
    // Back 버튼을 클릭 시 뽑기 카드 데이터 초기화와 함께 창이 닫힌다.
    public void CardDataInit()
    {
        //Debug.Log(alreadyCardDraw.transform.childCount);
        for (int i = 0; i < alreadyCardDraw.transform.childCount; i++)
        {
            //Debug.Log(alreadyCardDraw.transform.GetChild(i).name);
            Destroy(alreadyCardDraw.transform.GetChild(i).gameObject);
        }
    }

    // 뽑은 카드 정리(자료구조에서 중복 값 찾기)
    private void DrawCardOrganize_one()
    {   
        // 아이템이 없을 경우 실행
        var duplicates = getDrawResultItem.GroupBy(x => x)
                                        .SelectMany(g => g.Skip(1))
                                        .Distinct()
                                        .ToList();
        var duplicatesCnt = duplicates.Count;

        ResultItem = getDrawResultItem.Distinct().ToList();

        for (int i = 0; i < duplicatesCnt; i++) // 2
        {
            for (int j = 0; j < ResultItem.Count; j++) // 8
            {
                if (duplicates[i].itemID == ResultItem[j].itemID) // 30, 37  == 30, 
                {
                    ResultItem[j].itemValue += 1;
                    continue;
                }
            }
        }
        /*Debug.Log(duplicates.Count);
        Debug.Log(duplicates[0].itemID);*/
    }

    private void DrawCardOrganize_two()
    {
        // 아이템이 이미 아이템에 존재할 경우 실행
    }

    // Retry 버튼을 클릭 시 다시 이전 갯수대로 뽑는다. 
    public void RandomCardDraw()
    {

    }

    private GameObject itemObjProduce;
    private ItemOnObject itemProduce;
    // 아이템 오브젝트 생성 및 이동 함수
    public void ItemProduceAndIventoryMove()
    {
        DrawCardOrganize_one();
        //Debug.Log($"getDrawResultItem.Count : {getDrawResultItem.Count}"); // OK
        for (int i = 0; i < ResultItem.Count; i++) // 10
        {
            // 아이템 오브젝트를 복제한다.
            itemObjProduce = (GameObject)Instantiate(prefabItem);
            // ItemOnObject 스크립트를 가져온다.
            itemProduce = itemObjProduce.GetComponent<ItemOnObject>();
            // 아이템 정보를 복사한다.
            itemProduce.item = ResultItem[i];

            itemObjProduce.transform.SetParent(warriorInventory.transform.GetChild(i));
            itemObjProduce.transform.localPosition = Vector3.zero;
            itemObjProduce.transform.localScale = new Vector3(0.4f, 0.4f, 0.4f);
        }
    }

    // 카드 구매 후 재화 업데이트 예정 --------------------------------------------

    // ----------------------------------------------------------------------------


}

