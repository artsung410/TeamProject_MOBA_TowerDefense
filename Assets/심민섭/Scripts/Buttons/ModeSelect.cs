using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// ��ư�� Ŭ�� �Ǹ� ModeSelectImage�� enabled �ȴ�.
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
        // �̹����� ���������� Ų��.
        if (gameObject.transform.GetChild(0).gameObject.activeSelf == false)
        {
            gameObject.transform.GetChild(0).gameObject.SetActive(true);
        }

        // �ٸ� ����� �̹����� ����.
        if (nomalMode.activeSelf == true)
        {
            battingMode.SetActive(false);
        }
    }

    public void battingModeSelectOn()
    {
        // �̹����� ���������� Ų��.
        if (gameObject.transform.GetChild(0).gameObject.activeSelf == false)
        {
            gameObject.transform.GetChild(0).gameObject.SetActive(true);
        }

        // �ٸ� ����� �̹����� ����.
        if (battingMode.activeSelf == true)
        {
            nomalMode.SetActive(false);
        }
    }
}
