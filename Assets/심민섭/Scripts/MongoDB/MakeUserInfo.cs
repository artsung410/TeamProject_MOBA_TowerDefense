using UnityEngine;
using MongoDB.Bson;
using System.Collections;
using System.Collections.Generic;

// DB 정보를 만든다.
// 1. InventoryGetData에서 데이터를 받아서 아래의 틀에 넣어준다.
public class MakeUserInfo : MonoBehaviour
{
    public static MakeUserInfo instance;
    // ###############################################
    //             NAME : Simstealer                      
    //             MAIL : minsub4400@gmail.com         
    // ###############################################

    private InventoryGetData inventoryGetData;

    

    private void Awake()
    {
        instance = this;
    }

    // 유저 정보
    public string id;
    public string nickName;
    private PlayerStorage playerStorage;
    public void User_Info_DataSet()
    {
        playerStorage = GameObject.FindGameObjectWithTag("GetCaller").GetComponent<PlayerStorage>();
        id = playerStorage._id;
        nickName = playerStorage.userName;
    }

    // 유저 보유 카드 수
    public int haveCardCnt;
    public int warriorCardCnt;
    public int wizardCardCnt;
    public int inherenceCardCnt;
    public int towerCardCnt;
    public int otherItemCnt;

    public void User_Card_Info_DataSet()
    {
        haveCardCnt = InventoryGetData.instance.haveCardCnt;
        warriorCardCnt = InventoryGetData.instance.warriorCardCnt;
        wizardCardCnt = InventoryGetData.instance.wizardCardCnt;
        inherenceCardCnt = InventoryGetData.instance.inherenceCardCnt;
        towerCardCnt = InventoryGetData.instance.towerCardCnt;
        otherItemCnt = InventoryGetData.instance.otherItemCnt;
    }


    private void Start()
    {
        ChainAttack = new int[3];
        Smash = new int[3];
        Whirlwind = new int[3];
        SpritSword = new int[3];
        Leap = new int[3];

        Blaze = new int[3];
        Lightning = new int[3];
        Blink = new int[3];
        IceArrow = new int[3];
        EnergyBolt = new int[3];

        increasedMinionProduction = new int[3];
        increasedMinionAttackPower = new int[3];
        dragonSummon = new int[3];

        towerAttackGuard = new int[5];
        towerAttackCannon = new int[5];
        towerAttackFlame = new int[5];
        towerAttackFrozenOrb = new int[5];
        towerAttackLightning = new int[5];
        towerAttackBlackHall = new int[5];
        towerAttackRuined = new int[5];
        towerAttackLaser = new int[5];
        towerAttackWind = new int[5];
        towerAttackLight = new int[5];
        towerMinionWolf = new int[5];
        towerMinionGolem = new int[5];
        towerMinionTreantGuard = new int[5];
        towerMinionGoblin = new int[5];
        towerMinionMonster_Bee = new int[5];
        towerMinionMushroom = new int[5];
        towerBuffDamageIncrease = new int[5];
        towerBuffAttackSpeedIncrease = new int[5];
        towerBuffHp_RegenIncrease = new int[5];
        towerBuffSpeedIncrease = new int[5];
        towerDeBuffDamageDecrease = new int[5];
        towerDeBuffHpRegenDecrease = new int[5];
        towerDeBuffMoveDecrease = new int[5];
        towerDeBuffAttackSpeedDecrease = new int[5];
}

    // TODO: 인벤토리 정리/리팩토링 
    // 전사 카드 보유 데이터 목록
    public int[] ChainAttack;
    public int[] Smash;
    public int[] Whirlwind;
    public int[] SpritSword;
    public int[] Leap;
    public void User_Warrior_Card_Info_DataSet()
    {
        for (int i = 0; i < InventoryGetData.instance.warriorInventoryData.Count; i++)
        {
            // 마구 휘두르기
            if (InventoryGetData.instance.warriorInventoryData[i].GetComponent<ItemOnObject>().item.itemID == 1)
            {
                ChainAttack[0] = InventoryGetData.instance.warriorInventoryData[i].GetComponent<ItemOnObject>().item.itemValue;
            }
            else if (InventoryGetData.instance.warriorInventoryData[i].GetComponent<ItemOnObject>().item.itemID == 2)
            {
                ChainAttack[1] = InventoryGetData.instance.warriorInventoryData[i].GetComponent<ItemOnObject>().item.itemValue;
            }
            else if (InventoryGetData.instance.warriorInventoryData[i].GetComponent<ItemOnObject>().item.itemID == 3)
            {
                ChainAttack[2] = InventoryGetData.instance.warriorInventoryData[i].GetComponent<ItemOnObject>().item.itemValue;
            }
            // 내려찍기
            else if (InventoryGetData.instance.warriorInventoryData[i].GetComponent<ItemOnObject>().item.itemID == 4)
            {
                Smash[0] = InventoryGetData.instance.warriorInventoryData[i].GetComponent<ItemOnObject>().item.itemValue;
            }
            else if (InventoryGetData.instance.warriorInventoryData[i].GetComponent<ItemOnObject>().item.itemID == 5)
            {
                Smash[1] = InventoryGetData.instance.warriorInventoryData[i].GetComponent<ItemOnObject>().item.itemValue;
            }
            else if (InventoryGetData.instance.warriorInventoryData[i].GetComponent<ItemOnObject>().item.itemID == 6)
            {
                Smash[2] = InventoryGetData.instance.warriorInventoryData[i].GetComponent<ItemOnObject>().item.itemValue;
            }
            // 휠윈드
            else if (InventoryGetData.instance.warriorInventoryData[i].GetComponent<ItemOnObject>().item.itemID == 7)
            {
                Whirlwind[0] = InventoryGetData.instance.warriorInventoryData[i].GetComponent<ItemOnObject>().item.itemValue;
            }
            else if (InventoryGetData.instance.warriorInventoryData[i].GetComponent<ItemOnObject>().item.itemID == 8)
            {
                Whirlwind[1] = InventoryGetData.instance.warriorInventoryData[i].GetComponent<ItemOnObject>().item.itemValue;
            }
            else if (InventoryGetData.instance.warriorInventoryData[i].GetComponent<ItemOnObject>().item.itemID == 9)
            {
                Whirlwind[2] = InventoryGetData.instance.warriorInventoryData[i].GetComponent<ItemOnObject>().item.itemValue;
            }
            // 검기
            else if (InventoryGetData.instance.warriorInventoryData[i].GetComponent<ItemOnObject>().item.itemID == 10)
            {
                SpritSword[0] = InventoryGetData.instance.warriorInventoryData[i].GetComponent<ItemOnObject>().item.itemValue;
            }
            else if (InventoryGetData.instance.warriorInventoryData[i].GetComponent<ItemOnObject>().item.itemID == 11)
            {
                SpritSword[1] = InventoryGetData.instance.warriorInventoryData[i].GetComponent<ItemOnObject>().item.itemValue;
            }
            else if (InventoryGetData.instance.warriorInventoryData[i].GetComponent<ItemOnObject>().item.itemID == 12)
            {
                SpritSword[2] = InventoryGetData.instance.warriorInventoryData[i].GetComponent<ItemOnObject>().item.itemValue;
            }
            // 도약
            else if (InventoryGetData.instance.warriorInventoryData[i].GetComponent<ItemOnObject>().item.itemID == 13)
            {
                Leap[0] = InventoryGetData.instance.warriorInventoryData[i].GetComponent<ItemOnObject>().item.itemValue;
            }
            else if (InventoryGetData.instance.warriorInventoryData[i].GetComponent<ItemOnObject>().item.itemID == 14)
            {
                Leap[1] = InventoryGetData.instance.warriorInventoryData[i].GetComponent<ItemOnObject>().item.itemValue;
            }
            else if (InventoryGetData.instance.warriorInventoryData[i].GetComponent<ItemOnObject>().item.itemID == 15)
            {
                Leap[2] = InventoryGetData.instance.warriorInventoryData[i].GetComponent<ItemOnObject>().item.itemValue;
            }
        }
    }

    public int[] Blaze;
    public int[] Lightning;
    public int[] Blink;
    public int[] IceArrow;
    public int[] EnergyBolt;
    public void User_Wizard_Card_Info_DataSet()
    {
        for (int i = 0; i < InventoryGetData.instance.wizardInventoryData.Count; i++)
        {
            // 블리자드
            if (InventoryGetData.instance.wizardInventoryData[i].GetComponent<ItemOnObject>().item.itemID == 1)
            {
                Blaze[0] = InventoryGetData.instance.wizardInventoryData[i].GetComponent<ItemOnObject>().item.itemValue;
            }
            else if (InventoryGetData.instance.wizardInventoryData[i].GetComponent<ItemOnObject>().item.itemID == 2)
            {
                Blaze[1] = InventoryGetData.instance.wizardInventoryData[i].GetComponent<ItemOnObject>().item.itemValue;
            }
            else if (InventoryGetData.instance.wizardInventoryData[i].GetComponent<ItemOnObject>().item.itemID == 3)
            {
                Blaze[2] = InventoryGetData.instance.wizardInventoryData[i].GetComponent<ItemOnObject>().item.itemValue;
            }
            // 라이트닝
            else if (InventoryGetData.instance.wizardInventoryData[i].GetComponent<ItemOnObject>().item.itemID == 4)
            {
                Lightning[0] = InventoryGetData.instance.wizardInventoryData[i].GetComponent<ItemOnObject>().item.itemValue;
            }
            else if (InventoryGetData.instance.wizardInventoryData[i].GetComponent<ItemOnObject>().item.itemID == 5)
            {
                Lightning[1] = InventoryGetData.instance.wizardInventoryData[i].GetComponent<ItemOnObject>().item.itemValue;
            }
            else if (InventoryGetData.instance.wizardInventoryData[i].GetComponent<ItemOnObject>().item.itemID == 6)
            {
                Lightning[2] = InventoryGetData.instance.wizardInventoryData[i].GetComponent<ItemOnObject>().item.itemValue;
            }
            // 블링크
            else if (InventoryGetData.instance.wizardInventoryData[i].GetComponent<ItemOnObject>().item.itemID == 7)
            {
                Blink[0] = InventoryGetData.instance.wizardInventoryData[i].GetComponent<ItemOnObject>().item.itemValue;
            }
            else if (InventoryGetData.instance.wizardInventoryData[i].GetComponent<ItemOnObject>().item.itemID == 8)
            {
                Blink[1] = InventoryGetData.instance.wizardInventoryData[i].GetComponent<ItemOnObject>().item.itemValue;
            }
            else if (InventoryGetData.instance.wizardInventoryData[i].GetComponent<ItemOnObject>().item.itemID == 9)
            {
                Blink[2] = InventoryGetData.instance.wizardInventoryData[i].GetComponent<ItemOnObject>().item.itemValue;
            }
            // 아이스애로우
            else if (InventoryGetData.instance.wizardInventoryData[i].GetComponent<ItemOnObject>().item.itemID == 10)
            {
                IceArrow[0] = InventoryGetData.instance.wizardInventoryData[i].GetComponent<ItemOnObject>().item.itemValue;
            }
            else if (InventoryGetData.instance.wizardInventoryData[i].GetComponent<ItemOnObject>().item.itemID == 11)
            {
                IceArrow[1] = InventoryGetData.instance.wizardInventoryData[i].GetComponent<ItemOnObject>().item.itemValue;
            }
            else if (InventoryGetData.instance.wizardInventoryData[i].GetComponent<ItemOnObject>().item.itemID == 12)
            {
                IceArrow[2] = InventoryGetData.instance.wizardInventoryData[i].GetComponent<ItemOnObject>().item.itemValue;
            }
            // 에너지볼트
            else if (InventoryGetData.instance.wizardInventoryData[i].GetComponent<ItemOnObject>().item.itemID == 13)
            {
                EnergyBolt[0] = InventoryGetData.instance.wizardInventoryData[i].GetComponent<ItemOnObject>().item.itemValue;
            }
            else if (InventoryGetData.instance.wizardInventoryData[i].GetComponent<ItemOnObject>().item.itemID == 14)
            {
                EnergyBolt[1] = InventoryGetData.instance.wizardInventoryData[i].GetComponent<ItemOnObject>().item.itemValue;
            }
            else if (InventoryGetData.instance.wizardInventoryData[i].GetComponent<ItemOnObject>().item.itemID == 15)
            {
                EnergyBolt[2] = InventoryGetData.instance.wizardInventoryData[i].GetComponent<ItemOnObject>().item.itemValue;
            }
        }
    }
    

    public int[] increasedMinionProduction;
    public int[] increasedMinionAttackPower;
    public int[] dragonSummon;
    public void User_Inherence_Card_Info_DataSet()
    {
        for (int i = 0; i < InventoryGetData.instance.inherenceInventoryData.Count; i++)
        {
            // 미니언 생산력 증가
            if (InventoryGetData.instance.inherenceInventoryData[i].GetComponent<ItemOnObject>().item.itemID == 1)
            {
                increasedMinionProduction[0] = InventoryGetData.instance.inherenceInventoryData[i].GetComponent<ItemOnObject>().item.itemValue;
            }
            else if (InventoryGetData.instance.inherenceInventoryData[i].GetComponent<ItemOnObject>().item.itemID == 2)
            {
                increasedMinionProduction[1] = InventoryGetData.instance.inherenceInventoryData[i].GetComponent<ItemOnObject>().item.itemValue;
            }
            else if (InventoryGetData.instance.inherenceInventoryData[i].GetComponent<ItemOnObject>().item.itemID == 3)
            {
                increasedMinionProduction[2] = InventoryGetData.instance.inherenceInventoryData[i].GetComponent<ItemOnObject>().item.itemValue;
            }
            // 미니언 데미지 증가
            else if (InventoryGetData.instance.inherenceInventoryData[i].GetComponent<ItemOnObject>().item.itemID == 4)
            {
                increasedMinionAttackPower[0] = InventoryGetData.instance.inherenceInventoryData[i].GetComponent<ItemOnObject>().item.itemValue;
            }
            else if (InventoryGetData.instance.inherenceInventoryData[i].GetComponent<ItemOnObject>().item.itemID == 5)
            {
                increasedMinionAttackPower[1] = InventoryGetData.instance.inherenceInventoryData[i].GetComponent<ItemOnObject>().item.itemValue;
            }
            else if (InventoryGetData.instance.inherenceInventoryData[i].GetComponent<ItemOnObject>().item.itemID == 6)
            {
                increasedMinionAttackPower[2] = InventoryGetData.instance.inherenceInventoryData[i].GetComponent<ItemOnObject>().item.itemValue;
            }
            // 드래곤 소환
            else if (InventoryGetData.instance.inherenceInventoryData[i].GetComponent<ItemOnObject>().item.itemID == 7)
            {
                dragonSummon[0] = InventoryGetData.instance.inherenceInventoryData[i].GetComponent<ItemOnObject>().item.itemValue;
            }
            else if (InventoryGetData.instance.inherenceInventoryData[i].GetComponent<ItemOnObject>().item.itemID == 8)
            {
                dragonSummon[1] = InventoryGetData.instance.inherenceInventoryData[i].GetComponent<ItemOnObject>().item.itemValue;
            }
            else if (InventoryGetData.instance.inherenceInventoryData[i].GetComponent<ItemOnObject>().item.itemID == 9)
            {
                dragonSummon[2] = InventoryGetData.instance.inherenceInventoryData[i].GetComponent<ItemOnObject>().item.itemValue;
            }
        }
    }

    // 기타 아이템
    public int cardPack_warrior_N;
    public int cardPack_wizard_N;
    public int cardPack_inherence_N;
    public int cardPack_attack_N;
    public int cardPack_minion_N;
    public int cardPack_buff_N;
    public int cardPack_random_N;
    public int cardPack_warrior_P;
    public int cardPack_wizard_P;
    public int cardPack_inherence_P;
    public int cardPack_attack_P;
    public int cardPack_minion_P;
    public int cardPack_buff_P;
    public int cardPack_random_P;
    public void User_OtherInventory_Card_Info_DataSet()
    {
        for (int i = 0; i < InventoryGetData.instance.otherInventoryData.Count; i++)
        {
            // 노말 카드팩
            if (InventoryGetData.instance.otherInventoryData[i].GetComponent<ItemOnObject>().item.itemName == "Warrior Skill"
                && InventoryGetData.instance.otherInventoryData[i].GetComponent<ItemOnObject>().item.ClassType == "Nomal")
            {
                cardPack_warrior_N += InventoryGetData.instance.otherInventoryData[i].GetComponent<ItemOnObject>().item.itemValue;
            }
            else if (InventoryGetData.instance.otherInventoryData[i].GetComponent<ItemOnObject>().item.itemName == "Wizard Skill"
                && InventoryGetData.instance.otherInventoryData[i].GetComponent<ItemOnObject>().item.ClassType == "Nomal")
            {
                cardPack_wizard_N += InventoryGetData.instance.otherInventoryData[i].GetComponent<ItemOnObject>().item.itemValue;
            }
            else if (InventoryGetData.instance.otherInventoryData[i].GetComponent<ItemOnObject>().item.itemName == "Common Skill"
                && InventoryGetData.instance.otherInventoryData[i].GetComponent<ItemOnObject>().item.ClassType == "Nomal")
            {
                cardPack_inherence_N += InventoryGetData.instance.otherInventoryData[i].GetComponent<ItemOnObject>().item.itemValue;
            }
            else if (InventoryGetData.instance.otherInventoryData[i].GetComponent<ItemOnObject>().item.itemName == "Attack Tower"
                && InventoryGetData.instance.otherInventoryData[i].GetComponent<ItemOnObject>().item.ClassType == "Nomal")
            {
                cardPack_attack_N += InventoryGetData.instance.otherInventoryData[i].GetComponent<ItemOnObject>().item.itemValue;
            }
            else if (InventoryGetData.instance.otherInventoryData[i].GetComponent<ItemOnObject>().item.itemName == "Minion Tower"
                && InventoryGetData.instance.otherInventoryData[i].GetComponent<ItemOnObject>().item.ClassType == "Nomal")
            {
                cardPack_minion_N += InventoryGetData.instance.otherInventoryData[i].GetComponent<ItemOnObject>().item.itemValue;
            }
            else if (InventoryGetData.instance.otherInventoryData[i].GetComponent<ItemOnObject>().item.itemName == "Buff Tower"
                && InventoryGetData.instance.otherInventoryData[i].GetComponent<ItemOnObject>().item.ClassType == "Nomal")
            {
                cardPack_buff_N += InventoryGetData.instance.otherInventoryData[i].GetComponent<ItemOnObject>().item.itemValue;
            }
            else if (InventoryGetData.instance.otherInventoryData[i].GetComponent<ItemOnObject>().item.itemName == "Random Tower"
                && InventoryGetData.instance.otherInventoryData[i].GetComponent<ItemOnObject>().item.ClassType == "Nomal")
            {
                cardPack_random_N += InventoryGetData.instance.otherInventoryData[i].GetComponent<ItemOnObject>().item.itemValue;
            }
            // 프리미엄 카드팩
            else if (InventoryGetData.instance.otherInventoryData[i].GetComponent<ItemOnObject>().item.itemName == "Warrior Skill"
                && InventoryGetData.instance.otherInventoryData[i].GetComponent<ItemOnObject>().item.ClassType == "Premium")
            {
                cardPack_warrior_P += InventoryGetData.instance.otherInventoryData[i].GetComponent<ItemOnObject>().item.itemValue;
            }
            else if (InventoryGetData.instance.otherInventoryData[i].GetComponent<ItemOnObject>().item.itemName == "Wizard Skill"
                && InventoryGetData.instance.otherInventoryData[i].GetComponent<ItemOnObject>().item.ClassType == "Premium")
            {
                cardPack_wizard_P += InventoryGetData.instance.otherInventoryData[i].GetComponent<ItemOnObject>().item.itemValue;
            }
            else if (InventoryGetData.instance.otherInventoryData[i].GetComponent<ItemOnObject>().item.itemName == "Common Skill"
                && InventoryGetData.instance.otherInventoryData[i].GetComponent<ItemOnObject>().item.ClassType == "Premium")
            {
                cardPack_inherence_P += InventoryGetData.instance.otherInventoryData[i].GetComponent<ItemOnObject>().item.itemValue;
            }
            else if (InventoryGetData.instance.otherInventoryData[i].GetComponent<ItemOnObject>().item.itemName == "Attack Tower"
                && InventoryGetData.instance.otherInventoryData[i].GetComponent<ItemOnObject>().item.ClassType == "Premium")
            {
                cardPack_attack_P += InventoryGetData.instance.otherInventoryData[i].GetComponent<ItemOnObject>().item.itemValue;
            }
            else if (InventoryGetData.instance.otherInventoryData[i].GetComponent<ItemOnObject>().item.itemName == "Minion Tower"
                && InventoryGetData.instance.otherInventoryData[i].GetComponent<ItemOnObject>().item.ClassType == "Premium")
            {
                cardPack_minion_P += InventoryGetData.instance.otherInventoryData[i].GetComponent<ItemOnObject>().item.itemValue;
            }
            else if (InventoryGetData.instance.otherInventoryData[i].GetComponent<ItemOnObject>().item.itemName == "Buff Tower"
                && InventoryGetData.instance.otherInventoryData[i].GetComponent<ItemOnObject>().item.ClassType == "Premium")
            {
                cardPack_buff_P += InventoryGetData.instance.otherInventoryData[i].GetComponent<ItemOnObject>().item.itemValue;
            }
            else if (InventoryGetData.instance.otherInventoryData[i].GetComponent<ItemOnObject>().item.itemName == "Random Tower"
                && InventoryGetData.instance.otherInventoryData[i].GetComponent<ItemOnObject>().item.ClassType == "Premium")
            {
                cardPack_random_P += InventoryGetData.instance.otherInventoryData[i].GetComponent<ItemOnObject>().item.itemValue;
            }
        }
    }
    public int[] towerAttackGuard;
    public int[] towerAttackCannon;
    public int[] towerAttackFlame;
    public int[] towerAttackFrozenOrb;
    public int[] towerAttackLightning;
    public int[] towerAttackBlackHall;
    public int[] towerAttackRuined;
    public int[] towerAttackLaser;
    public int[] towerAttackWind;
    public int[] towerAttackLight;
    public int[] towerMinionWolf;
    public int[] towerMinionGolem;
    public int[] towerMinionTreantGuard;
    public int[] towerMinionGoblin;
    public int[] towerMinionMonster_Bee;
    public int[] towerMinionMushroom;
    public int[] towerBuffDamageIncrease;
    public int[] towerBuffHp_RegenIncrease;
    public int[] towerBuffSpeedIncrease;
    public int[] towerBuffAttackSpeedIncrease;
    public int[] towerDeBuffDamageDecrease;
    public int[] towerDeBuffHpRegenDecrease;
    public int[] towerDeBuffMoveDecrease;
    public int[] towerDeBuffAttackSpeedDecrease;
    public void User_Tower_Card_Info_DataSet()
    {
        for (int i = 0; i < InventoryGetData.instance.towerInventoryData.Count; i++)
        {
            // 공격 가드
            if (InventoryGetData.instance.towerInventoryData[i].GetComponent<ItemOnObject>().item.itemID == 1)
            {
                towerAttackGuard[0] = InventoryGetData.instance.towerInventoryData[i].GetComponent<ItemOnObject>().item.itemValue;
            }
            else if (InventoryGetData.instance.towerInventoryData[i].GetComponent<ItemOnObject>().item.itemID == 2)
            {
                towerAttackGuard[1] = InventoryGetData.instance.towerInventoryData[i].GetComponent<ItemOnObject>().item.itemValue;
            }
            else if (InventoryGetData.instance.towerInventoryData[i].GetComponent<ItemOnObject>().item.itemID == 3)
            {
                towerAttackGuard[2] = InventoryGetData.instance.towerInventoryData[i].GetComponent<ItemOnObject>().item.itemValue;
            }
            else if (InventoryGetData.instance.towerInventoryData[i].GetComponent<ItemOnObject>().item.itemID == 4)
            {
                towerAttackGuard[3] = InventoryGetData.instance.towerInventoryData[i].GetComponent<ItemOnObject>().item.itemValue;
            }
            else if (InventoryGetData.instance.towerInventoryData[i].GetComponent<ItemOnObject>().item.itemID == 5)
            {
                towerAttackGuard[4] = InventoryGetData.instance.towerInventoryData[i].GetComponent<ItemOnObject>().item.itemValue;
            }
            // 공격 케논
            else if (InventoryGetData.instance.towerInventoryData[i].GetComponent<ItemOnObject>().item.itemID == 6)
            {
                towerAttackCannon[0] = InventoryGetData.instance.towerInventoryData[i].GetComponent<ItemOnObject>().item.itemValue;
            }
            else if (InventoryGetData.instance.towerInventoryData[i].GetComponent<ItemOnObject>().item.itemID == 7)
            {
                towerAttackCannon[1] = InventoryGetData.instance.towerInventoryData[i].GetComponent<ItemOnObject>().item.itemValue;
            }
            else if (InventoryGetData.instance.towerInventoryData[i].GetComponent<ItemOnObject>().item.itemID == 8)
            {
                towerAttackCannon[2] = InventoryGetData.instance.towerInventoryData[i].GetComponent<ItemOnObject>().item.itemValue;
            }
            else if (InventoryGetData.instance.towerInventoryData[i].GetComponent<ItemOnObject>().item.itemID == 9)
            {
                towerAttackCannon[3] = InventoryGetData.instance.towerInventoryData[i].GetComponent<ItemOnObject>().item.itemValue;
            }
            else if (InventoryGetData.instance.towerInventoryData[i].GetComponent<ItemOnObject>().item.itemID == 10)
            {
                towerAttackCannon[4] = InventoryGetData.instance.towerInventoryData[i].GetComponent<ItemOnObject>().item.itemValue;
            }
            // 공격 플레임
            else if (InventoryGetData.instance.towerInventoryData[i].GetComponent<ItemOnObject>().item.itemID == 11)
            {
                towerAttackFlame[0] = InventoryGetData.instance.towerInventoryData[i].GetComponent<ItemOnObject>().item.itemValue;
            }
            else if (InventoryGetData.instance.towerInventoryData[i].GetComponent<ItemOnObject>().item.itemID == 12)
            {
                towerAttackFlame[1] = InventoryGetData.instance.towerInventoryData[i].GetComponent<ItemOnObject>().item.itemValue;
            }
            else if (InventoryGetData.instance.towerInventoryData[i].GetComponent<ItemOnObject>().item.itemID == 13)
            {
                towerAttackFlame[2] = InventoryGetData.instance.towerInventoryData[i].GetComponent<ItemOnObject>().item.itemValue;
            }
            else if (InventoryGetData.instance.towerInventoryData[i].GetComponent<ItemOnObject>().item.itemID == 14)
            {
                towerAttackFlame[3] = InventoryGetData.instance.towerInventoryData[i].GetComponent<ItemOnObject>().item.itemValue;
            }
            else if (InventoryGetData.instance.towerInventoryData[i].GetComponent<ItemOnObject>().item.itemID == 15)
            {
                towerAttackFlame[4] = InventoryGetData.instance.towerInventoryData[i].GetComponent<ItemOnObject>().item.itemValue;
            }
            // 공격 프로즌
            else if (InventoryGetData.instance.towerInventoryData[i].GetComponent<ItemOnObject>().item.itemID == 16)
            {
                towerAttackFrozenOrb[0] = InventoryGetData.instance.towerInventoryData[i].GetComponent<ItemOnObject>().item.itemValue;
            }
            else if (InventoryGetData.instance.towerInventoryData[i].GetComponent<ItemOnObject>().item.itemID == 17)
            {
                towerAttackFrozenOrb[1] = InventoryGetData.instance.towerInventoryData[i].GetComponent<ItemOnObject>().item.itemValue;
            }
            else if (InventoryGetData.instance.towerInventoryData[i].GetComponent<ItemOnObject>().item.itemID == 18)
            {
                towerAttackFrozenOrb[2] = InventoryGetData.instance.towerInventoryData[i].GetComponent<ItemOnObject>().item.itemValue;
            }
            else if (InventoryGetData.instance.towerInventoryData[i].GetComponent<ItemOnObject>().item.itemID == 19)
            {
                towerAttackFrozenOrb[3] = InventoryGetData.instance.towerInventoryData[i].GetComponent<ItemOnObject>().item.itemValue;
            }
            else if (InventoryGetData.instance.towerInventoryData[i].GetComponent<ItemOnObject>().item.itemID == 20)
            {
                towerAttackFrozenOrb[4] = InventoryGetData.instance.towerInventoryData[i].GetComponent<ItemOnObject>().item.itemValue;
            }
            // 공격 라이트닝
            else if (InventoryGetData.instance.towerInventoryData[i].GetComponent<ItemOnObject>().item.itemID == 21)
            {
                towerAttackLightning[0] = InventoryGetData.instance.towerInventoryData[i].GetComponent<ItemOnObject>().item.itemValue;
            }
            else if (InventoryGetData.instance.towerInventoryData[i].GetComponent<ItemOnObject>().item.itemID == 22)
            {
                towerAttackLightning[1] = InventoryGetData.instance.towerInventoryData[i].GetComponent<ItemOnObject>().item.itemValue;
            }
            else if (InventoryGetData.instance.towerInventoryData[i].GetComponent<ItemOnObject>().item.itemID == 23)
            {
                towerAttackLightning[2] = InventoryGetData.instance.towerInventoryData[i].GetComponent<ItemOnObject>().item.itemValue;
            }
            else if (InventoryGetData.instance.towerInventoryData[i].GetComponent<ItemOnObject>().item.itemID == 24)
            {
                towerAttackLightning[3] = InventoryGetData.instance.towerInventoryData[i].GetComponent<ItemOnObject>().item.itemValue;
            }
            else if (InventoryGetData.instance.towerInventoryData[i].GetComponent<ItemOnObject>().item.itemID == 25)
            {
                towerAttackLightning[4] = InventoryGetData.instance.towerInventoryData[i].GetComponent<ItemOnObject>().item.itemValue;
            }
            // 공격 블랙홀
            else if (InventoryGetData.instance.towerInventoryData[i].GetComponent<ItemOnObject>().item.itemID == 26)
            {
                towerAttackBlackHall[0] = InventoryGetData.instance.towerInventoryData[i].GetComponent<ItemOnObject>().item.itemValue;
            }
            else if (InventoryGetData.instance.towerInventoryData[i].GetComponent<ItemOnObject>().item.itemID == 27)
            {
                towerAttackBlackHall[1] = InventoryGetData.instance.towerInventoryData[i].GetComponent<ItemOnObject>().item.itemValue;
            }
            else if (InventoryGetData.instance.towerInventoryData[i].GetComponent<ItemOnObject>().item.itemID == 28)
            {
                towerAttackBlackHall[2] = InventoryGetData.instance.towerInventoryData[i].GetComponent<ItemOnObject>().item.itemValue;
            }
            else if (InventoryGetData.instance.towerInventoryData[i].GetComponent<ItemOnObject>().item.itemID == 29)
            {
                towerAttackBlackHall[3] = InventoryGetData.instance.towerInventoryData[i].GetComponent<ItemOnObject>().item.itemValue;
            }
            else if (InventoryGetData.instance.towerInventoryData[i].GetComponent<ItemOnObject>().item.itemID == 30)
            {
                towerAttackBlackHall[4] = InventoryGetData.instance.towerInventoryData[i].GetComponent<ItemOnObject>().item.itemValue;
            }
            // Ruined 타워
            else if (InventoryGetData.instance.towerInventoryData[i].GetComponent<ItemOnObject>().item.itemID == 31)
            {
                towerAttackRuined[0] = InventoryGetData.instance.towerInventoryData[i].GetComponent<ItemOnObject>().item.itemValue;
            }
            else if (InventoryGetData.instance.towerInventoryData[i].GetComponent<ItemOnObject>().item.itemID == 32)
            {
                towerAttackRuined[1] = InventoryGetData.instance.towerInventoryData[i].GetComponent<ItemOnObject>().item.itemValue;
            }
            else if (InventoryGetData.instance.towerInventoryData[i].GetComponent<ItemOnObject>().item.itemID == 33)
            {
                towerAttackRuined[2] = InventoryGetData.instance.towerInventoryData[i].GetComponent<ItemOnObject>().item.itemValue;
            }
            else if (InventoryGetData.instance.towerInventoryData[i].GetComponent<ItemOnObject>().item.itemID == 34)
            {
                towerAttackRuined[3] = InventoryGetData.instance.towerInventoryData[i].GetComponent<ItemOnObject>().item.itemValue;
            }
            else if (InventoryGetData.instance.towerInventoryData[i].GetComponent<ItemOnObject>().item.itemID == 35)
            {
                towerAttackRuined[4] = InventoryGetData.instance.towerInventoryData[i].GetComponent<ItemOnObject>().item.itemValue;
            }
            // 레이저 타워
            else if (InventoryGetData.instance.towerInventoryData[i].GetComponent<ItemOnObject>().item.itemID == 36)
            {
                towerAttackLaser[0] = InventoryGetData.instance.towerInventoryData[i].GetComponent<ItemOnObject>().item.itemValue;
            }
            else if (InventoryGetData.instance.towerInventoryData[i].GetComponent<ItemOnObject>().item.itemID == 37)
            {
                towerAttackLaser[1] = InventoryGetData.instance.towerInventoryData[i].GetComponent<ItemOnObject>().item.itemValue;
            }
            else if (InventoryGetData.instance.towerInventoryData[i].GetComponent<ItemOnObject>().item.itemID == 38)
            {
                towerAttackLaser[2] = InventoryGetData.instance.towerInventoryData[i].GetComponent<ItemOnObject>().item.itemValue;
            }
            else if (InventoryGetData.instance.towerInventoryData[i].GetComponent<ItemOnObject>().item.itemID == 39)
            {
                towerAttackLaser[3] = InventoryGetData.instance.towerInventoryData[i].GetComponent<ItemOnObject>().item.itemValue;
            }
            else if (InventoryGetData.instance.towerInventoryData[i].GetComponent<ItemOnObject>().item.itemID == 40)
            {
                towerAttackLaser[4] = InventoryGetData.instance.towerInventoryData[i].GetComponent<ItemOnObject>().item.itemValue;
            }
            // 윈드 타워
            else if (InventoryGetData.instance.towerInventoryData[i].GetComponent<ItemOnObject>().item.itemID == 41)
            {
                towerAttackWind[0] = InventoryGetData.instance.towerInventoryData[i].GetComponent<ItemOnObject>().item.itemValue;
            }
            else if (InventoryGetData.instance.towerInventoryData[i].GetComponent<ItemOnObject>().item.itemID == 42)
            {
                towerAttackWind[1] = InventoryGetData.instance.towerInventoryData[i].GetComponent<ItemOnObject>().item.itemValue;
            }
            else if (InventoryGetData.instance.towerInventoryData[i].GetComponent<ItemOnObject>().item.itemID == 43)
            {
                towerAttackWind[2] = InventoryGetData.instance.towerInventoryData[i].GetComponent<ItemOnObject>().item.itemValue;
            }
            else if (InventoryGetData.instance.towerInventoryData[i].GetComponent<ItemOnObject>().item.itemID == 44)
            {
                towerAttackWind[3] = InventoryGetData.instance.towerInventoryData[i].GetComponent<ItemOnObject>().item.itemValue;
            }
            else if (InventoryGetData.instance.towerInventoryData[i].GetComponent<ItemOnObject>().item.itemID == 45)
            {
                towerAttackWind[4] = InventoryGetData.instance.towerInventoryData[i].GetComponent<ItemOnObject>().item.itemValue;
            }
            // Light 타워
            else if (InventoryGetData.instance.towerInventoryData[i].GetComponent<ItemOnObject>().item.itemID == 46)
            {
                towerAttackLight[0] = InventoryGetData.instance.towerInventoryData[i].GetComponent<ItemOnObject>().item.itemValue;
            }
            else if (InventoryGetData.instance.towerInventoryData[i].GetComponent<ItemOnObject>().item.itemID == 47)
            {
                towerAttackLight[1] = InventoryGetData.instance.towerInventoryData[i].GetComponent<ItemOnObject>().item.itemValue;
            }
            else if (InventoryGetData.instance.towerInventoryData[i].GetComponent<ItemOnObject>().item.itemID == 48)
            {
                towerAttackLight[2] = InventoryGetData.instance.towerInventoryData[i].GetComponent<ItemOnObject>().item.itemValue;
            }
            else if (InventoryGetData.instance.towerInventoryData[i].GetComponent<ItemOnObject>().item.itemID == 49)
            {
                towerAttackLight[3] = InventoryGetData.instance.towerInventoryData[i].GetComponent<ItemOnObject>().item.itemValue;
            }
            else if (InventoryGetData.instance.towerInventoryData[i].GetComponent<ItemOnObject>().item.itemID == 50)
            {
                towerAttackLight[4] = InventoryGetData.instance.towerInventoryData[i].GetComponent<ItemOnObject>().item.itemValue;
            }
            // 버프 타워 뎀증
            else if (InventoryGetData.instance.towerInventoryData[i].GetComponent<ItemOnObject>().item.itemID == 51)
            {
                towerBuffDamageIncrease[0] = InventoryGetData.instance.towerInventoryData[i].GetComponent<ItemOnObject>().item.itemValue;
            }
            else if (InventoryGetData.instance.towerInventoryData[i].GetComponent<ItemOnObject>().item.itemID == 52)
            {
                towerBuffDamageIncrease[1] = InventoryGetData.instance.towerInventoryData[i].GetComponent<ItemOnObject>().item.itemValue;
            }
            else if (InventoryGetData.instance.towerInventoryData[i].GetComponent<ItemOnObject>().item.itemID == 53)
            {
                towerBuffDamageIncrease[2] = InventoryGetData.instance.towerInventoryData[i].GetComponent<ItemOnObject>().item.itemValue;
            }
            else if (InventoryGetData.instance.towerInventoryData[i].GetComponent<ItemOnObject>().item.itemID == 54)
            {
                towerBuffDamageIncrease[3] = InventoryGetData.instance.towerInventoryData[i].GetComponent<ItemOnObject>().item.itemValue;
            }
            else if (InventoryGetData.instance.towerInventoryData[i].GetComponent<ItemOnObject>().item.itemID == 55)
            {
                towerBuffDamageIncrease[4] = InventoryGetData.instance.towerInventoryData[i].GetComponent<ItemOnObject>().item.itemValue;
            }
            // 버프 타워 체젠증
            else if (InventoryGetData.instance.towerInventoryData[i].GetComponent<ItemOnObject>().item.itemID == 56)
            {
                towerBuffHp_RegenIncrease[0] = InventoryGetData.instance.towerInventoryData[i].GetComponent<ItemOnObject>().item.itemValue;
            }
            else if (InventoryGetData.instance.towerInventoryData[i].GetComponent<ItemOnObject>().item.itemID == 57)
            {
                towerBuffHp_RegenIncrease[1] = InventoryGetData.instance.towerInventoryData[i].GetComponent<ItemOnObject>().item.itemValue;
            }
            else if (InventoryGetData.instance.towerInventoryData[i].GetComponent<ItemOnObject>().item.itemID == 58)
            {
                towerBuffHp_RegenIncrease[2] = InventoryGetData.instance.towerInventoryData[i].GetComponent<ItemOnObject>().item.itemValue;
            }
            else if (InventoryGetData.instance.towerInventoryData[i].GetComponent<ItemOnObject>().item.itemID == 59)
            {
                towerBuffHp_RegenIncrease[3] = InventoryGetData.instance.towerInventoryData[i].GetComponent<ItemOnObject>().item.itemValue;
            }
            else if (InventoryGetData.instance.towerInventoryData[i].GetComponent<ItemOnObject>().item.itemID == 60)
            {
                towerBuffHp_RegenIncrease[4] = InventoryGetData.instance.towerInventoryData[i].GetComponent<ItemOnObject>().item.itemValue;
            }
            // 버프 타워 이속증
            else if (InventoryGetData.instance.towerInventoryData[i].GetComponent<ItemOnObject>().item.itemID == 61)
            {
                towerBuffSpeedIncrease[0] = InventoryGetData.instance.towerInventoryData[i].GetComponent<ItemOnObject>().item.itemValue;
            }
            else if (InventoryGetData.instance.towerInventoryData[i].GetComponent<ItemOnObject>().item.itemID == 62)
            {
                towerBuffSpeedIncrease[1] = InventoryGetData.instance.towerInventoryData[i].GetComponent<ItemOnObject>().item.itemValue;
            }
            else if (InventoryGetData.instance.towerInventoryData[i].GetComponent<ItemOnObject>().item.itemID == 63)
            {
                towerBuffSpeedIncrease[2] = InventoryGetData.instance.towerInventoryData[i].GetComponent<ItemOnObject>().item.itemValue;
            }
            else if (InventoryGetData.instance.towerInventoryData[i].GetComponent<ItemOnObject>().item.itemID == 64)
            {
                towerBuffSpeedIncrease[3] = InventoryGetData.instance.towerInventoryData[i].GetComponent<ItemOnObject>().item.itemValue;
            }
            else if (InventoryGetData.instance.towerInventoryData[i].GetComponent<ItemOnObject>().item.itemID == 65)
            {
                towerBuffSpeedIncrease[4] = InventoryGetData.instance.towerInventoryData[i].GetComponent<ItemOnObject>().item.itemValue;
            }
            // 버프 타워 공속 증가
            else if (InventoryGetData.instance.towerInventoryData[i].GetComponent<ItemOnObject>().item.itemID == 66)
            {
                towerBuffAttackSpeedIncrease[0] = InventoryGetData.instance.towerInventoryData[i].GetComponent<ItemOnObject>().item.itemValue;
            }
            else if (InventoryGetData.instance.towerInventoryData[i].GetComponent<ItemOnObject>().item.itemID == 67)
            {
                towerBuffAttackSpeedIncrease[1] = InventoryGetData.instance.towerInventoryData[i].GetComponent<ItemOnObject>().item.itemValue;
            }
            else if (InventoryGetData.instance.towerInventoryData[i].GetComponent<ItemOnObject>().item.itemID == 68)
            {
                towerBuffAttackSpeedIncrease[2] = InventoryGetData.instance.towerInventoryData[i].GetComponent<ItemOnObject>().item.itemValue;
            }
            else if (InventoryGetData.instance.towerInventoryData[i].GetComponent<ItemOnObject>().item.itemID == 69)
            {
                towerBuffAttackSpeedIncrease[3] = InventoryGetData.instance.towerInventoryData[i].GetComponent<ItemOnObject>().item.itemValue;
            }
            else if (InventoryGetData.instance.towerInventoryData[i].GetComponent<ItemOnObject>().item.itemID == 70)
            {
                towerBuffAttackSpeedIncrease[4] = InventoryGetData.instance.towerInventoryData[i].GetComponent<ItemOnObject>().item.itemValue;
            }
            // 버프 타워 뎀감
            else if (InventoryGetData.instance.towerInventoryData[i].GetComponent<ItemOnObject>().item.itemID == 71)
            {
                towerDeBuffDamageDecrease[0] = InventoryGetData.instance.towerInventoryData[i].GetComponent<ItemOnObject>().item.itemValue;
            }
            else if (InventoryGetData.instance.towerInventoryData[i].GetComponent<ItemOnObject>().item.itemID == 72)
            {
                towerDeBuffDamageDecrease[1] = InventoryGetData.instance.towerInventoryData[i].GetComponent<ItemOnObject>().item.itemValue;
            }
            else if (InventoryGetData.instance.towerInventoryData[i].GetComponent<ItemOnObject>().item.itemID == 73)
            {
                towerDeBuffDamageDecrease[2] = InventoryGetData.instance.towerInventoryData[i].GetComponent<ItemOnObject>().item.itemValue;
            }
            else if (InventoryGetData.instance.towerInventoryData[i].GetComponent<ItemOnObject>().item.itemID == 74)
            {
                towerDeBuffDamageDecrease[3] = InventoryGetData.instance.towerInventoryData[i].GetComponent<ItemOnObject>().item.itemValue;
            }
            else if (InventoryGetData.instance.towerInventoryData[i].GetComponent<ItemOnObject>().item.itemID == 75)
            {
                towerDeBuffDamageDecrease[4] = InventoryGetData.instance.towerInventoryData[i].GetComponent<ItemOnObject>().item.itemValue;
            }
            // 버프 타워 체젠감
            else if (InventoryGetData.instance.towerInventoryData[i].GetComponent<ItemOnObject>().item.itemID == 76)
            {
                towerDeBuffHpRegenDecrease[0] = InventoryGetData.instance.towerInventoryData[i].GetComponent<ItemOnObject>().item.itemValue;
            }
            else if (InventoryGetData.instance.towerInventoryData[i].GetComponent<ItemOnObject>().item.itemID == 77)
            {
                towerDeBuffHpRegenDecrease[1] = InventoryGetData.instance.towerInventoryData[i].GetComponent<ItemOnObject>().item.itemValue;
            }
            else if (InventoryGetData.instance.towerInventoryData[i].GetComponent<ItemOnObject>().item.itemID == 78)
            {
                towerDeBuffHpRegenDecrease[2] = InventoryGetData.instance.towerInventoryData[i].GetComponent<ItemOnObject>().item.itemValue;
            }
            else if (InventoryGetData.instance.towerInventoryData[i].GetComponent<ItemOnObject>().item.itemID == 79)
            {
                towerDeBuffHpRegenDecrease[3] = InventoryGetData.instance.towerInventoryData[i].GetComponent<ItemOnObject>().item.itemValue;
            }
            else if (InventoryGetData.instance.towerInventoryData[i].GetComponent<ItemOnObject>().item.itemID == 80)
            {
                towerDeBuffHpRegenDecrease[4] = InventoryGetData.instance.towerInventoryData[i].GetComponent<ItemOnObject>().item.itemValue;
            }
            // 버프 타워 이속감
            else if (InventoryGetData.instance.towerInventoryData[i].GetComponent<ItemOnObject>().item.itemID == 81)
            {
                towerDeBuffMoveDecrease[0] = InventoryGetData.instance.towerInventoryData[i].GetComponent<ItemOnObject>().item.itemValue;
            }
            else if (InventoryGetData.instance.towerInventoryData[i].GetComponent<ItemOnObject>().item.itemID == 82)
            {
                towerDeBuffMoveDecrease[1] = InventoryGetData.instance.towerInventoryData[i].GetComponent<ItemOnObject>().item.itemValue;
            }
            else if (InventoryGetData.instance.towerInventoryData[i].GetComponent<ItemOnObject>().item.itemID == 83)
            {
                towerDeBuffMoveDecrease[2] = InventoryGetData.instance.towerInventoryData[i].GetComponent<ItemOnObject>().item.itemValue;
            }
            else if (InventoryGetData.instance.towerInventoryData[i].GetComponent<ItemOnObject>().item.itemID == 84)
            {
                towerDeBuffMoveDecrease[3] = InventoryGetData.instance.towerInventoryData[i].GetComponent<ItemOnObject>().item.itemValue;
            }
            else if (InventoryGetData.instance.towerInventoryData[i].GetComponent<ItemOnObject>().item.itemID == 85)
            {
                towerDeBuffMoveDecrease[4] = InventoryGetData.instance.towerInventoryData[i].GetComponent<ItemOnObject>().item.itemValue;
            }
            // 버프 타워 공속감
            else if (InventoryGetData.instance.towerInventoryData[i].GetComponent<ItemOnObject>().item.itemID == 86)
            {
                towerDeBuffAttackSpeedDecrease[0] = InventoryGetData.instance.towerInventoryData[i].GetComponent<ItemOnObject>().item.itemValue;
            }
            else if (InventoryGetData.instance.towerInventoryData[i].GetComponent<ItemOnObject>().item.itemID == 87)
            {
                towerDeBuffAttackSpeedDecrease[1] = InventoryGetData.instance.towerInventoryData[i].GetComponent<ItemOnObject>().item.itemValue;
            }
            else if (InventoryGetData.instance.towerInventoryData[i].GetComponent<ItemOnObject>().item.itemID == 88)
            {
                towerDeBuffAttackSpeedDecrease[2] = InventoryGetData.instance.towerInventoryData[i].GetComponent<ItemOnObject>().item.itemValue;
            }
            else if (InventoryGetData.instance.towerInventoryData[i].GetComponent<ItemOnObject>().item.itemID == 89)
            {
                towerDeBuffAttackSpeedDecrease[3] = InventoryGetData.instance.towerInventoryData[i].GetComponent<ItemOnObject>().item.itemValue;
            }
            else if (InventoryGetData.instance.towerInventoryData[i].GetComponent<ItemOnObject>().item.itemID == 90)
            {
                towerDeBuffAttackSpeedDecrease[4] = InventoryGetData.instance.towerInventoryData[i].GetComponent<ItemOnObject>().item.itemValue;
            }
            // 미니언 타워 울프
            else if (InventoryGetData.instance.towerInventoryData[i].GetComponent<ItemOnObject>().item.itemID == 91)
            {
                towerMinionWolf[0] = InventoryGetData.instance.towerInventoryData[i].GetComponent<ItemOnObject>().item.itemValue;
            }
            else if (InventoryGetData.instance.towerInventoryData[i].GetComponent<ItemOnObject>().item.itemID == 92)
            {
                towerMinionWolf[1] = InventoryGetData.instance.towerInventoryData[i].GetComponent<ItemOnObject>().item.itemValue;
            }
            else if (InventoryGetData.instance.towerInventoryData[i].GetComponent<ItemOnObject>().item.itemID == 93)
            {
                towerMinionWolf[2] = InventoryGetData.instance.towerInventoryData[i].GetComponent<ItemOnObject>().item.itemValue;
            }
            else if (InventoryGetData.instance.towerInventoryData[i].GetComponent<ItemOnObject>().item.itemID == 94)
            {
                towerMinionWolf[3] = InventoryGetData.instance.towerInventoryData[i].GetComponent<ItemOnObject>().item.itemValue;
            }
            else if (InventoryGetData.instance.towerInventoryData[i].GetComponent<ItemOnObject>().item.itemID == 95)
            {
                towerMinionWolf[4] = InventoryGetData.instance.towerInventoryData[i].GetComponent<ItemOnObject>().item.itemValue;
            }
            // 미니언 타워 골렘
            else if (InventoryGetData.instance.towerInventoryData[i].GetComponent<ItemOnObject>().item.itemID == 96)
            {
                towerMinionGolem[0] = InventoryGetData.instance.towerInventoryData[i].GetComponent<ItemOnObject>().item.itemValue;
            }
            else if (InventoryGetData.instance.towerInventoryData[i].GetComponent<ItemOnObject>().item.itemID == 97)
            {
                towerMinionGolem[1] = InventoryGetData.instance.towerInventoryData[i].GetComponent<ItemOnObject>().item.itemValue;
            }
            else if (InventoryGetData.instance.towerInventoryData[i].GetComponent<ItemOnObject>().item.itemID == 98)
            {
                towerMinionGolem[2] = InventoryGetData.instance.towerInventoryData[i].GetComponent<ItemOnObject>().item.itemValue;
            }
            else if (InventoryGetData.instance.towerInventoryData[i].GetComponent<ItemOnObject>().item.itemID == 99)
            {
                towerMinionGolem[3] = InventoryGetData.instance.towerInventoryData[i].GetComponent<ItemOnObject>().item.itemValue;
            }
            else if (InventoryGetData.instance.towerInventoryData[i].GetComponent<ItemOnObject>().item.itemID == 100)
            {
                towerMinionGolem[4] = InventoryGetData.instance.towerInventoryData[i].GetComponent<ItemOnObject>().item.itemValue;
            }
            // 미니언 타워 엔트
            else if (InventoryGetData.instance.towerInventoryData[i].GetComponent<ItemOnObject>().item.itemID == 101)
            {
                towerMinionTreantGuard[0] = InventoryGetData.instance.towerInventoryData[i].GetComponent<ItemOnObject>().item.itemValue;
            }
            else if (InventoryGetData.instance.towerInventoryData[i].GetComponent<ItemOnObject>().item.itemID == 102)
            {
                towerMinionTreantGuard[1] = InventoryGetData.instance.towerInventoryData[i].GetComponent<ItemOnObject>().item.itemValue;
            }
            else if (InventoryGetData.instance.towerInventoryData[i].GetComponent<ItemOnObject>().item.itemID == 103)
            {
                towerMinionTreantGuard[2] = InventoryGetData.instance.towerInventoryData[i].GetComponent<ItemOnObject>().item.itemValue;
            }
            else if (InventoryGetData.instance.towerInventoryData[i].GetComponent<ItemOnObject>().item.itemID == 104)
            {
                towerMinionTreantGuard[3] = InventoryGetData.instance.towerInventoryData[i].GetComponent<ItemOnObject>().item.itemValue;
            }
            else if (InventoryGetData.instance.towerInventoryData[i].GetComponent<ItemOnObject>().item.itemID == 105)
            {
                towerMinionTreantGuard[4] = InventoryGetData.instance.towerInventoryData[i].GetComponent<ItemOnObject>().item.itemValue;
            }
            // 미니언 타워 고블린
            else if (InventoryGetData.instance.towerInventoryData[i].GetComponent<ItemOnObject>().item.itemID == 106)
            {
                towerMinionGoblin[0] = InventoryGetData.instance.towerInventoryData[i].GetComponent<ItemOnObject>().item.itemValue;
            }
            else if (InventoryGetData.instance.towerInventoryData[i].GetComponent<ItemOnObject>().item.itemID == 107)
            {
                towerMinionGoblin[1] = InventoryGetData.instance.towerInventoryData[i].GetComponent<ItemOnObject>().item.itemValue;
            }
            else if (InventoryGetData.instance.towerInventoryData[i].GetComponent<ItemOnObject>().item.itemID == 108)
            {
                towerMinionGoblin[2] = InventoryGetData.instance.towerInventoryData[i].GetComponent<ItemOnObject>().item.itemValue;
            }
            else if (InventoryGetData.instance.towerInventoryData[i].GetComponent<ItemOnObject>().item.itemID == 109)
            {
                towerMinionGoblin[3] = InventoryGetData.instance.towerInventoryData[i].GetComponent<ItemOnObject>().item.itemValue;
            }
            else if (InventoryGetData.instance.towerInventoryData[i].GetComponent<ItemOnObject>().item.itemID == 110)
            {
                towerMinionGoblin[4] = InventoryGetData.instance.towerInventoryData[i].GetComponent<ItemOnObject>().item.itemValue;
            }
            // 벌
            else if (InventoryGetData.instance.towerInventoryData[i].GetComponent<ItemOnObject>().item.itemID == 111)
            {
                towerMinionMonster_Bee[0] = InventoryGetData.instance.towerInventoryData[i].GetComponent<ItemOnObject>().item.itemValue;
            }
            else if (InventoryGetData.instance.towerInventoryData[i].GetComponent<ItemOnObject>().item.itemID == 112)
            {
                towerMinionMonster_Bee[1] = InventoryGetData.instance.towerInventoryData[i].GetComponent<ItemOnObject>().item.itemValue;
            }
            else if (InventoryGetData.instance.towerInventoryData[i].GetComponent<ItemOnObject>().item.itemID == 113)
            {
                towerMinionMonster_Bee[2] = InventoryGetData.instance.towerInventoryData[i].GetComponent<ItemOnObject>().item.itemValue;
            }
            else if (InventoryGetData.instance.towerInventoryData[i].GetComponent<ItemOnObject>().item.itemID == 114)
            {
                towerMinionMonster_Bee[3] = InventoryGetData.instance.towerInventoryData[i].GetComponent<ItemOnObject>().item.itemValue;
            }
            else if (InventoryGetData.instance.towerInventoryData[i].GetComponent<ItemOnObject>().item.itemID == 115)
            {
                towerMinionMonster_Bee[4] = InventoryGetData.instance.towerInventoryData[i].GetComponent<ItemOnObject>().item.itemValue;
            }
            // 미니언 타워 머쉬룸
            else if (InventoryGetData.instance.towerInventoryData[i].GetComponent<ItemOnObject>().item.itemID == 116)
            {
                towerMinionMushroom[0] = InventoryGetData.instance.towerInventoryData[i].GetComponent<ItemOnObject>().item.itemValue;
            }
            else if (InventoryGetData.instance.towerInventoryData[i].GetComponent<ItemOnObject>().item.itemID == 117)
            {
                towerMinionMushroom[1] = InventoryGetData.instance.towerInventoryData[i].GetComponent<ItemOnObject>().item.itemValue;
            }
            else if (InventoryGetData.instance.towerInventoryData[i].GetComponent<ItemOnObject>().item.itemID == 118)
            {
                towerMinionMushroom[2] = InventoryGetData.instance.towerInventoryData[i].GetComponent<ItemOnObject>().item.itemValue;
            }
            else if (InventoryGetData.instance.towerInventoryData[i].GetComponent<ItemOnObject>().item.itemID == 119)
            {
                towerMinionMushroom[3] = InventoryGetData.instance.towerInventoryData[i].GetComponent<ItemOnObject>().item.itemValue;
            }
            else if (InventoryGetData.instance.towerInventoryData[i].GetComponent<ItemOnObject>().item.itemID == 120)
            {
                towerMinionMushroom[4] = InventoryGetData.instance.towerInventoryData[i].GetComponent<ItemOnObject>().item.itemValue;
            }
        }
    }
}


