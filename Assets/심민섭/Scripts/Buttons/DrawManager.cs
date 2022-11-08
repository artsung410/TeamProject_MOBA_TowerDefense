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
    public int buyCount; // 0이 아니면 구매한 것 - 아이템 획득 처리 후 0으로 초기화

    [SerializeField]
    private GameObject prefabItem;

    // Other 인벤토리
    private GameObject otherInventory;

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
    private DrawItem firstItem;
    // 새로 들어온 아이템
    private DrawItem secondItem;
    // 새로 들어온 아이템 정보 담는 변수
    private DrawItem drawItem;
    // 새로 들어온 아이템 오브젝트 담는 변수
    private GameObject drawItemObj;

    public void PutCardItem_AfterBuy()
    {
        // 슬롯 수 만큼 반복해서 비어있는 곳을 확인
        for (int i = 0; i < otherInventory.transform.childCount; i++)
        {
            if (otherInventory.transform.GetChild(i).childCount > 0) // 아이템이 이미 있는 경우
            {
                firstItem = otherInventory.transform.GetChild(i).GetChild(0).GetComponent<DrawItem>();
                //Debug.Log(otherInventory.transform.GetChild(i).GetChild(0).name); // DrawBox
                /*drawItem = drawItemObj.GetComponent<DrawItem>();
                secondItem.drawBox.BoxName = selectNameText;*/

                // 아이템 명이 같다면
                if (firstItem.drawBox.BoxName == selectNameText)
                {
                    // Value만 올려준다.
                    Text text = firstItem.transform.GetChild(1).GetComponent<Text>();
                    text.text = (int.Parse(text.text) + buyCount).ToString();

                    Debug.Log("아이템 명이 같음");
                    return;
                }
                else // 이름이 같지 않은 경우
                {
                    if (otherInventory.transform.GetChild(i).childCount == 0)
                    {
                        Debug.Log("아이템 명이 다름");
                        drawItemObj = (GameObject)Instantiate(prefabItem);
                        drawItem = drawItemObj.GetComponent<DrawItem>();
                        drawItem.drawBox.BoxImage = selectImage; // 박스 이미지
                        drawItem.drawBox.BoxName = selectNameText; // 박스 명

                        drawItemObj.transform.SetParent(otherInventory.transform.GetChild(i));
                        drawItemObj.transform.GetChild(0).GetComponent<Image>().sprite =
                            drawItemObj.GetComponent<DrawItem>().drawBox.BoxImage;
                        drawItemObj.transform.GetChild(1).GetComponent<Text>().text = buyCount.ToString();
                        drawItemObj.transform.localPosition = Vector3.zero;
                    }
                }
            }
            // 슬롯이 비어있을 경우
            if (otherInventory.transform.GetChild(i).childCount == 0)
            {
                drawItemObj = (GameObject)Instantiate(prefabItem);
                drawItem = drawItemObj.GetComponent<DrawItem>();
                drawItem.drawBox.BoxImage = selectImage; // 박스 이미지
                drawItem.drawBox.BoxName = selectNameText; // 박스 명

                drawItemObj.transform.SetParent(otherInventory.transform.GetChild(i));
                drawItemObj.transform.GetChild(0).GetComponent<Image>().sprite =
                    drawItemObj.GetComponent<DrawItem>().drawBox.BoxImage;
                drawItemObj.transform.GetChild(1).GetComponent<Text>().text = buyCount.ToString();
                drawItemObj.transform.localPosition = Vector3.zero;
                // 아이템을 넣었다면 
                return;
            }
            
        }
    }
    // 카드 구매 후 재화 업데이트 예정 --------------------------------------------

    // ----------------------------------------------------------------------------
}

