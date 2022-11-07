using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrawButton : MonoBehaviour
{
    // ###############################################
    //             NAME : Simstealer                      
    //             MAIL : minsub4400@gmail.com         
    // ###############################################


    // �̱� ��ư�� ������ �̱� â�� �˾��ȴ�.

    private GameObject drawPanel;

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
        // �̱� â ��������
        drawPanel = GameObject.FindGameObjectWithTag("Canvas").transform.GetChild(4).GetChild(0).gameObject;
        // ī�� �븻 â
        NomalObjUI = GameObject.FindGameObjectWithTag("DrawInventory").transform.GetChild(0).GetChild(1).GetChild(0).gameObject;
        NomalObjs = GameObject.FindGameObjectWithTag("DrawInventory").transform.GetChild(0).GetChild(1).GetChild(2).GetChild(0).GetChild(0).gameObject;
        // ī�� �����̾� â
        PremiumObjUI = GameObject.FindGameObjectWithTag("DrawInventory").transform.GetChild(0).GetChild(1).GetChild(1).gameObject;
        PremiumObjs = GameObject.FindGameObjectWithTag("DrawInventory").transform.GetChild(0).GetChild(1).GetChild(3).GetChild(0).GetChild(0).gameObject;

        // Ÿ�� �븻 â
        TowerNomalObjUI = GameObject.FindGameObjectWithTag("DrawInventory").transform.GetChild(0).GetChild(2).GetChild(0).gameObject;
        TowerNomalObjs = GameObject.FindGameObjectWithTag("DrawInventory").transform.GetChild(0).GetChild(2).GetChild(2).GetChild(0).GetChild(0).gameObject;
        // Ÿ�� �����̾� â
        TowerPremiumObjUI = GameObject.FindGameObjectWithTag("DrawInventory").transform.GetChild(0).GetChild(2).GetChild(1).gameObject;
        TowerPremiumObjs = GameObject.FindGameObjectWithTag("DrawInventory").transform.GetChild(0).GetChild(2).GetChild(3).GetChild(0).GetChild(0).gameObject;

        skillCardDrawTab = GameObject.FindGameObjectWithTag("DrawInventory").transform.GetChild(0).GetChild(1).gameObject;
        towerCardDrawTab = GameObject.FindGameObjectWithTag("DrawInventory").transform.GetChild(0).GetChild(2).gameObject;
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
    }

    // Ÿ�� ī�� â ����
    public void TowerCardTabOpen()
    {
        towerCardDrawTab.SetActive(true);
    }

    // �븻 ī�� �̱�â ����
    public void SelectNomalPanel()
    {
        // ��ư�� ������ SelectUI(Nomal) - Image, ���� ������Ʈ Ȱ��ȭ
        gameObject.transform.parent.parent.GetChild(0).gameObject.SetActive(true);
        gameObject.transform.GetChild(0).gameObject.SetActive(true);
        // ��ư�� ������ �����̾� ������Ʈ ��Ȱ��ȭ
        PremiumObjUI.SetActive(false);
        PremiumObjs.SetActive(false);
    }

    // �����̾� ī�� �̱�â ����
    public void SelectPremiunPanel()
    {
        // ��ư�� ������ SelectUI(Premium) - Image, ���� ������Ʈ ��Ȱ��ȭ
        gameObject.transform.parent.parent.GetChild(1).gameObject.SetActive(true);
        gameObject.transform.GetChild(0).gameObject.SetActive(true);
        // ��ư�� ������ �븻 ������Ʈ ��Ȱ��ȭ
        NomalObjUI.SetActive(false);
        NomalObjs.SetActive(false);
    }

    public void SelectNomalTowerPanel()
    {
        // ��ư�� ������ SelectUI(Nomal) - Image, ���� ������Ʈ Ȱ��ȭ
        gameObject.transform.parent.parent.GetChild(0).gameObject.SetActive(true);
        gameObject.transform.GetChild(0).gameObject.SetActive(true);
        // ��ư�� ������ �����̾� ������Ʈ ��Ȱ��ȭ
        TowerPremiumObjUI.SetActive(false);
        TowerPremiumObjs.SetActive(false);
    }

    public void SelectPremiumTowerPanel()
    {
        // ��ư�� ������ SelectUI(Premium) - Image, ���� ������Ʈ ��Ȱ��ȭ
        gameObject.transform.parent.parent.GetChild(1).gameObject.SetActive(true);
        gameObject.transform.GetChild(0).gameObject.SetActive(true);
        // ��ư�� ������ �븻 ������Ʈ ��Ȱ��ȭ
        TowerNomalObjUI.SetActive(false);
        TowerNomalObjs.SetActive(false);
    }

}
