using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

    // ###############################################
    //             NAME : ARTSUNG                      
    //             MAIL : artsung410@gmail.com         
    // ###############################################
    
public class BuffTooltip : MonoBehaviour
{
    public TextMeshProUGUI nameText;
    public TextMeshProUGUI descriptionText;
    public TextMeshProUGUI serveText;
    public Image itemImage;

    // ���� ���� ����
    public void SetupBuffTooltip(BuffBlueprint buff)
    {
        itemImage.sprite = buff.Icon;
        string buffDescText = buff.Desc;
        descriptionText.text = buffDescText.Replace("\r", "");

        if (buff.Type == (int)Buff_Effect_Type.Buff)
        {
            nameText.text = $"<color=#00FF00>{buff.ToolTipName}</color>";                            
            serveText.text = "Type : Buff";                                           
        }
        else
        {
            nameText.text = $"<color=#DC143C>{buff.ToolTipName}</color>";
            serveText.text = "Type : Debuff";
        }
    }


    // ��ų ���� ����
    public void SetupSkillTooltip(string keyName, Item item)
    {
        // ex. [Q] �����ֵη��
        string tempNameText = $"<color=#FFD700>[{keyName}] {item.skillData.Name.name}</color>";

        // ex. Rank : 1        CoolTime : 7sec
        string tempServeText = $"Rank : {item.skillData.Rank}    /    CoolTime : {item.skillData.CoolTime}";

        // ���� ���� ����
        nameText.text = tempNameText.Replace("\r", "");

        // ���� ������ ����
        serveText.text = tempServeText.Replace("\r", "");

        // ���� ������ ����
        itemImage.sprite = item.skillData.SkillIcon;

        // ���� ���� ����
        descriptionText.text = item.skillData.Desc;
    }

    float halfwidth;
    RectTransform rt;
    private void Start()
    {
        // ������ �ǹ���ǥ�� x���� ���ݺ��� ũ�� ���콺 ���ʿ� ��쵵�� ��.
        halfwidth = GetComponentInParent<CanvasScaler>().referenceResolution.x * 0.5f;
        rt = GetComponent<RectTransform>();
        
    }
    private void Update()
    {
        transform.position = Input.mousePosition;

        //if (rt.anchoredPosition.x + rt.sizeDelta.x > halfwidth)
        //{
        //    rt.pivot = new Vector2(1, 1);
        //}
        //else
        //{
        //    rt.pivot = new Vector2(0, 1);
        //}
    }
}
