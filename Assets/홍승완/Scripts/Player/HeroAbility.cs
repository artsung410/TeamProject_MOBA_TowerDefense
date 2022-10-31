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
    //public Image[] coolTimeImgs;
    public SkillCoolTimeManager coolTimeManager;

    GameObject go;

    #region private 변수들
    //bool[] isCoolDown = new bool[4];
    //float[] skillCoolTimeArr = new float[4];

    #endregion

    void Awake()
    {
        int count = GameObject.FindGameObjectWithTag("GetCaller").gameObject.GetComponent<TrojanHorse>().skillItems.Count;

        if (count == 0)
        {
            return;
        }

        AbilityPrefabs = new GameObject[count];
        for (int i = 0; i < count; i++)
        {
            // 트로이목마 안에 있는 아이템 프리팹 게임상에서 가져오기
            AbilityPrefabs[i] = GameObject.FindGameObjectWithTag("GetCaller").gameObject.GetComponent<TrojanHorse>().skillItems[i].itemModel;
        }
    }

    //void Start()
    //{
    //    StartCoroutine(Init());
    //}

    //IEnumerator Init()
    //{
    //    coolTimeImgs = new Image[AbilityPrefabs.Length];
    //    yield return new WaitForSeconds(0.5f);
    //    for (int i = 0; i < 4; i++)
    //    {
    //        coolTimeImgs[i] = PlayerHUD.Instance.skillTable.transform.GetChild(i).GetChild(2).gameObject.GetComponent<Image>();
    //    }
    //}

    void Update()
    {
        // 게임 끝나면 정지
        if (GameManager.Instance.isGameEnd == true)
        {
            return;
        }

        if (photonView.IsMine)
        {
            AbilityQ();
            AbilityW();
            AbilityE();
            AbilityR();

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
        if (AbilityPrefabs[0] == null || AbilityPrefabs[0].GetComponent<SkillHandler>() == null)
        {
            return;
        }

        if (Input.GetKeyDown(KeyCode.Q) && coolTimeManager.isCoolDown[0] == false)
        {
            coolTimeManager.isCoolDown[0] = true;
            coolTimeManager.coolTimeImgs[0].fillAmount = 1;

            AbilityPrefabs[0].GetComponent<SkillHandler>().SetDamage = SkillManager.Instance.atk[0];

            go = PhotonNetwork.Instantiate(AbilityPrefabs[0].name, skillSpawn.position, Quaternion.identity);

        }


    }


    private void AbilityW()
    {
        if (AbilityPrefabs[1] == null || AbilityPrefabs[1].GetComponent<SkillHandler>() == null)
        {
            return;
        }

        if (Input.GetKeyDown(KeyCode.W) && coolTimeManager.isCoolDown[1] == false)
        {
            coolTimeManager.isCoolDown[1] = true;
            coolTimeManager.coolTimeImgs[1].fillAmount = 1;


            AbilityPrefabs[1].GetComponent<SkillHandler>().SetDamage = SkillManager.Instance.atk[1];

            //skillCoolTimeArr[1] = SkillManager.Instance.time[1];

            go = PhotonNetwork.Instantiate(AbilityPrefabs[1].name, skillSpawn.position, Quaternion.identity);
        }

        //if (isCoolDown[1])
        //{
        //    coolTimeImgs[1].fillAmount -= 1 / skillCoolTimeArr[1] * Time.deltaTime;
        //    if (coolTimeImgs[1].fillAmount <= 0f)
        //    {
        //        coolTimeImgs[1].fillAmount = 0f;
        //        isCoolDown[1] = false;
        //    }
        //}
    }

    private void AbilityE()
    {
        if (AbilityPrefabs[2] == null || AbilityPrefabs[2].GetComponent<SkillHandler>() == null)
        {
            return;
        }

        if (Input.GetKeyDown(KeyCode.E) && coolTimeManager.isCoolDown[2] == false)
        {
            coolTimeManager.isCoolDown[2] = true;
            coolTimeManager.coolTimeImgs[2].fillAmount = 1;


            AbilityPrefabs[2].GetComponent<SkillHandler>().SetDamage = SkillManager.Instance.atk[2];
            //skillCoolTimeArr[2] = SkillManager.Instance.time[2];

            go = PhotonNetwork.Instantiate(AbilityPrefabs[2].name, skillSpawn.position, Quaternion.identity);
        }

        //if (isCoolDown[2])
        //{
        //    coolTimeImgs[2].fillAmount -= 1 / skillCoolTimeArr[2] * Time.deltaTime;
        //    if (coolTimeImgs[2].fillAmount <= 0f)
        //    {
        //        coolTimeImgs[2].fillAmount = 0f;
        //        isCoolDown[2] = false;
        //    }
        //}
    }


    private void AbilityR()
    {
        if (AbilityPrefabs[3] == null || AbilityPrefabs[3].GetComponent<SkillHandler>() == null)
        {
            return;
        }

        if (Input.GetKeyDown(KeyCode.R) && coolTimeManager.isCoolDown[3] == false)
        {
            coolTimeManager.isCoolDown[3] = true;
            coolTimeManager.coolTimeImgs[3].fillAmount = 1;
            coolTimeManager.CoolTimeCheckR();


            AbilityPrefabs[3].GetComponent<SkillHandler>().SetDamage = SkillManager.Instance.atk[3];
            //skillCoolTimeArr[3] = SkillManager.Instance.time[3];

            go = PhotonNetwork.Instantiate(AbilityPrefabs[3].name, skillSpawn.position, Quaternion.identity);
        }

        //if (isCoolDown[3])
        //{
        //    coolTimeImgs[3].fillAmount -= 1 / skillCoolTimeArr[3] * Time.deltaTime;

        //    if (coolTimeImgs[3].fillAmount <= 0f)
        //    {
        //        coolTimeImgs[3].fillAmount = 0f;
        //        isCoolDown[3] = false;
        //    }
        //}
    }



}
