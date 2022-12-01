using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class SkillButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    // ###############################################
    //             NAME : ARTSUNG                      
    //             MAIL : artsung410@gmail.com         
    // ###############################################
    public Item item;
    public BuffTooltip tooltip;

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (item != null)
        {
            tooltip.gameObject.SetActive(true);
            tooltip.SetupSkillTooltip(name, item);
            Debug.Log(GetType().Name);
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (item != null)
        {
            tooltip.gameObject.SetActive(false);
        }
    }
}
