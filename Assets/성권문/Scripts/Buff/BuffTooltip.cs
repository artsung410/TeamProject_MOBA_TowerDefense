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
    public TextMeshProUGUI skillColltimeText;

    public void SetupBuffTooltip(string className, BuffBlueprint buff)
    {
        nameText.text = buff.Name;
        skillColltimeText.enabled = false;
        string valueText = buff.Value.ToString();
        string buffDescText = buff.Desc;

        if (buff.Type == (int)Buff_Effect_Type.Buff)
        {
            string desc = buffDescText + $"<color=#228B22> +{valueText} </color>";
            //descriptionText.text = buff.Desc + "<color=#228B22>" + "+" + valueText + "</color>";
            descriptionText.text = desc.Replace("\r", "");
        }
        else
        {
            //descriptionText.text = buff.Desc + "<color=#DC143C>" + valueText + "</color>";
            descriptionText.text = $"{buffDescText} <color=#DC143C> +{valueText} </color>";
        }
    }

    public void SetupSkillTooltip(string className, Item item)
    {
        nameText.text = item.skillData.Name.name;
        descriptionText.text = item.skillData.Desc;
        skillColltimeText.enabled = true;
        string valueText = item.skillData.CoolTime.ToString();
        skillColltimeText.text = "( " + valueText + " Sec )";
        skillColltimeText.color = Color.magenta;
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
