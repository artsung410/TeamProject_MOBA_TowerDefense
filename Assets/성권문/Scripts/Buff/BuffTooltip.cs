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

    public void SetupTooltip(string name, string des)
    {
        nameText.text = name;
        descriptionText.text = des;
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
    }
}
