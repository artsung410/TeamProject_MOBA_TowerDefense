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

    // 버프 툴팁 적용
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


    // 스킬 툴팁 적용
    public void SetupSkillTooltip(string keyName, Item item)
    {
        // ex. [Q] 마구휘두루기
        string tempNameText = $"<color=#FFD700>[{keyName}] {item.skillData.Name.name}</color>";

        // ex. Rank : 1        CoolTime : 7sec
        string tempServeText = $"Rank : {item.skillData.Rank}    /    CoolTime : {item.skillData.CoolTime}";

        // 툴팁 제목 적용
        nameText.text = tempNameText.Replace("\r", "");

        // 툴팁 부제목 적용
        serveText.text = tempServeText.Replace("\r", "");

        // 툴팁 아이콘 적용
        itemImage.sprite = item.skillData.SkillIcon;

        // 툴팁 설명 적용
        descriptionText.text = item.skillData.Desc;
    }

    float halfwidth;
    RectTransform rt;
    private void Start()
    {
        // 툴팁의 피벗좌표가 x축의 절반보다 크면 마우스 왼쪽에 띄우도록 함.
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
