using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.UI;
using System;

public class HeroAbility : MonoBehaviourPun
{
    // ###############################################
    //             NAME : HongSW                      
    //             MAIL : gkenfktm@gmail.com         
    // ###############################################

    public GameObject[] AbilityPrefabs;
    public Transform skillSpawn;
    public Image[] coolTimeImgs;

    GameObject go;

    #region private 변수들
    bool[] isCoolDown = new bool[4];
    float[] skillCoolTimeArr = new float[4];

    #endregion

    void Awake()
    {
        for (int i = 0; i < AbilityPrefabs.Length; i++)
        {
            // 트로이목마 안에 있는 아이템 프리팹 게임상에서 가져오기
            AbilityPrefabs[i] = GameObject.FindGameObjectWithTag("GetCaller").gameObject.GetComponent<TrojanHorse>().skillItems[i].itemModel;
        }

    }

    void Start()
    {
        StartCoroutine(Init());
    }

    IEnumerator Init()
    {
        yield return new WaitForSeconds(0.5f);
        for (int i = 0; i < 4; i++)
        {
            coolTimeImgs[i] = PlayerHUD.Instance.skillTable.transform.GetChild(i).GetChild(2).gameObject.GetComponent<Image>();
        }
    }

    void Update()
    {
        if (photonView.IsMine)
        {
            AbilityQ();
            //AbilityW();
            //AbilityE();
            //AbilityR();

            if (go != null)
            {
                go.GetComponent<SkillHandler>().GetPlayerPos(this);
                go.GetComponent<SkillHandler>().GetPlayerStatus(this.GetComponent<Stats>());
                go.GetComponent<SkillHandler>().GetMousePos(this.GetComponent<PlayerBehaviour>());
            }
        }

    }

    private void AbilityQ()
    {
        if (AbilityPrefabs[0] == null)
        {
            return;
        }

        if (Input.GetKeyDown(KeyCode.Q) && isCoolDown[0] == false)
        {
            isCoolDown[0] = true;
            coolTimeImgs[0].fillAmount = 1;

            // q의 데미지
            AbilityPrefabs[0].GetComponent<ChainAttack>().Damage = SkillManager.Instance.atk[0];
            // q의 쿨타임
            skillCoolTimeArr[0] = SkillManager.Instance.time[0];

            go = PhotonNetwork.Instantiate(AbilityPrefabs[0].name, skillSpawn.position, Quaternion.identity);
        }

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


    private void AbilityW()
    {
        if (AbilityPrefabs[1] == null)
        {
            return;
        }

        if (Input.GetKeyDown(KeyCode.Q) && isCoolDown[1] == false)
        {
            isCoolDown[1] = true;
            coolTimeImgs[1].fillAmount = 1;

            // q의 데미지
            AbilityPrefabs[1].GetComponent<ChainAttack>().Damage = SkillManager.Instance.atk[1];
            // q의 쿨타임
            skillCoolTimeArr[1] = SkillManager.Instance.time[1];

            go = PhotonNetwork.Instantiate(AbilityPrefabs[1].name, skillSpawn.position, Quaternion.identity);
        }

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

    private void AbilityE()
    {
        if (AbilityPrefabs[2] == null)
        {
            return;
        }

        if (Input.GetKeyDown(KeyCode.Q) && isCoolDown[2] == false)
        {
            isCoolDown[2] = true;
            coolTimeImgs[2].fillAmount = 1;

            // q의 데미지
            AbilityPrefabs[2].GetComponent<ChainAttack>().Damage = SkillManager.Instance.atk[2];
            // q의 쿨타임
            skillCoolTimeArr[2] = SkillManager.Instance.time[2];

            go = PhotonNetwork.Instantiate(AbilityPrefabs[2].name, skillSpawn.position, Quaternion.identity);
        }

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


    private void AbilityR()
    {
        if (AbilityPrefabs[3] == null)
        {
            return;
        }

        if (Input.GetKeyDown(KeyCode.Q) && isCoolDown[3] == false)
        {
            isCoolDown[3] = true;
            coolTimeImgs[3].fillAmount = 1;

            // q의 데미지
            AbilityPrefabs[3].GetComponent<ChainAttack>().Damage = SkillManager.Instance.atk[3];
            // q의 쿨타임
            skillCoolTimeArr[3] = SkillManager.Instance.time[3];

            go = PhotonNetwork.Instantiate(AbilityPrefabs[3].name, skillSpawn.position, Quaternion.identity);
        }

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
