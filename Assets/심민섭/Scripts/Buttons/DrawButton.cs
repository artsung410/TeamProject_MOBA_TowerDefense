using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DrawButton : MonoBehaviour
{
    // ###############################################
    //             NAME : Simstealer                      
    //             MAIL : minsub4400@gmail.com         
    // ###############################################


    // 뽑기 버튼을 누르면 뽑기 창이 팝업된다.

    private GameObject drawPanel;
    private GameObject drawCardAndTowerSelect;

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
        // 뽑기 메인 패널 가져오기
        drawPanel = GameObject.FindGameObjectWithTag("Canvas").transform.GetChild(4).GetChild(0).gameObject;

        // 뽑기 카드/타워 선택 화면
        drawCardAndTowerSelect = GameObject.FindGameObjectWithTag("DrawInventory").transform.GetChild(0).GetChild(0).gameObject;

        // 카드 노말 창
        NomalObjUI = GameObject.FindGameObjectWithTag("DrawInventory").transform.GetChild(0).GetChild(1).GetChild(0).gameObject;

        // 카드 프리미엄 창
        PremiumObjUI = GameObject.FindGameObjectWithTag("DrawInventory").transform.GetChild(0).GetChild(1).GetChild(1).gameObject;

        // 타워 노말 창
        TowerNomalObjUI = GameObject.FindGameObjectWithTag("DrawInventory").transform.GetChild(0).GetChild(2).GetChild(0).gameObject;
        // 타워 프리미엄 창
        TowerPremiumObjUI = GameObject.FindGameObjectWithTag("DrawInventory").transform.GetChild(0).GetChild(2).GetChild(1).gameObject;

        skillCardDrawTab = GameObject.FindGameObjectWithTag("DrawInventory").transform.GetChild(0).GetChild(1).gameObject;
        towerCardDrawTab = GameObject.FindGameObjectWithTag("DrawInventory").transform.GetChild(0).GetChild(2).gameObject;

        // 구매 창
        buyerCardWindow = GameObject.FindGameObjectWithTag("DrawInventory").transform.GetChild(1).gameObject;
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
        // 버튼이 눌리면 첫 화면 비활성화
        drawCardAndTowerSelect.SetActive(false);
    }

    // 타워 카드 창 오픈
    public void TowerCardTabOpen()
    {
        towerCardDrawTab.SetActive(true);
        // 버튼이 눌리면 첫 화면 비활성화
        drawCardAndTowerSelect.SetActive(false);
    }

    // 이미지 데이터
    [SerializeField]
    private Sprite originalImageName;
    [SerializeField]
    private Sprite changeImageName;

    // 노말 카드 뽑기창 선택
    public void SelectNomalPanel()
    {
        // 버튼이 눌리면 SelectUI(Nomal) - Image, 하위 오브젝트 활성화
        gameObject.transform.GetChild(0).gameObject.SetActive(true);
        // 프리미엄 창은 비활성화
        PremiumObjUI.transform.GetChild(0).GetChild(0).gameObject.SetActive(false);
        gameObject.GetComponent<Image>().sprite = changeImageName;
        gameObject.transform.GetChild(1).GetComponent<Text>().color = Color.white;
        PremiumObjUI.transform.GetChild(0).GetComponent<Image>().sprite = originalImageName;
        PremiumObjUI.transform.GetChild(0).GetChild(1).GetComponent<Text>().color = Color.grey;

    }

    // 프리미엄 카드 뽑기창 선택
    public void SelectPremiunPanel()
    {
        // 버튼이 눌리면 SelectUI(Premium) - Image, 하위 오브젝트 비활성화
        gameObject.transform.GetChild(0).gameObject.SetActive(true);
        // 노말 창은 비활성화
        NomalObjUI.transform.GetChild(0).GetChild(0).gameObject.SetActive(false);
        gameObject.GetComponent<Image>().sprite = changeImageName;
        gameObject.transform.GetChild(1).GetComponent<Text>().color = Color.white;
        NomalObjUI.transform.GetChild(0).GetComponent<Image>().sprite = originalImageName;
        NomalObjUI.transform.GetChild(0).GetChild(1).GetComponent<Text>().color = Color.grey;
    }

    public void SelectNomalTowerPanel()
    {
        // 버튼이 눌리면 SelectUI(Nomal) - Image, 하위 오브젝트 활성화
        gameObject.transform.GetChild(0).gameObject.SetActive(true);
        TowerPremiumObjUI.transform.GetChild(0).GetChild(0).gameObject.SetActive(false);
        gameObject.GetComponent<Image>().sprite = changeImageName;
        gameObject.transform.GetChild(1).GetComponent<Text>().color = Color.white;
        TowerPremiumObjUI.transform.GetChild(0).GetComponent<Image>().sprite = originalImageName;
        TowerPremiumObjUI.transform.GetChild(0).GetChild(1).GetComponent<Text>().color = Color.grey;

    }

    public void SelectPremiumTowerPanel()
    {
        // 버튼이 눌리면 SelectUI(Premium) - Image, 하위 오브젝트 비활성화
        gameObject.transform.GetChild(0).gameObject.SetActive(true);
        TowerNomalObjUI.transform.GetChild(0).GetChild(0).gameObject.SetActive(false);
        TowerNomalObjUI.transform.GetChild(0).GetChild(0).gameObject.SetActive(false);
        gameObject.GetComponent<Image>().sprite = changeImageName;
        gameObject.transform.GetChild(1).GetComponent<Text>().color = Color.white;
        TowerNomalObjUI.transform.GetChild(0).GetComponent<Image>().sprite = originalImageName;
        TowerNomalObjUI.transform.GetChild(0).GetChild(1).GetComponent<Text>().color = Color.grey;
    }

    // 활성화 될 오브젝트(BuyerCardWindow - Obj)
    private GameObject buyerCardWindow;

    // ----------------- 뽑기 버튼(스킬 노말) -------------------
    public void DrawCommonSkillCard_N()
    {
        // 버튼을 누르면 BuyerCardWindow - Obj가 활성화
        buyerCardWindow.SetActive(true);
        // 버튼의 정보 저장
        DrawManager.instance.selectImage = gameObject.transform.GetChild(2).GetComponent<Image>().sprite;
        DrawManager.instance.selectNameText = gameObject.transform.GetChild(0).GetComponent<Text>().text;
        DrawManager.instance.selectExplanationText = gameObject.transform.GetChild(1).GetComponent<Text>().text;
        DrawManager.instance.buyCurencyName = "Zera";
    }
    public void DrawWarriorSkillCard_N()
    {
        // 버튼을 누르면 BuyerCardWindow - Obj가 활성화
        buyerCardWindow.SetActive(true);
        // 버튼의 정보 저장
        DrawManager.instance.selectImage = gameObject.transform.GetChild(2).GetComponent<Image>().sprite;
        DrawManager.instance.selectNameText = gameObject.transform.GetChild(0).GetComponent<Text>().text;
        DrawManager.instance.selectExplanationText = gameObject.transform.GetChild(1).GetComponent<Text>().text;
        DrawManager.instance.buyCurencyName = "Zera";

    }
    public void DrawWizardSkillCard_N()
    {
        // 버튼을 누르면 BuyerCardWindow - Obj가 활성화
        buyerCardWindow.SetActive(true);
        // 버튼의 정보 저장
        DrawManager.instance.selectImage = gameObject.transform.GetChild(2).GetComponent<Image>().sprite;
        DrawManager.instance.selectNameText = gameObject.transform.GetChild(0).GetComponent<Text>().text;
        DrawManager.instance.selectExplanationText = gameObject.transform.GetChild(1).GetComponent<Text>().text;
        DrawManager.instance.buyCurencyName = "Zera";
    }
    public void DrawAssassinSkillCard_N()
    {
        // 버튼을 누르면 BuyerCardWindow - Obj가 활성화
        buyerCardWindow.SetActive(true);
        // 버튼의 정보 저장
        DrawManager.instance.selectImage = gameObject.transform.GetChild(2).GetComponent<Image>().sprite;
        DrawManager.instance.selectNameText = gameObject.transform.GetChild(0).GetComponent<Text>().text;
        DrawManager.instance.selectExplanationText = gameObject.transform.GetChild(1).GetComponent<Text>().text;
        DrawManager.instance.buyCurencyName = "Zera";
    }

    // ----------------- 뽑기 버튼(스킬 프리미엄) -------------------
    public void DrawCommonSkillCard_P()
    {
        // 버튼을 누르면 BuyerCardWindow - Obj가 활성화
        buyerCardWindow.SetActive(true);
        // 버튼의 정보 저장
        DrawManager.instance.selectImage = gameObject.transform.GetChild(2).GetComponent<Image>().sprite;
        DrawManager.instance.selectNameText = gameObject.transform.GetChild(0).GetComponent<Text>().text;
        DrawManager.instance.selectExplanationText = gameObject.transform.GetChild(1).GetComponent<Text>().text;
        DrawManager.instance.buyCurencyName = "Dappx";
    }
    public void DrawWarriorSkillCard_P()
    {
        // 버튼을 누르면 BuyerCardWindow - Obj가 활성화
        buyerCardWindow.SetActive(true);
        // 버튼의 정보 저장
        DrawManager.instance.selectImage = gameObject.transform.GetChild(2).GetComponent<Image>().sprite;
        DrawManager.instance.selectNameText = gameObject.transform.GetChild(0).GetComponent<Text>().text;
        DrawManager.instance.selectExplanationText = gameObject.transform.GetChild(1).GetComponent<Text>().text;
        DrawManager.instance.buyCurencyName = "Dappx";

    }
    public void DrawWizardSkillCard_P()
    {
        // 버튼을 누르면 BuyerCardWindow - Obj가 활성화
        buyerCardWindow.SetActive(true);
        // 버튼의 정보 저장
        DrawManager.instance.selectImage = gameObject.transform.GetChild(2).GetComponent<Image>().sprite;
        DrawManager.instance.selectNameText = gameObject.transform.GetChild(0).GetComponent<Text>().text;
        DrawManager.instance.selectExplanationText = gameObject.transform.GetChild(1).GetComponent<Text>().text;
        DrawManager.instance.buyCurencyName = "Dappx";
    }
    public void DrawAssassinSkillCard_P()
    {
        // 버튼을 누르면 BuyerCardWindow - Obj가 활성화
        buyerCardWindow.SetActive(true);
        // 버튼의 정보 저장
        DrawManager.instance.selectImage = gameObject.transform.GetChild(2).GetComponent<Image>().sprite;
        DrawManager.instance.selectNameText = gameObject.transform.GetChild(0).GetComponent<Text>().text;
        DrawManager.instance.selectExplanationText = gameObject.transform.GetChild(1).GetComponent<Text>().text;
        DrawManager.instance.buyCurencyName = "Dappx";
    }

    // ----------------- 뽑기 버튼(타워 노말) -------------------
    public void DrawRandomTowerCard_N()
    {
        // 버튼을 누르면 BuyerCardWindow - Obj가 활성화
        buyerCardWindow.SetActive(true);
        // 버튼의 정보 저장
        DrawManager.instance.selectImage = gameObject.transform.GetChild(2).GetComponent<Image>().sprite;
        DrawManager.instance.selectNameText = gameObject.transform.GetChild(0).GetComponent<Text>().text;
        DrawManager.instance.selectExplanationText = gameObject.transform.GetChild(1).GetComponent<Text>().text;
        DrawManager.instance.buyCurencyName = "Zera";
    }
    public void DrawAttackTowerCard_N()
    {
        // 버튼을 누르면 BuyerCardWindow - Obj가 활성화
        buyerCardWindow.SetActive(true);
        // 버튼의 정보 저장
        DrawManager.instance.selectImage = gameObject.transform.GetChild(2).GetComponent<Image>().sprite;
        DrawManager.instance.selectNameText = gameObject.transform.GetChild(0).GetComponent<Text>().text;
        DrawManager.instance.selectExplanationText = gameObject.transform.GetChild(1).GetComponent<Text>().text;
        DrawManager.instance.buyCurencyName = "Zera";

    }
    public void DrawMinionTowerlCard_N()
    {
        // 버튼을 누르면 BuyerCardWindow - Obj가 활성화
        buyerCardWindow.SetActive(true);
        // 버튼의 정보 저장
        DrawManager.instance.selectImage = gameObject.transform.GetChild(2).GetComponent<Image>().sprite;
        DrawManager.instance.selectNameText = gameObject.transform.GetChild(0).GetComponent<Text>().text;
        DrawManager.instance.selectExplanationText = gameObject.transform.GetChild(1).GetComponent<Text>().text;
        DrawManager.instance.buyCurencyName = "Zera";
    }
    public void DrawBuffTowerCard_N()
    {
        // 버튼을 누르면 BuyerCardWindow - Obj가 활성화
        buyerCardWindow.SetActive(true);
        // 버튼의 정보 저장
        DrawManager.instance.selectImage = gameObject.transform.GetChild(2).GetComponent<Image>().sprite;
        DrawManager.instance.selectNameText = gameObject.transform.GetChild(0).GetComponent<Text>().text;
        DrawManager.instance.selectExplanationText = gameObject.transform.GetChild(1).GetComponent<Text>().text;
        DrawManager.instance.buyCurencyName = "Zera";
    }

    // ----------------- 뽑기 버튼(타워 프리미엄) -------------------
    public void DrawRandomTowerCard_P()
    {
        // 버튼을 누르면 BuyerCardWindow - Obj가 활성화
        buyerCardWindow.SetActive(true);
        // 버튼의 정보 저장
        DrawManager.instance.selectImage = gameObject.transform.GetChild(2).GetComponent<Image>().sprite;
        DrawManager.instance.selectNameText = gameObject.transform.GetChild(0).GetComponent<Text>().text;
        DrawManager.instance.selectExplanationText = gameObject.transform.GetChild(1).GetComponent<Text>().text;
        DrawManager.instance.buyCurencyName = "Dappx";
    }
    public void DrawAttackTowerCard_P()
    {
        // 버튼을 누르면 BuyerCardWindow - Obj가 활성화
        buyerCardWindow.SetActive(true);
        // 버튼의 정보 저장
        DrawManager.instance.selectImage = gameObject.transform.GetChild(2).GetComponent<Image>().sprite;
        DrawManager.instance.selectNameText = gameObject.transform.GetChild(0).GetComponent<Text>().text;
        DrawManager.instance.selectExplanationText = gameObject.transform.GetChild(1).GetComponent<Text>().text;
        DrawManager.instance.buyCurencyName = "Dappx";

    }
    public void DrawMinionTowerlCard_P()
    {
        // 버튼을 누르면 BuyerCardWindow - Obj가 활성화
        buyerCardWindow.SetActive(true);
        // 버튼의 정보 저장
        DrawManager.instance.selectImage = gameObject.transform.GetChild(2).GetComponent<Image>().sprite;
        DrawManager.instance.selectNameText = gameObject.transform.GetChild(0).GetComponent<Text>().text;
        DrawManager.instance.selectExplanationText = gameObject.transform.GetChild(1).GetComponent<Text>().text;
        DrawManager.instance.buyCurencyName = "Dappx";
    }
    public void DrawBuffTowerCard_P()
    {
        // 버튼을 누르면 BuyerCardWindow - Obj가 활성화
        buyerCardWindow.SetActive(true);
        // 버튼의 정보 저장
        DrawManager.instance.selectImage = gameObject.transform.GetChild(2).GetComponent<Image>().sprite;
        DrawManager.instance.selectNameText = gameObject.transform.GetChild(0).GetComponent<Text>().text;
        DrawManager.instance.selectExplanationText = gameObject.transform.GetChild(1).GetComponent<Text>().text;
        DrawManager.instance.buyCurencyName = "Dappx";
    }

}
