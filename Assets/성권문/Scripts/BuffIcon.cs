using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
    // ###############################################
    //             NAME : ARTSUNG                      
    //             MAIL : artsung410@gmail.com         
    // ###############################################

public class BuffIcon : MonoBehaviour, IPointerEnterHandler
{
    public int id;
    public BuffData buff;

    private void Start()
    {
        BuffManager.onBuffEvent += SetBuff;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        Debug.Log("Buff½½·Ô¿¡ °®´Ù´ï");
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
