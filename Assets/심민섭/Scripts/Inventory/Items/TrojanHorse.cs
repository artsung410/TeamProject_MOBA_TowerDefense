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
    public int playerNumber;

    [Header("ī�� ���� ID")]
    public List<int> cardId = new List<int>();

    [Header("������ ī�� �ε���")]
    public List<int> cardIndex = new List<int>();

    [Header("������ ī�� ��")]
    public List<string> cardName = new List<string>();

    [Header("������ ������")]
    public List<GameObject> cardPrefab  = new List<GameObject>();

    private ItemOnObject itemOnObject;

    private GameObject EquipmentItemInventory;

    public void PlayerTrojanInfo()
    {
        // PlayerNumber �ޱ�
        playerNumber = GetComponent<PlayerStorage>().playerNumber;

        //ItemOnObject���� �����´�.

        // ���� ���� ������Ʈ ��������
        EquipmentItemInventory = GameObject.FindGameObjectWithTag("Inventory").transform.GetChild(0).GetChild(0).GetChild(1).GetChild(1).GetChild(1).GetChild(1).gameObject;

        // ���� ������Ʈ���� ����Ʈ�� �ִ´�.
        List<GameObject> TrojanDataList= new List<GameObject>();
        for (int i = 0; i < EquipmentItemInventory.transform.childCount; i++)
        {
            TrojanDataList.Add(EquipmentItemInventory.transform.GetChild(i).gameObject);
            //Debug.Log(TrojanDataList[i].name);
        }

        for (int i = 0; i < TrojanDataList.Count; i++)
        {
            // count�� 1�̸� �ε��� ����
            //Debug.Log(TrojanDataList[i].transform.childCount);
            if (TrojanDataList[i].transform.childCount == 1)
            {
                cardIndex.Add(i);
                // �׸��� �����͸� ������
                ItemOnObject itemOnObject = TrojanDataList[i].gameObject.transform.GetChild(0).GetComponent<ItemOnObject>();
                cardId.Add(itemOnObject.item.itemID);
                cardName.Add(itemOnObject.item.itemName);
                cardPrefab.Add(itemOnObject.item.itemModel);
            }
        }
        // ������Ʈ �����ͼ� ���� ItemOnObject
        //itemOnObject = Get
    }
}
