using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class JobButtonImageChange : MonoBehaviour
{
    // ###############################################
    //             NAME : Simstealer                      
    //             MAIL : minsub4400@gmail.com         
    // ###############################################

    // ��ư�� ������ �̹����� ����ȴ�.

    // �̹��� ������
    [SerializeField]
    private Sprite originalImageName;
    [SerializeField]
    private Sprite changeImageName;

    public GameObject warrior;
    public GameObject wizard;
    public GameObject assassin;
    public GameObject inherence;

    public void warriorButtonImageChange()
    {
        // ����� �̹����� ����.
        gameObject.GetComponent<Image>().sprite = changeImageName;
        wizard.GetComponent<Image>().sprite = originalImageName;
        assassin.GetComponent<Image>().sprite = originalImageName;
        inherence.GetComponent<Image>().sprite = originalImageName;
    }

    public void wizardButtonImageChange()
    {
        // ����� �̹����� ����.
        gameObject.GetComponent<Image>().sprite = changeImageName;
        warrior.GetComponent<Image>().sprite = originalImageName;
        assassin.GetComponent<Image>().sprite = originalImageName;
        inherence.GetComponent<Image>().sprite = originalImageName;
    }

    public void assassinButtonImageChange()
    {
        // ����� �̹����� ����.
        gameObject.GetComponent<Image>().sprite = changeImageName;
        warrior.GetComponent<Image>().sprite = originalImageName;
        wizard.GetComponent<Image>().sprite = originalImageName;
        inherence.GetComponent<Image>().sprite = originalImageName;
    }

    public void inherenceButtonImageChange()
    {
        // ����� �̹����� ����.
        gameObject.GetComponent<Image>().sprite = changeImageName;
        warrior.GetComponent<Image>().sprite = originalImageName;
        assassin.GetComponent<Image>().sprite = originalImageName;
        wizard.GetComponent<Image>().sprite = originalImageName;
    }
}
