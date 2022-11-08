using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrawButton : MonoBehaviour
{
    // ###############################################
    //             NAME : Simstealer                      
    //             MAIL : minsub4400@gmail.com         
    // ###############################################


    // 뽑기 버튼을 누르면 뽑기 창이 팝업된다.

    private GameObject drawPanel;

    // 카드 노말
    private GameObject NomalObjUI;
    private GameObject NomalObjs;
    // 카드 프리미엄
    private GameObject PremiumObjUI;
    private GameObject PremiumObjs;

    private GameObject skillCardDrawTab;
    private GameObject towerCardDrawTab;

    // 타워 노말
    private GameObject TowerNomalObjUI;
    private GameObject TowerNomalObjs;
    // 타워 프리미엄
    private GameObject TowerPremiumObjUI;
    private GameObject TowerPremiumObjs;

    private void Start()
    {
        // 뽑기 창 가져오기
        drawPanel = GameObject.FindGameObjectWithTag("Canvas").transform.GetChild(4).GetChild(0).gameObject;
        // 카드 노말 창
        NomalObjUI = GameObject.FindGameObjectWithTag("DrawInventory").transform.GetChild(0).GetChild(1).GetChild(0).gameObject;
        NomalObjs = GameObject.FindGameObjectWithTag("DrawInventory").transform.GetChild(0).GetChild(1).GetChild(2).GetChild(0).GetChild(0).gameObject;
        // 카드 프리미엄 창
        PremiumObjUI = GameObject.FindGameObjectWithTag("DrawInventory").transform.GetChild(0).GetChild(1).GetChild(1).gameObject;
        PremiumObjs = GameObject.FindGameObjectWithTag("DrawInventory").transform.GetChild(0).GetChild(1).GetChild(3).GetChild(0).GetChild(0).gameObject;

        // 타워 노말 창
        TowerNomalObjUI = GameObject.FindGameObjectWithTag("DrawInventory").transform.GetChild(0).GetChild(2).GetChild(0).gameObject;
        TowerNomalObjs = GameObject.FindGameObjectWithTag("DrawInventory").transform.GetChild(0).GetChild(2).GetChild(2).GetChild(0).GetChild(0).gameObject;
        // 타워 프리미엄 창
        TowerPremiumObjUI = GameObject.FindGameObjectWithTag("DrawInventory").transform.GetChild(0).GetChild(2).GetChild(1).gameObject;
        TowerPremiumObjs = GameObject.FindGameObjectWithTag("DrawInventory").transform.GetChild(0).GetChild(2).GetChild(3).GetChild(0).GetChild(0).gameObject;

        skillCardDrawTab = GameObject.FindGameObjectWithTag("DrawInventory").transform.GetChild(0).GetChild(1).gameObject;
        towerCardDrawTab = GameObject.FindGameObjectWithTag("DrawInventory").transform.GetChild(0).GetChild(2).gameObject;
    }
    
    // 뽑기창 열기
    public void OpenDraw()
    {
        drawPanel.SetActive(true);
    }


    // 스킬 카드 창 오픈
    public void SkillCardTabOpen()
    {
        skillCardDrawTab.SetActive(true);
    }

    // 타워 카드 창 오픈
    public void TowerCardTabOpen()
    {
        towerCardDrawTab.SetActive(true);
    }

    // 노말 카드 뽑기창 선택
    public void SelectNomalPanel()
    {
        // 버튼이 눌리면 SelectUI(Nomal) - Image, 하위 오브젝트 활성화
        gameObject.transform.parent.parent.GetChild(0).gameObject.SetActive(true);
        gameObject.transform.GetChild(0).gameObject.SetActive(true);
        // 버튼이 눌리면 프리미엄 오브젝트 비활성화
        PremiumObjUI.SetActive(false);
        PremiumObjs.SetActive(false);
    }

    // 프리미엄 카드 뽑기창 선택
    public void SelectPremiunPanel()
    {
        // 버튼이 눌리면 SelectUI(Premium) - Image, 하위 오브젝트 비활성화
        gameObject.transform.parent.parent.GetChild(1).gameObject.SetActive(true);
        gameObject.transform.GetChild(0).gameObject.SetActive(true);
        // 버튼이 눌리면 노말 오브젝트 비활성화
        NomalObjUI.SetActive(false);
        NomalObjs.SetActive(false);
    }

    public void SelectNomalTowerPanel()
    {
        // 버튼이 눌리면 SelectUI(Nomal) - Image, 하위 오브젝트 활성화
        gameObject.transform.parent.parent.GetChild(0).gameObject.SetActive(true);
        gameObject.transform.GetChild(0).gameObject.SetActive(true);
        // 버튼이 눌리면 프리미엄 오브젝트 비활성화
        TowerPremiumObjUI.SetActive(false);
        TowerPremiumObjs.SetActive(false);
    }

    public void SelectPremiumTowerPanel()
    {
        // 버튼이 눌리면 SelectUI(Premium) - Image, 하위 오브젝트 비활성화
        gameObject.transform.parent.parent.GetChild(1).gameObject.SetActive(true);
        gameObject.transform.GetChild(0).gameObject.SetActive(true);
        // 버튼이 눌리면 노말 오브젝트 비활성화
        TowerNomalObjUI.SetActive(false);
        TowerNomalObjs.SetActive(false);
    }

}
