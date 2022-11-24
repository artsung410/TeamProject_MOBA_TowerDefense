using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OpenDrawBox : MonoBehaviour
{
    // ###############################################
    //             NAME : Simstealer                      
    //             MAIL : minsub4400@gmail.com         
    // ###############################################

    // ��ο��ư�� ������ ī�尡 �����µ� �ϴ��� ������ �Ҹ���� �����Ҳ���

    // ��ο� �ڽ� �ݱ�
    [SerializeField]
    private GameObject closeObj;
    // �̱� â ����
    [SerializeField]
    private GameObject openObj_ten;
    [SerializeField]
    private GameObject openObj_one;

    [SerializeField]
    private RandomSelect randomSelect_one;
    [SerializeField]
    private RandomSelect randomSelect_ten;

    private void OnEnable()
    {
        gameObject.GetComponent<Button>().interactable = true;
    }


    // ī�� �̱� �Լ� ���� (��ο� �Ŵ���)
    public void BoxOpen()
    {
        // ������ �Ҹ�
        DrawManager.instance.OpenBoxDisCount();
        // ī�� �̱�
        closeObj.SetActive(false);

        if (DrawManager.instance.boxCount == 10)
        {
            openObj_ten.SetActive(true);
            randomSelect_ten.SkillResultSelect();
        }
        else if (DrawManager.instance.boxCount == 1)
        {
            openObj_one.SetActive(true);
            randomSelect_one.SkillResultSelect();
        }
    }

    private void Update()
    {
        // �ڱ� �ڽ��� Ȱ��ȭ �Ǿ� �ְ�
        if (gameObject.activeSelf == true && DrawManager.instance.boxItem.item.itemValue < 10 && gameObject.GetComponent<Button>().interactable == true && DrawManager.instance.boxCount == 10)
        {
            gameObject.GetComponent<Button>().interactable = false;
        }
        if (gameObject.name == "OneRetry - Button" && DrawManager.instance.boxItem.item.itemValue < 1)
        {
            gameObject.GetComponent<Button>().interactable = false;
        }
    }

    public void RetryButton()
    {
        StartCoroutine(RetryButtonIE());
    }

    public IEnumerator RetryButtonIE()
    {
        // �ڵ� ���� ����
        gameObject.GetComponent<AutoCardOpen>().AutoOpenButton();

        yield return new WaitForSeconds(2f);

        // ������ �Ҹ�
        DrawManager.instance.OpenBoxDisCount();
        // ī�� �̱� â �ʱ�ȭ
        DrawManager.instance.CardDataInit();
        // ī�� �̱� ����
        if (DrawManager.instance.boxCount == 10)
        {
            randomSelect_ten.SkillResultSelect();
        }
        else if (DrawManager.instance.boxCount == 1)
        {
            randomSelect_one.SkillResultSelect();
        }
    }
}
