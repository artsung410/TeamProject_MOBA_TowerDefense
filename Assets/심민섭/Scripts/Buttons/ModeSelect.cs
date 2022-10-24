using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 버튼이 클릭 되면 ModeSelectImage가 enabled 된다.
public class ModeSelect : MonoBehaviour
{
    // ###############################################
    //             NAME : Simstealer                      
    //             MAIL : minsub4400@gmail.com         
    // ###############################################

    [SerializeField]
    private GameObject nomalMode;
    [SerializeField]
    private GameObject battingMode;

    public void nomalModeSelectOn()
    {
        // 이미지가 꺼저있으면 킨다.
        if (gameObject.transform.GetChild(0).gameObject.activeSelf == false)
        {
            gameObject.transform.GetChild(0).gameObject.SetActive(true);
        }

        // 다른 모드의 이미지는 끈다.
        if (nomalMode.activeSelf == true)
        {
            battingMode.SetActive(false);
        }
    }

    public void battingModeSelectOn()
    {
        // 이미지가 꺼저있으면 킨다.
        if (gameObject.transform.GetChild(0).gameObject.activeSelf == false)
        {
            gameObject.transform.GetChild(0).gameObject.SetActive(true);
        }

        // 다른 모드의 이미지는 끈다.
        if (battingMode.activeSelf == true)
        {
            nomalMode.SetActive(false);
        }
    }
}
