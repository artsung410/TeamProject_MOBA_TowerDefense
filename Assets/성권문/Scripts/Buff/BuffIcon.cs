using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
    // ###############################################
    //             NAME : ARTSUNG                      
    //             MAIL : artsung410@gmail.com         
    // ###############################################

public class BuffIcon : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public int id;
    public BuffData buff;
    public BuffTooltip tooltip;

    private void Start()
    {
        BuffManager.onBuffEvent += SetBuff;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (buff != null)
        {
            tooltip.gameObject.SetActive(true);
            tooltip.SetupTooltip(buff.Name, buff.Desc);
        }

    }

    public void OnPointerExit(PointerEventData eventData)
    {
        tooltip.gameObject.SetActive(false);
    }

    public void SetBuff(int inputId)
    {
        if (inputId != id)
        {
            return;
        }

        gameObject.GetComponent<Image>().sprite = buff.buffIcon;
        Color color = gameObject.GetComponent<Image>().color;
        color.a = 1f;
        gameObject.GetComponent<Image>().color = color;
    }


}
