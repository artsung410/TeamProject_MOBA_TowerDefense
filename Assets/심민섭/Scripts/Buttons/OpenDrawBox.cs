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
    private GameObject openObj;

    [SerializeField]
    private RandomSelect randomSelect;

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
        randomSelect.SkillResultSelect();
        closeObj.SetActive(false);
        openObj.SetActive(true);
    }

    private void Update()
    {
        // �ڱ� �ڽ��� Ȱ��ȭ �Ǿ� �ְ�
        if (gameObject.activeSelf == true && DrawManager.instance.boxItem.item.itemValue < 10 && gameObject.GetComponent<Button>().interactable == true)
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
        randomSelect.SkillResultSelect();
    }
}
