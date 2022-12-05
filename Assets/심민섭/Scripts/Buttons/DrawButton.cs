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


    // �̱� ��ư�� ������ �̱� â�� �˾��ȴ�.

    private GameObject drawPanel;
    private GameObject drawCardAndTowerSelect;

    // ī�� �븻
    private GameObject NomalObjUI;
    private GameObject NomalObjs;
    // ī�� �����̾�
    private GameObject PremiumObjUI;
    private GameObject PremiumObjs;

    private GameObject skillCardDrawTab;
    private GameObject towerCardDrawTab;

    // Ÿ�� �븻
    private GameObject TowerNomalObjUI;
    private GameObject TowerNomalObjs;
    // Ÿ�� �����̾�
    private GameObject TowerPremiumObjUI;
    private GameObject TowerPremiumObjs;

    private void Start()
    {
        // �̱� ���� �г� ��������
        drawPanel = GameObject.FindGameObjectWithTag("Canvas").transform.GetChild(4).GetChild(0).gameObject;

        // �̱� ī��/Ÿ�� ���� ȭ��
        drawCardAndTowerSelect = GameObject.FindGameObjectWithTag("DrawInventory").transform.GetChild(0).GetChild(0).gameObject;

        // ī�� �븻 â
        NomalObjUI = GameObject.FindGameObjectWithTag("DrawInventory").transform.GetChild(0).GetChild(1).GetChild(0).gameObject;

        // ī�� �����̾� â
        PremiumObjUI = GameObject.FindGameObjectWithTag("DrawInventory").transform.GetChild(0).GetChild(1).GetChild(1).gameObject;

        // Ÿ�� �븻 â
        TowerNomalObjUI = GameObject.FindGameObjectWithTag("DrawInventory").transform.GetChild(0).GetChild(2).GetChild(0).gameObject;
        // Ÿ�� �����̾� â
        TowerPremiumObjUI = GameObject.FindGameObjectWithTag("DrawInventory").transform.GetChild(0).GetChild(2).GetChild(1).gameObject;

        skillCardDrawTab = GameObject.FindGameObjectWithTag("DrawInventory").transform.GetChild(0).GetChild(1).gameObject;
        towerCardDrawTab = GameObject.FindGameObjectWithTag("DrawInventory").transform.GetChild(0).GetChild(2).gameObject;

        // ���� â
        buyerCardWindow = GameObject.FindGameObjectWithTag("DrawInventory").transform.GetChild(1).gameObject;
    }
    
    // �̱�â ����
    public void OpenDraw()
    {
        drawPanel.SetActive(true);
    }


    // ��ų ī�� â ����
    public void SkillCardTabOpen()
    {
        skillCardDrawTab.SetActive(true);
        // ��ư�� ������ ù ȭ�� ��Ȱ��ȭ
        drawCardAndTowerSelect.SetActive(false);
    }

    // Ÿ�� ī�� â ����
    public void TowerCardTabOpen()
    {
        towerCardDrawTab.SetActive(true);
        // ��ư�� ������ ù ȭ�� ��Ȱ��ȭ
        drawCardAndTowerSelect.SetActive(false);
    }

    // �̹��� ������
    [SerializeField]
    private Sprite originalImageName;
    [SerializeField]
    private Sprite changeImageName;

    // �븻 ī�� �̱�â ����
    public void SelectNomalPanel()
    {
        // ��ư�� ������ SelectUI(Nomal) - Image, ���� ������Ʈ Ȱ��ȭ
        gameObject.transform.GetChild(0).gameObject.SetActive(true);
        // �����̾� â�� ��Ȱ��ȭ
        PremiumObjUI.transform.GetChild(0).GetChild(0).gameObject.SetActive(false);
        gameObject.GetComponent<Image>().sprite = changeImageName;
        gameObject.transform.GetChild(1).GetComponent<Text>().color = Color.white;
        PremiumObjUI.transform.GetChild(0).GetComponent<Image>().sprite = originalImageName;
        PremiumObjUI.transform.GetChild(0).GetChild(1).GetComponent<Text>().color = Color.grey;

    }

    // �����̾� ī�� �̱�â ����
    public void SelectPremiunPanel()
    {
        // ��ư�� ������ SelectUI(Premium) - Image, ���� ������Ʈ ��Ȱ��ȭ
        gameObject.transform.GetChild(0).gameObject.SetActive(true);
        // �븻 â�� ��Ȱ��ȭ
        NomalObjUI.transform.GetChild(0).GetChild(0).gameObject.SetActive(false);
        gameObject.GetComponent<Image>().sprite = changeImageName;
        gameObject.transform.GetChild(1).GetComponent<Text>().color = Color.white;
        NomalObjUI.transform.GetChild(0).GetComponent<Image>().sprite = originalImageName;
        NomalObjUI.transform.GetChild(0).GetChild(1).GetComponent<Text>().color = Color.grey;
    }

    public void SelectNomalTowerPanel()
    {
        // ��ư�� ������ SelectUI(Nomal) - Image, ���� ������Ʈ Ȱ��ȭ
        gameObject.transform.GetChild(0).gameObject.SetActive(true);
        TowerPremiumObjUI.transform.GetChild(0).GetChild(0).gameObject.SetActive(false);
        gameObject.GetComponent<Image>().sprite = changeImageName;
        gameObject.transform.GetChild(1).GetComponent<Text>().color = Color.white;
        TowerPremiumObjUI.transform.GetChild(0).GetComponent<Image>().sprite = originalImageName;
        TowerPremiumObjUI.transform.GetChild(0).GetChild(1).GetComponent<Text>().color = Color.grey;

    }

    public void SelectPremiumTowerPanel()
    {
        // ��ư�� ������ SelectUI(Premium) - Image, ���� ������Ʈ ��Ȱ��ȭ
        gameObject.transform.GetChild(0).gameObject.SetActive(true);
        TowerNomalObjUI.transform.GetChild(0).GetChild(0).gameObject.SetActive(false);
        TowerNomalObjUI.transform.GetChild(0).GetChild(0).gameObject.SetActive(false);
        gameObject.GetComponent<Image>().sprite = changeImageName;
        gameObject.transform.GetChild(1).GetComponent<Text>().color = Color.white;
        TowerNomalObjUI.transform.GetChild(0).GetComponent<Image>().sprite = originalImageName;
        TowerNomalObjUI.transform.GetChild(0).GetChild(1).GetComponent<Text>().color = Color.grey;
    }

    // Ȱ��ȭ �� ������Ʈ(BuyerCardWindow - Obj)
    private GameObject buyerCardWindow;

    // ----------------- �̱� ��ư(��ų �븻) -------------------
    public void DrawCommonSkillCard_N()
    {
        // ��ư�� ������ BuyerCardWindow - Obj�� Ȱ��ȭ
        buyerCardWindow.SetActive(true);
        // ��ư�� ���� ����
        DrawManager.instance.selectImage = gameObject.transform.GetChild(2).GetComponent<Image>().sprite;
        DrawManager.instance.selectNameText = gameObject.transform.GetChild(0).GetComponent<Text>().text;
        DrawManager.instance.selectExplanationText = gameObject.transform.GetChild(1).GetComponent<Text>().text;
        DrawManager.instance.buyCurencyName = "Zera";
    }
    public void DrawWarriorSkillCard_N()
    {
        // ��ư�� ������ BuyerCardWindow - Obj�� Ȱ��ȭ
        buyerCardWindow.SetActive(true);
        // ��ư�� ���� ����
        DrawManager.instance.selectImage = gameObject.transform.GetChild(2).GetComponent<Image>().sprite;
        DrawManager.instance.selectNameText = gameObject.transform.GetChild(0).GetComponent<Text>().text;
        DrawManager.instance.selectExplanationText = gameObject.transform.GetChild(1).GetComponent<Text>().text;
        DrawManager.instance.buyCurencyName = "Zera";

    }
    public void DrawWizardSkillCard_N()
    {
        // ��ư�� ������ BuyerCardWindow - Obj�� Ȱ��ȭ
        buyerCardWindow.SetActive(true);
        // ��ư�� ���� ����
        DrawManager.instance.selectImage = gameObject.transform.GetChild(2).GetComponent<Image>().sprite;
        DrawManager.instance.selectNameText = gameObject.transform.GetChild(0).GetComponent<Text>().text;
        DrawManager.instance.selectExplanationText = gameObject.transform.GetChild(1).GetComponent<Text>().text;
        DrawManager.instance.buyCurencyName = "Zera";
    }
    public void DrawAssassinSkillCard_N()
    {
        // ��ư�� ������ BuyerCardWindow - Obj�� Ȱ��ȭ
        buyerCardWindow.SetActive(true);
        // ��ư�� ���� ����
        DrawManager.instance.selectImage = gameObject.transform.GetChild(2).GetComponent<Image>().sprite;
        DrawManager.instance.selectNameText = gameObject.transform.GetChild(0).GetComponent<Text>().text;
        DrawManager.instance.selectExplanationText = gameObject.transform.GetChild(1).GetComponent<Text>().text;
        DrawManager.instance.buyCurencyName = "Zera";
    }

    // ----------------- �̱� ��ư(��ų �����̾�) -------------------
    public void DrawCommonSkillCard_P()
    {
        // ��ư�� ������ BuyerCardWindow - Obj�� Ȱ��ȭ
        buyerCardWindow.SetActive(true);
        // ��ư�� ���� ����
        DrawManager.instance.selectImage = gameObject.transform.GetChild(2).GetComponent<Image>().sprite;
        DrawManager.instance.selectNameText = gameObject.transform.GetChild(0).GetComponent<Text>().text;
        DrawManager.instance.selectExplanationText = gameObject.transform.GetChild(1).GetComponent<Text>().text;
        DrawManager.instance.buyCurencyName = "Dappx";
    }
    public void DrawWarriorSkillCard_P()
    {
        // ��ư�� ������ BuyerCardWindow - Obj�� Ȱ��ȭ
        buyerCardWindow.SetActive(true);
        // ��ư�� ���� ����
        DrawManager.instance.selectImage = gameObject.transform.GetChild(2).GetComponent<Image>().sprite;
        DrawManager.instance.selectNameText = gameObject.transform.GetChild(0).GetComponent<Text>().text;
        DrawManager.instance.selectExplanationText = gameObject.transform.GetChild(1).GetComponent<Text>().text;
        DrawManager.instance.buyCurencyName = "Dappx";

    }
    public void DrawWizardSkillCard_P()
    {
        // ��ư�� ������ BuyerCardWindow - Obj�� Ȱ��ȭ
        buyerCardWindow.SetActive(true);
        // ��ư�� ���� ����
        DrawManager.instance.selectImage = gameObject.transform.GetChild(2).GetComponent<Image>().sprite;
        DrawManager.instance.selectNameText = gameObject.transform.GetChild(0).GetComponent<Text>().text;
        DrawManager.instance.selectExplanationText = gameObject.transform.GetChild(1).GetComponent<Text>().text;
        DrawManager.instance.buyCurencyName = "Dappx";
    }
    public void DrawAssassinSkillCard_P()
    {
        // ��ư�� ������ BuyerCardWindow - Obj�� Ȱ��ȭ
        buyerCardWindow.SetActive(true);
        // ��ư�� ���� ����
        DrawManager.instance.selectImage = gameObject.transform.GetChild(2).GetComponent<Image>().sprite;
        DrawManager.instance.selectNameText = gameObject.transform.GetChild(0).GetComponent<Text>().text;
        DrawManager.instance.selectExplanationText = gameObject.transform.GetChild(1).GetComponent<Text>().text;
        DrawManager.instance.buyCurencyName = "Dappx";
    }

    // ----------------- �̱� ��ư(Ÿ�� �븻) -------------------
    public void DrawRandomTowerCard_N()
    {
        // ��ư�� ������ BuyerCardWindow - Obj�� Ȱ��ȭ
        buyerCardWindow.SetActive(true);
        // ��ư�� ���� ����
        DrawManager.instance.selectImage = gameObject.transform.GetChild(2).GetComponent<Image>().sprite;
        DrawManager.instance.selectNameText = gameObject.transform.GetChild(0).GetComponent<Text>().text;
        DrawManager.instance.selectExplanationText = gameObject.transform.GetChild(1).GetComponent<Text>().text;
        DrawManager.instance.buyCurencyName = "Zera";
    }
    public void DrawAttackTowerCard_N()
    {
        // ��ư�� ������ BuyerCardWindow - Obj�� Ȱ��ȭ
        buyerCardWindow.SetActive(true);
        // ��ư�� ���� ����
        DrawManager.instance.selectImage = gameObject.transform.GetChild(2).GetComponent<Image>().sprite;
        DrawManager.instance.selectNameText = gameObject.transform.GetChild(0).GetComponent<Text>().text;
        DrawManager.instance.selectExplanationText = gameObject.transform.GetChild(1).GetComponent<Text>().text;
        DrawManager.instance.buyCurencyName = "Zera";

    }
    public void DrawMinionTowerlCard_N()
    {
        // ��ư�� ������ BuyerCardWindow - Obj�� Ȱ��ȭ
        buyerCardWindow.SetActive(true);
        // ��ư�� ���� ����
        DrawManager.instance.selectImage = gameObject.transform.GetChild(2).GetComponent<Image>().sprite;
        DrawManager.instance.selectNameText = gameObject.transform.GetChild(0).GetComponent<Text>().text;
        DrawManager.instance.selectExplanationText = gameObject.transform.GetChild(1).GetComponent<Text>().text;
        DrawManager.instance.buyCurencyName = "Zera";
    }
    public void DrawBuffTowerCard_N()
    {
        // ��ư�� ������ BuyerCardWindow - Obj�� Ȱ��ȭ
        buyerCardWindow.SetActive(true);
        // ��ư�� ���� ����
        DrawManager.instance.selectImage = gameObject.transform.GetChild(2).GetComponent<Image>().sprite;
        DrawManager.instance.selectNameText = gameObject.transform.GetChild(0).GetComponent<Text>().text;
        DrawManager.instance.selectExplanationText = gameObject.transform.GetChild(1).GetComponent<Text>().text;
        DrawManager.instance.buyCurencyName = "Zera";
    }

    // ----------------- �̱� ��ư(Ÿ�� �����̾�) -------------------
    public void DrawRandomTowerCard_P()
    {
        // ��ư�� ������ BuyerCardWindow - Obj�� Ȱ��ȭ
        buyerCardWindow.SetActive(true);
        // ��ư�� ���� ����
        DrawManager.instance.selectImage = gameObject.transform.GetChild(2).GetComponent<Image>().sprite;
        DrawManager.instance.selectNameText = gameObject.transform.GetChild(0).GetComponent<Text>().text;
        DrawManager.instance.selectExplanationText = gameObject.transform.GetChild(1).GetComponent<Text>().text;
        DrawManager.instance.buyCurencyName = "Dappx";
    }
    public void DrawAttackTowerCard_P()
    {
        // ��ư�� ������ BuyerCardWindow - Obj�� Ȱ��ȭ
        buyerCardWindow.SetActive(true);
        // ��ư�� ���� ����
        DrawManager.instance.selectImage = gameObject.transform.GetChild(2).GetComponent<Image>().sprite;
        DrawManager.instance.selectNameText = gameObject.transform.GetChild(0).GetComponent<Text>().text;
        DrawManager.instance.selectExplanationText = gameObject.transform.GetChild(1).GetComponent<Text>().text;
        DrawManager.instance.buyCurencyName = "Dappx";

    }
    public void DrawMinionTowerlCard_P()
    {
        // ��ư�� ������ BuyerCardWindow - Obj�� Ȱ��ȭ
        buyerCardWindow.SetActive(true);
        // ��ư�� ���� ����
        DrawManager.instance.selectImage = gameObject.transform.GetChild(2).GetComponent<Image>().sprite;
        DrawManager.instance.selectNameText = gameObject.transform.GetChild(0).GetComponent<Text>().text;
        DrawManager.instance.selectExplanationText = gameObject.transform.GetChild(1).GetComponent<Text>().text;
        DrawManager.instance.buyCurencyName = "Dappx";
    }
    public void DrawBuffTowerCard_P()
    {
        // ��ư�� ������ BuyerCardWindow - Obj�� Ȱ��ȭ
        buyerCardWindow.SetActive(true);
        // ��ư�� ���� ����
        DrawManager.instance.selectImage = gameObject.transform.GetChild(2).GetComponent<Image>().sprite;
        DrawManager.instance.selectNameText = gameObject.transform.GetChild(0).GetComponent<Text>().text;
        DrawManager.instance.selectExplanationText = gameObject.transform.GetChild(1).GetComponent<Text>().text;
        DrawManager.instance.buyCurencyName = "Dappx";
    }

}
