using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemManager : MonoBehaviour
{
    // ###############################################
    //             NAME : Simstealer                      
    //             MAIL : minsub4400@gmail.com         
    // ###############################################

    // �������� �����ؼ� ī���Ѵ�.
    // ��Ī��ư�� ������ �����Ѵ�.

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
