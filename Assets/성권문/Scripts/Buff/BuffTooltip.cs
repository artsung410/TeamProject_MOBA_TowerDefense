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
        // 툴팁의 피벗좌표가 x축의 절반보다 크면 마우스 왼쪽에 띄우도록 함.
        halfwidth = GetComponentInParent<CanvasScaler>().referenceResolution.x * 0.5f;
        rt = GetComponent<RectTransform>();
        
    }
    private void Update()
    {
        transform.position = Input.mousePosition;
    }
}
