//#define OLD_VER
#define PARSE_VER

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkillCoolTimeManager : MonoBehaviour
{
    // ###############################################
    //             NAME : HongSW                      
    //             MAIL : gkenfktm@gmail.com         
    // ###############################################

    public Image[] coolTimeImgs;
    public bool[] isCoolDown;
    float[] skillCoolTimeArr;

    private void OnEnable()
    {
        coolTimeImgs = new Image[4];
        isCoolDown = new bool[4];
        skillCoolTimeArr = new float[4];
    }

    void Start()
    {
        StartCoroutine(Init());
    }

    IEnumerator Init()
    {
        yield return new WaitForSeconds(0.5f);

        // q스킬 제외중

        for (int i = 0; i < 4; i++)
        {
            coolTimeImgs[i] = PlayerHUD.Instance.skillTable.transform.GetChild(i).GetChild(2).gameObject.GetComponent<Image>();
            //Debug.Log($" i : {i}\n" +
            //    $"coltime : {coolTimeImgs[i]}");
#if OLD_VER
            skillCoolTimeArr[i] = SkillManager.Instance.CoolTime[i];
#endif

#if PARSE_VER
            skillCoolTimeArr[i] = SkillManager.Instance.Datas[i].CoolTime;
#endif
        }
    }

    private void Update()
    {
        //TODO : 이벤트함수로 만들어 호출하기
        CoolTimeCheckQ();
        CoolTimeCheckW();
        CoolTimeCheckE();
        CoolTimeCheckR();
    }

    public void CoolTimeCheckQ()
    {
        if (isCoolDown[0])
        {
            coolTimeImgs[0].fillAmount -= 1 / skillCoolTimeArr[0] * Time.deltaTime;

            if (coolTimeImgs[0].fillAmount <= 0f)
            {
                coolTimeImgs[0].fillAmount = 0f;
                isCoolDown[0] = false;
            }

        }

    }

    public void CoolTimeCheckW()
    {
        if (isCoolDown[1])
        {
            coolTimeImgs[1].fillAmount -= 1 / skillCoolTimeArr[1] * Time.deltaTime;

            if (coolTimeImgs[1].fillAmount <= 0f)
            {
                coolTimeImgs[1].fillAmount = 0f;
                isCoolDown[1] = false;
            }
        }
    }

    public void CoolTimeCheckE()
    {
        if (isCoolDown[2])
        {
            coolTimeImgs[2].fillAmount -= 1 / skillCoolTimeArr[2] * Time.deltaTime;

            if (coolTimeImgs[2].fillAmount <= 0f)
            {
                coolTimeImgs[2].fillAmount = 0f;
                isCoolDown[2] = false;
            }
        }
    }

    public void CoolTimeCheckR()
    {
        if (isCoolDown[3])
        {
            coolTimeImgs[3].fillAmount -= 1 / skillCoolTimeArr[3] * Time.deltaTime;

            if (coolTimeImgs[3].fillAmount <= 0f)
            {
                coolTimeImgs[3].fillAmount = 0f;
                isCoolDown[3] = false;
            }
        }
    }
}
