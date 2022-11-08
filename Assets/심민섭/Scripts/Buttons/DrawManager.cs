using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrawManager : MonoBehaviour
{
    // ###############################################
    //             NAME : Simstealer                      
    //             MAIL : minsub4400@gmail.com         
    // ###############################################
    // 버튼 정보 저장 변수
    public Sprite selectImage;
    public string selectNameText;
    public string selectExplanationText;
    public string buyCurencyName;
    // 구매한 갯수
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

    // 카드 구매 후 재화 업데이트 예정 --------------------------------------------

    // ----------------------------------------------------------------------------
}

