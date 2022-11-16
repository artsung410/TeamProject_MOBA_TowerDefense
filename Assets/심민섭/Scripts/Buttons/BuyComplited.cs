using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BuyComplited : MonoBehaviour
{
    // ###############################################
    //             NAME : Simstealer                      
    //             MAIL : minsub4400@gmail.com         
    // ###############################################

    [SerializeField]
    private Image buyItemImage;
    [SerializeField]
    private Text buyItemName;
    [SerializeField]
    private Text buyCount;


    // ��� ���� �̹���
    [SerializeField]
    private Sprite commonSkillCardImage;
    [SerializeField]
    private Sprite warriorSkillCardImage;
    [SerializeField]
    private Sprite wizardSkillCardImage;
    [SerializeField]
    private Sprite assessinSkillCardImage;
    [SerializeField]
    private Sprite randomTowerCardImage;
    [SerializeField]
    private Sprite attackTowerCardImage;
    [SerializeField]
    private Sprite minionTowerCardImage;
    [SerializeField]
    private Sprite buffTowerCardImage;

    private void OnEnable()
    {
        StartCoroutine(BuyerInfoUpdate());
    }

    IEnumerator BuyerInfoUpdate()
    {
        yield return new WaitForSeconds(0.01f);
        // Ȱ��ȭ �Ǹ� ��ư ������ ������Ʈ
        //buyItemImage.sprite = DrawManager.instance.selectImage;
        buyItemName.text = DrawManager.instance.selectNameText;
        // ���ǹ��� �ξ �ؽ�Ʈ�� �´� �̹����� �߰� ����

        if (buyItemName.text == "Common Skill")
        {
            buyItemImage.sprite = commonSkillCardImage;
        }
        else if (buyItemName.text == "Warrior Skill")
        {
            buyItemImage.sprite = warriorSkillCardImage;
        }
        else if (buyItemName.text == "Wizard Skill")
        {
            buyItemImage.sprite = wizardSkillCardImage;
        }
        else if (buyItemName.text == "Assassin Skill")
        {
            buyItemImage.sprite = assessinSkillCardImage;
        }
        else if (buyItemName.text == "Random Tower")
        {
            buyItemImage.sprite = randomTowerCardImage;
        }
        else if (buyItemName.text == "Attack Tower")
        {
            buyItemImage.sprite = attackTowerCardImage;
        }
        else if (buyItemName.text == "Minion Tower")
        {
            buyItemImage.sprite = minionTowerCardImage;
        }
        else if (buyItemName.text == "Buff Tower")
        {
            buyItemImage.sprite = buffTowerCardImage;
        }
        else
        {
            Debug.LogError("�ش� ī�尡 ���� ���� �ʽ��ϴ�.");
        }

        buyCount.text = "X" + DrawManager.instance.buyCount.ToString();
    }
    
}
