using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OpenBoxWindow : MonoBehaviour
{
    // ###############################################
    //             NAME : Simstealer                      
    //             MAIL : minsub4400@gmail.com         
    // ###############################################

    private Image drawImage;
    private Text drawText;
    private Text openText;

    // 스킬
    [SerializeField]
    private Sprite commonSkill;
    [SerializeField]
    private Sprite warriorSkill;
    [SerializeField]
    private Sprite wizardSkill;
    [SerializeField]
    private Sprite assassinSkill;
    [SerializeField]
    private Sprite commonSkill_P;
    [SerializeField]
    private Sprite warriorSkill_P;
    [SerializeField]
    private Sprite wizardSkill_P;
    [SerializeField]
    private Sprite assassinSkill_P;

    // 타워
    [SerializeField]
    private Sprite randomTower;
    [SerializeField]
    private Sprite attackTower;
    [SerializeField]
    private Sprite minionTower;
    [SerializeField]
    private Sprite buffTower;
    [SerializeField]
    private Sprite randomTower_P;
    [SerializeField]
    private Sprite attackTower_P;
    [SerializeField]
    private Sprite minionTower_P;
    [SerializeField]
    private Sprite buffTower_P;

    private void Awake()
    {
        drawImage = gameObject.transform.GetChild(0).GetChild(0).GetComponent<Image>();
        drawText = gameObject.transform.GetChild(0).GetChild(1).GetComponent<Text>();
        openText = gameObject.transform.GetChild(0).GetChild(4).GetComponent<Text>();
    }
    // 여기서는 뽑기 이미지와 이름을 업데이트하는 역할을 한다.
    private void OnEnable()
    {
        StartCoroutine(DrawInfoChages());
    }


    IEnumerator DrawInfoChages()
    {
        yield return new WaitForSeconds(0.01f);
        drawText.text = DrawManager.instance.boxName;

        if (drawText.text == "Common Skill")
        {
            openText.text = "1~2 star random all skill cards. \n You can get one.";
            drawImage.sprite = commonSkill;
        }
        else if (drawText.text == "Warrior Skill")
        {
            openText.text = "1~2 star random warrior skill cards. \n You can get one.";
            drawImage.sprite = warriorSkill;
        }
        else if (drawText.text == "Wizard Skill")
        {
            openText.text = "1~2 star random wizard skill cards. \n You can get one.";
            drawImage.sprite = wizardSkill;
        }
        else if (drawText.text == "Assassin Skill")
        {
            openText.text = "1~2 star random assassin skill cards. \n You can get one.";
            drawImage.sprite = assassinSkill;
        }
        else if (drawText.text == "Common Skill_P")
        {
            openText.text = "2~3 star random all skill cards. \n You can get one.";
            drawImage.sprite = commonSkill_P;
        }
        else if (drawText.text == "Warrior Skill_P")
        {
            openText.text = "2~3 star warrior all skill cards. \n You can get one.";
            drawImage.sprite = warriorSkill_P;
        }
        else if (drawText.text == "Wizard Skill_P")
        {
            openText.text = "2~3 star wizard all skill cards. \n You can get one.";
            drawImage.sprite = wizardSkill_P;
        }
        else if (drawText.text == "Assassin Skill_P")
        {
            openText.text = "2~3 star assasin all skill cards. \n You can get one.";
            drawImage.sprite = assassinSkill_P;
        }
        else if (drawText.text == "Random Tower")
        {
            openText.text = "1~3 star random all tower cards. \n You can get one.";
            drawImage.sprite = randomTower;
        }
        else if (drawText.text == "Attack Tower")
        {
            openText.text = "1~3 star random attack tower cards. \n You can get one.";
            drawImage.sprite = attackTower;
        }
        else if (drawText.text == "Minion Tower")
        {
            openText.text = "1~3 star random minion tower cards. \n You can get one.";
            drawImage.sprite = minionTower;
        }
        else if (drawText.text == "Buff & Debuff Tower")
        {
            openText.text = "1~3 star random buff tower cards. \n You can get one.";
            drawImage.sprite = buffTower;
        }
        else if (drawText.text == "Random Tower_P")
        {
            openText.text = "1~3 star random all tower cards. \n You can get one.";
            drawImage.sprite = randomTower_P;
        }
        else if (drawText.text == "Attack Tower_P")
        {
            openText.text = "1~3 star random attack tower cards. \n You can get one.";
            drawImage.sprite = attackTower_P;
        }
        else if (drawText.text == "Minion Tower_P")
        {
            openText.text = "1~3 star random minion tower cards. \n You can get one.";
            drawImage.sprite = minionTower_P;
        }
        else if (drawText.text == "Buff & Debuff Tower_P")
        {
            openText.text = "1~3 star random buff tower cards. \n You can get one.";
            drawImage.sprite = buffTower_P;
        }





    }
}
