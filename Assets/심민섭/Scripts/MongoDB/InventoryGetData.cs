using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 각 직업별, 타워 인벤토리에서 데이터를 가져와 저장한다.
public class InventoryGetData : MonoBehaviour
{
    public static InventoryGetData instance;
    // ###############################################
    //             NAME : Simstealer                      
    //             MAIL : minsub4400@gmail.com         
    // ###############################################

    // 기타 인벤토리(카드팩)
    // 전사 인벤토리(전사 카드)
    // 마법사 인벤토리(마법사 카드)
    // 암살자 인벤토리(제외)
    // 공통 인벤토리(공통 카드)
    // 타워 인벤토리(타워 카드)

    // 아이템을 저장할 리스트
    public List<GameObject> otherInventoryData = new List<GameObject>();
    public List<GameObject> warriorInventoryData = new List<GameObject>();
    public List<GameObject> wizardInventoryData = new List<GameObject>();
    public List<GameObject> inherenceInventoryData = new List<GameObject>();
    public List<GameObject> towerInventoryData = new List<GameObject>();

    public int haveCardCnt;
    public int warriorCardCnt;
    public int wizardCardCnt;
    public int inherenceCardCnt;
    public int towerCardCnt;
    public int otherItemCnt;



    // Other 인벤토리
    private GameObject otherInventory;
    // Warrior 인벤토리
    private GameObject warriorInventory;
    // Wizard 인벤토리
    private GameObject wizardInventory;
    // Tower 인벤토리
    private GameObject towerInventory;
    // Inherence 인벤토리
    private GameObject inherenceInventory;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        // Other 인벤토리
        otherInventory = GameObject.FindGameObjectWithTag("Canvas").transform.GetChild(3).GetChild(2).GetChild(0).GetChild(0).GetChild(0).GetChild(0).gameObject; // Slots
        // Warrior 인벤토리
        warriorInventory = GameObject.FindGameObjectWithTag("Canvas").transform.GetChild(3).GetChild(1).GetChild(0).GetChild(0).GetChild(0).GetChild(0).GetChild(0).GetChild(0).gameObject;
        // Wizard 인벤토리
        wizardInventory = GameObject.FindGameObjectWithTag("Canvas").transform.GetChild(3).GetChild(1).GetChild(0).GetChild(1).GetChild(0).GetChild(0).GetChild(0).GetChild(0).gameObject;
        // Inherence 인벤토리
        inherenceInventory = GameObject.FindGameObjectWithTag("Canvas").transform.GetChild(3).GetChild(1).GetChild(0).GetChild(3).GetChild(0).GetChild(0).GetChild(0).GetChild(0).gameObject;
        // Tower 인벤토리
        towerInventory = GameObject.FindGameObjectWithTag("Canvas").transform.GetChild(3).GetChild(1).GetChild(1).GetChild(0).GetChild(0).GetChild(0).GetChild(0).GetChild(0).gameObject;
    }

    // 함수를 호출하면 인벤토리 내에 있는 아이템을 리스트에 저장한다.
    public void GetItemInInventoryData()
    {
        otherInventoryData.Clear();
        warriorInventoryData.Clear();
        wizardInventoryData.Clear();
        inherenceInventoryData.Clear();
        towerInventoryData.Clear();

        for (int i = 0; i < otherInventory.transform.childCount; i++)
        {
            Debug.Log("기타 아이템 저장 시작");
            if (otherInventory.transform.GetChild(i).childCount != 0)
            {
                otherInventoryData.Add(otherInventory.transform.GetChild(i).GetChild(0).gameObject);
                otherItemCnt += otherInventory.transform.GetChild(i).GetChild(0).gameObject.GetComponent<ItemOnObject>().item.itemValue;
            }
            Debug.Log("기타 아이템 저장 완료");
        }
        for (int i = 0; i < warriorInventory.transform.childCount; i++)
        {
            Debug.Log("전사 아이템 저장 시작");
            if (warriorInventory.transform.GetChild(i).childCount != 0)
            {
                warriorInventoryData.Add(warriorInventory.transform.GetChild(i).GetChild(0).gameObject);
                warriorCardCnt += warriorInventory.transform.GetChild(i).GetChild(0).gameObject.GetComponent<ItemOnObject>().item.itemValue;
            }
            Debug.Log("전사 아이템 저장 완료");
        }
        
        for (int i = 0; i < wizardInventory.transform.childCount; i++)
        {
            Debug.Log("마법사 아이템 저장 시작");
            if (wizardInventory.transform.GetChild(i).childCount != 0)
            {
                wizardInventoryData.Add(wizardInventory.transform.GetChild(i).GetChild(0).gameObject);
                wizardCardCnt += wizardInventory.transform.GetChild(i).GetChild(0).gameObject.GetComponent<ItemOnObject>().item.itemValue;
            }
            Debug.Log("마법사 아이템 저장 완료");
        }
        for (int i = 0; i < inherenceInventory.transform.childCount; i++)
        {
            Debug.Log("공통 아이템 저장 시작");
            if (inherenceInventory.transform.GetChild(i).childCount != 0)
            {
                inherenceInventoryData.Add(inherenceInventory.transform.GetChild(i).GetChild(0).gameObject);
                inherenceCardCnt += inherenceInventory.transform.GetChild(i).GetChild(0).gameObject.GetComponent<ItemOnObject>().item.itemValue;
            }
            Debug.Log("공통 아이템 저장 완료");
        }
        for (int i = 0; i < towerInventory.transform.childCount; i++)
        {
            Debug.Log("타워 아이템 저장 시작");
            if (towerInventory.transform.GetChild(i).childCount != 0)
            {
                towerInventoryData.Add(towerInventory.transform.GetChild(i).GetChild(0).gameObject);
                towerCardCnt += towerInventory.transform.GetChild(i).GetChild(0).gameObject.GetComponent<ItemOnObject>().item.itemValue;
            }
            Debug.Log("타워 아이템 저장 완료");
        }
    }

}
