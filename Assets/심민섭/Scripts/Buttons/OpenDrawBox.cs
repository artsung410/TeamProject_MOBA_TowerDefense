using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    public void RetryButton()
    {
        // ������ �Ҹ�
        DrawManager.instance.OpenBoxDisCount();
        // ī�� �̱� â �ʱ�ȭ
        DrawManager.instance.CardDataInit();
        // ī�� �̱� ����
        randomSelect.SkillResultSelect();
    }
}
