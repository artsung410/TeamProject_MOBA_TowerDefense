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
    [SerializeField]
    private GameObject closeObj;

    // ī�� �̱� �Լ� ���� (��ο� �Ŵ���)
    public void BoxOpen()
    {
        // ī�� �̱�
        DrawManager.instance.RandomCardDraw();
        // ������ �Ҹ�
        DrawManager.instance.OpenBoxDisCount();

        closeObj.SetActive(false);
    }
}
