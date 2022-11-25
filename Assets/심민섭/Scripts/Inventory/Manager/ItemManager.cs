using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemManager : MonoBehaviour
{
    // ###############################################
    //             NAME : Simstealer                      
    //             MAIL : minsub4400@gmail.com         
    // ###############################################

    // 아이템을 복사해서 카피한다.
    // 매칭버튼을 누르면 복사한다.

    private GameObject warriorSlots;

    private void Start()
    {
        warriorSlots = GameObject.FindGameObjectWithTag("Canvas").transform.GetChild(1).GetChild(0).GetChild(0).GetChild(0).GetChild(0).GetChild(0).GetChild(0).gameObject;
    }


    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.T))
        {

        }
    }

}
