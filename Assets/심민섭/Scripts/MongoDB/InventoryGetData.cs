using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// �� ������, Ÿ�� �κ��丮���� �����͸� ������ �����Ѵ�.
public class InventoryGetData : MonoBehaviour
{
    public static InventoryGetData instance;
    // ###############################################
    //             NAME : Simstealer                      
    //             MAIL : minsub4400@gmail.com         
    // ###############################################

    // ��Ÿ �κ��丮(ī����)
    // ���� �κ��丮(���� ī��)
    // ������ �κ��丮(������ ī��)
    // �ϻ��� �κ��丮(����)
    // ���� �κ��丮(���� ī��)
    // Ÿ�� �κ��丮(Ÿ�� ī��)

    // �������� ������ ����Ʈ
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



    // Other �κ��丮
    private GameObject otherInventory;
    // Warrior �κ��丮
    private GameObject warriorInventory;
    // Wizard �κ��丮
    private GameObject wizardInventory;
    // Tower �κ��丮
    private GameObject towerInventory;
    // Inherence �κ��丮
    private GameObject inherenceInventory;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        // Other �κ��丮
        otherInventory = GameObject.FindGameObjectWithTag("Canvas").transform.GetChild(3).GetChild(2).GetChild(0).GetChild(0).GetChild(0).GetChild(0).gameObject; // Slots
        // Warrior �κ��丮
        warriorInventory = GameObject.FindGameObjectWithTag("Canvas").transform.GetChild(3).GetChild(1).GetChild(0).GetChild(0).GetChild(0).GetChild(0).GetChild(0).GetChild(0).gameObject;
        // Wizard �κ��丮
        wizardInventory = GameObject.FindGameObjectWithTag("Canvas").transform.GetChild(3).GetChild(1).GetChild(0).GetChild(1).GetChild(0).GetChild(0).GetChild(0).GetChild(0).gameObject;
        // Inherence �κ��丮
        inherenceInventory = GameObject.FindGameObjectWithTag("Canvas").transform.GetChild(3).GetChild(1).GetChild(0).GetChild(3).GetChild(0).GetChild(0).GetChild(0).GetChild(0).gameObject;
        // Tower �κ��丮
        towerInventory = GameObject.FindGameObjectWithTag("Canvas").transform.GetChild(3).GetChild(1).GetChild(1).GetChild(0).GetChild(0).GetChild(0).GetChild(0).GetChild(0).gameObject;
    }

    // �Լ��� ȣ���ϸ� �κ��丮 ���� �ִ� �������� ����Ʈ�� �����Ѵ�.
    public void GetItemInInventoryData()
    {
        otherInventoryData.Clear();
        warriorInventoryData.Clear();
        wizardInventoryData.Clear();
        inherenceInventoryData.Clear();
        towerInventoryData.Clear();

        for (int i = 0; i < otherInventory.transform.childCount; i++)
        {
            Debug.Log("��Ÿ ������ ���� ����");
            if (otherInventory.transform.GetChild(i).childCount != 0)
            {
                otherInventoryData.Add(otherInventory.transform.GetChild(i).GetChild(0).gameObject);
                otherItemCnt += otherInventory.transform.GetChild(i).GetChild(0).gameObject.GetComponent<ItemOnObject>().item.itemValue;
            }
            Debug.Log("��Ÿ ������ ���� �Ϸ�");
        }
        for (int i = 0; i < warriorInventory.transform.childCount; i++)
        {
            Debug.Log("���� ������ ���� ����");
            if (warriorInventory.transform.GetChild(i).childCount != 0)
            {
                warriorInventoryData.Add(warriorInventory.transform.GetChild(i).GetChild(0).gameObject);
                warriorCardCnt += warriorInventory.transform.GetChild(i).GetChild(0).gameObject.GetComponent<ItemOnObject>().item.itemValue;
            }
            Debug.Log("���� ������ ���� �Ϸ�");
        }
        
        for (int i = 0; i < wizardInventory.transform.childCount; i++)
        {
            Debug.Log("������ ������ ���� ����");
            if (wizardInventory.transform.GetChild(i).childCount != 0)
            {
                wizardInventoryData.Add(wizardInventory.transform.GetChild(i).GetChild(0).gameObject);
                wizardCardCnt += wizardInventory.transform.GetChild(i).GetChild(0).gameObject.GetComponent<ItemOnObject>().item.itemValue;
            }
            Debug.Log("������ ������ ���� �Ϸ�");
        }
        for (int i = 0; i < inherenceInventory.transform.childCount; i++)
        {
            Debug.Log("���� ������ ���� ����");
            if (inherenceInventory.transform.GetChild(i).childCount != 0)
            {
                inherenceInventoryData.Add(inherenceInventory.transform.GetChild(i).GetChild(0).gameObject);
                inherenceCardCnt += inherenceInventory.transform.GetChild(i).GetChild(0).gameObject.GetComponent<ItemOnObject>().item.itemValue;
            }
            Debug.Log("���� ������ ���� �Ϸ�");
        }
        for (int i = 0; i < towerInventory.transform.childCount; i++)
        {
            Debug.Log("Ÿ�� ������ ���� ����");
            if (towerInventory.transform.GetChild(i).childCount != 0)
            {
                towerInventoryData.Add(towerInventory.transform.GetChild(i).GetChild(0).gameObject);
                towerCardCnt += towerInventory.transform.GetChild(i).GetChild(0).gameObject.GetComponent<ItemOnObject>().item.itemValue;
            }
            Debug.Log("Ÿ�� ������ ���� �Ϸ�");
        }
    }

}
