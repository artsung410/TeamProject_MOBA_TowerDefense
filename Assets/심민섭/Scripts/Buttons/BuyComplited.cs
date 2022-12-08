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


    // 출력 해줄 이미지
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
        // 활성화 되면 버튼 정보를 업데이트
        //buyItemImage.sprite = DrawManager.instance.selectImage;
        buyItemName.text = DrawManager.instance.selectNameText;
        // 조건문을 두어서 텍스트에 맞는 이미지가 뜨게 변경

        if (buyItemName.text == "Common Skill" || buyItemName.text == "Common Skill_P")
        {
            buyItemImage.sprite = commonSkillCardImage;
        }
        else if (buyItemName.text == "Warrior Skill" || buyItemName.text == "Warrior Skill_P")
        {
            buyItemImage.sprite = warriorSkillCardImage;
        }
        else if (buyItemName.text == "Wizard Skill" || buyItemName.text == "Wizard Skill_P")
        {
            buyItemImage.sprite = wizardSkillCardImage;
        }
        else if (buyItemName.text == "Assassin Skill" || buyItemName.text == "Assassin Skill_P")
        {
            buyItemImage.sprite = assessinSkillCardImage;
        }
        else if (buyItemName.text == "Random Tower" || buyItemName.text == "Random Tower_P")
        {
            buyItemImage.sprite = randomTowerCardImage;
        }
        else if (buyItemName.text == "Attack Tower" || buyItemName.text == "Attack Tower_P")
        {
            buyItemImage.sprite = attackTowerCardImage;
        }
        else if (buyItemName.text == "Minion Tower" || buyItemName.text == "Minion Tower_P")
        {
            buyItemImage.sprite = minionTowerCardImage;
        }
        else if (buyItemName.text == "Buff & Debuff Tower" || buyItemName.text == "Buff & Debuff Tower_P")
        {
            buyItemImage.sprite = buffTowerCardImage;
        }
        else
        {
            Debug.LogError("해당 카드가 존재 하지 않습니다.");
        }

        buyCount.text = "X" + DrawManager.instance.buyCount.ToString();
    }
    
}
