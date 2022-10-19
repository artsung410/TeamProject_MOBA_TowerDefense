using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrojanHorse: MonoBehaviour
{
    // ###############################################
    //             NAME : Simstealer                      
    //             MAIL : minsub4400@gmail.com         
    // ###############################################
    [Header("�÷��̾� ��ȣ (������ ���� ���� ����)")]
    [SerializeField]
    private int playerNumber;

    [Header("Ÿ�� ī�� ���� ID")]
    [SerializeField]
    private List<int> cardId = new List<int>();

    [Header("Ÿ�� ������ ī�� �ε���")]
    [SerializeField]
    private List<int> cardIndex = new List<int>();

    [Header("Ÿ�� ������ ī�� ��")]
    [SerializeField]
    private List<string> cardName = new List<string>();

    [Header("Ÿ�� ������ ������")]
    [SerializeField]
    private List<GameObject> cardPrefab  = new List<GameObject>();

    private ItemOnObject itemOnObject;

    private GameObject EquipmentItemInventory;

    // ----------- �¿��� ���� ���� �߰� ����---------------
    [Header("��ų ī�� ���� ID")]
    [SerializeField]
    private List<int> skillId = new List<int>();

    [Header("��ų ������ ī�� �ε���")]
    [SerializeField]
    private List<int> skillIndex = new List<int>();

    [Header("��ų ������ ī�� ��")]
    [SerializeField]
    private List<string> skillCName = new List<string>();

    [Header("��ų ���ݷ�")]
    [SerializeField]
    private List<int> skillATK = new List<int>();

    [Header("��ų ��Ÿ�")]
    [SerializeField]
    private List<int> skillCrossroad = new List<int>();

    [Header("��ų ��Ÿ��")]
    [SerializeField]
    private List<int> skillCoolTime = new List<int>();

    // ���ݷ�, ��Ÿ�, ��Ÿ��
    [SerializeField]
    private ItemDataBaseList itemDatabase;

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.T))
        {
            PlayerTrojanInfo();
        }
    }

    public void PlayerTrojanInfo()
    {
        // PlayerNumber �ޱ�
        playerNumber = GetComponent<PlayerStorage>().playerNumber;

        //ItemOnObject���� �����´�.

        // ���� ���� ������Ʈ ��������
        EquipmentItemInventory = GameObject.FindGameObjectWithTag("Inventory").transform.GetChild(0).GetChild(1).GetChild(1).GetChild(1).GetChild(1).GetChild(1).gameObject;

        // ���� ������Ʈ���� ����Ʈ�� �ִ´�.
        List<GameObject> TrojanDataList= new List<GameObject>();
        for (int i = 0; i < EquipmentItemInventory.transform.childCount; i++)
        {
            TrojanDataList.Add(EquipmentItemInventory.transform.GetChild(i).gameObject);
        }

        for (int i = 0; i < TrojanDataList.Count; i++)
        {
            // count�� 1�̸� �ε��� ����(�������� ��� �ִٴ� ��)
            if (TrojanDataList[i].transform.childCount == 1)
            {
                if (i <= 3) // 0, 1, 2, 3
                {
                    skillIndex.Add(i);
                    // �׸��� �����͸� ������
                    ItemOnObject itemOnObject = TrojanDataList[i].gameObject.transform.GetChild(0).GetComponent<ItemOnObject>();
                    skillId.Add(itemOnObject.item.itemID);
                    skillCName.Add(itemOnObject.item.itemName);
                }

                // �����ϰ� �ִ� �������� ���� ID�� ������ ���̽����� ã�Ƽ� �����͸� �ҷ��´�.
                if (i == 0)
                    Debug.Log(itemDatabase.itemList[i + 1].itemAttributes[0].attributeValue); // 10
                                                                                              //skillATK.Add(itemDatabase.itemList[i + 1].itemAttributes[i].attributeValue);
                if (i == 1)
                    //skillCrossroad.Add(itemDatabase.itemList[i + 1].itemAttributes[i].attributeValue);
                    Debug.Log(itemDatabase.itemList[i + 1].itemAttributes[1].attributeValue);
                if (i == 2)
                    //skillCoolTime.Add(itemDatabase.itemList[i + 1].itemAttributes[i].attributeValue);
                    Debug.Log(itemDatabase.itemList[i + 1].itemAttributes[1].attributeValue);

                if (i > 3) // 4, 5, 6, 7
                {
                    cardIndex.Add(i);
                    // �׸��� �����͸� ������
                    ItemOnObject itemOnObject = TrojanDataList[i].gameObject.transform.GetChild(0).GetComponent<ItemOnObject>();
                    cardId.Add(itemOnObject.item.itemID);
                    cardName.Add(itemOnObject.item.itemName);
                    cardPrefab.Add(itemOnObject.item.itemModel);
                }
            }
        }
    }
}
