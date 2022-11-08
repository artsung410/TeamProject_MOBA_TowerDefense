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
    public int buyCount; // 0�� �ƴϸ� ������ �� - ������ ȹ�� ó�� �� 0���� �ʱ�ȭ

    [SerializeField]
    private GameObject prefabItem;

    // Other �κ��丮
    private GameObject otherInventory;

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
    private DrawItem firstItem;
    // ���� ���� ������
    private DrawItem secondItem;
    // ���� ���� ������ ���� ��� ����
    private DrawItem drawItem;
    // ���� ���� ������ ������Ʈ ��� ����
    private GameObject drawItemObj;

    public void PutCardItem_AfterBuy()
    {
        // ���� �� ��ŭ �ݺ��ؼ� ����ִ� ���� Ȯ��
        for (int i = 0; i < otherInventory.transform.childCount; i++)
        {
            if (otherInventory.transform.GetChild(i).childCount > 0) // �������� �̹� �ִ� ���
            {
                firstItem = otherInventory.transform.GetChild(i).GetChild(0).GetComponent<DrawItem>();
                //Debug.Log(otherInventory.transform.GetChild(i).GetChild(0).name); // DrawBox
                /*drawItem = drawItemObj.GetComponent<DrawItem>();
                secondItem.drawBox.BoxName = selectNameText;*/

                // ������ ���� ���ٸ�
                if (firstItem.drawBox.BoxName == selectNameText)
                {
                    // Value�� �÷��ش�.
                    Text text = firstItem.transform.GetChild(1).GetComponent<Text>();
                    text.text = (int.Parse(text.text) + buyCount).ToString();

                    Debug.Log("������ ���� ����");
                    return;
                }
                else // �̸��� ���� ���� ���
                {
                    if (otherInventory.transform.GetChild(i).childCount == 0)
                    {
                        Debug.Log("������ ���� �ٸ�");
                        drawItemObj = (GameObject)Instantiate(prefabItem);
                        drawItem = drawItemObj.GetComponent<DrawItem>();
                        drawItem.drawBox.BoxImage = selectImage; // �ڽ� �̹���
                        drawItem.drawBox.BoxName = selectNameText; // �ڽ� ��

                        drawItemObj.transform.SetParent(otherInventory.transform.GetChild(i));
                        drawItemObj.transform.GetChild(0).GetComponent<Image>().sprite =
                            drawItemObj.GetComponent<DrawItem>().drawBox.BoxImage;
                        drawItemObj.transform.GetChild(1).GetComponent<Text>().text = buyCount.ToString();
                        drawItemObj.transform.localPosition = Vector3.zero;
                    }
                }
            }
            // ������ ������� ���
            if (otherInventory.transform.GetChild(i).childCount == 0)
            {
                drawItemObj = (GameObject)Instantiate(prefabItem);
                drawItem = drawItemObj.GetComponent<DrawItem>();
                drawItem.drawBox.BoxImage = selectImage; // �ڽ� �̹���
                drawItem.drawBox.BoxName = selectNameText; // �ڽ� ��

                drawItemObj.transform.SetParent(otherInventory.transform.GetChild(i));
                drawItemObj.transform.GetChild(0).GetComponent<Image>().sprite =
                    drawItemObj.GetComponent<DrawItem>().drawBox.BoxImage;
                drawItemObj.transform.GetChild(1).GetComponent<Text>().text = buyCount.ToString();
                drawItemObj.transform.localPosition = Vector3.zero;
                // �������� �־��ٸ� 
                return;
            }
            
        }
    }
    // ī�� ���� �� ��ȭ ������Ʈ ���� --------------------------------------------

    // ----------------------------------------------------------------------------
}

