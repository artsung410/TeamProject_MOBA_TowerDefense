using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrawManager : MonoBehaviour
{
    // ###############################################
    //             NAME : Simstealer                      
    //             MAIL : minsub4400@gmail.com         
    // ###############################################
    // ��ư ���� ���� ����
    public Sprite selectImage;
    public string selectNameText;
    public string selectExplanationText;
    public string buyCurencyName;
    // ������ ����
    public int buyCount;


    public static DrawManager instance;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        buyCurencyName = "Zera";
    }

    // ī�� ���� �� ��ȭ ������Ʈ ���� --------------------------------------------

    // ----------------------------------------------------------------------------
}

