using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TextOverFlow : MonoBehaviour
{
    // ###############################################
    //             NAME : KimJaeMin                      
    //             MAIL : woals1566@gmail.com         
    // ###############################################

   public TMP_InputField text;   
    public bool textNotEnter { get; private set; }
    private void Start()
    {
        text = GetComponent<TMP_InputField>();
       
    }

    // Update is called once per frame
    void Update()
    {

        if (text.text.Length <= 0 && gameObject.activeSelf) // 
        {
            textNotEnter = true;
        }
        else
        {
            textNotEnter = false;
        }
        
        if(text.text.Length >= 176)
        {
            transform.GetChild(0).GetChild(5).gameObject.SetActive(true);
        }
        else
        {
            transform.GetChild(0).GetChild(5).gameObject.SetActive(false);
        }
    }
}
