using MongoDB.Bson;
using MongoDB.Bson.IO;
using MongoDB.Driver;
using Newtonsoft.Json;
using LitJson;
using MongoDB.Driver.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using System;

public class DataBaseInsert : MonoBehaviour
{
    // ###############################################
    //             NAME : Simstealer                      
    //             MAIL : minsub4400@gmail.com         
    // ###############################################
    MongoClient server = new MongoClient("mongodb+srv://metaverse:metaverse@cluster0.feoedbv.mongodb.net/?retryWrites=true&w=majority");
    IMongoDatabase database;
    IMongoCollection<BsonDocument> collection;

    void Start()
    {
        Debug.Log("데이터 연결 시작");
        database = server.GetDatabase("TowerDefense");
        //collection = database.GetCollection<BsonDocument>("User_Info");
    }

    public async void New_DataInsert_User_Info()
    {
        collection = database.GetCollection<BsonDocument>("User_Info");
        MakeUserInfo.instance.User_Info_DataSet();
        var User_Info = new BsonDocument()
        {
            {"user_id",  MakeUserInfo.instance.id},
            {"user_nickName", MakeUserInfo.instance.nickName}
        };

        Debug.Log("유저 정보 전송");
        await collection.InsertOneAsync(User_Info);
    }
    public async void New_DataInsert_User_Card_Info()
    {
        collection = database.GetCollection<BsonDocument>("User_Card_Info");
        MakeUserInfo.instance.User_Card_Info_DataSet();
        var User_Card_Info = new BsonDocument()
        {
            {"user_id", MakeUserInfo.instance.id},
            {"total_cnt", MakeUserInfo.instance.haveCardCnt},
            {"warrior", MakeUserInfo.instance.warriorCardCnt},
            {"wizard", MakeUserInfo.instance.wizardCardCnt},
            {"inherence", MakeUserInfo.instance.inherenceCardCnt},
            {"tower", MakeUserInfo.instance.towerCardCnt},
            {"other", MakeUserInfo.instance.otherItemCnt},
        };

        Debug.Log("유저 카드 정보 전송");
        await collection.InsertOneAsync(User_Card_Info);
    }
    public async void New_DataInsert_Other_CardPack_Info()
    {
        collection = database.GetCollection<BsonDocument>("User_CardPack_Info");
        MakeUserInfo.instance.User_OtherInventory_Card_Info_DataSet();
        var User_CardPack_Info = new BsonDocument()
        {
            {"user_id", MakeUserInfo.instance.id},
            {"cardPack_warrior_N", MakeUserInfo.instance.cardPack_warrior_N},
            {"cardPack_wizard_N", MakeUserInfo.instance.cardPack_wizard_N},
            {"cardPack_ingerence_N", MakeUserInfo.instance.cardPack_inherence_N},
            {"cardPack_attack_N", MakeUserInfo.instance.cardPack_attack_N},
            {"cardPack_minion_N", MakeUserInfo.instance.cardPack_minion_N},
            {"cardPack_buff_N", MakeUserInfo.instance.cardPack_buff_N},
            {"cardPack_random_N", MakeUserInfo.instance.cardPack_random_N},
            {"cardPack_warrior_P", MakeUserInfo.instance.cardPack_warrior_P},
            {"cardPack_wizard_P", MakeUserInfo.instance.cardPack_wizard_P},
            {"cardPack_ingerence_P", MakeUserInfo.instance.cardPack_inherence_P},
            {"cardPack_attack_P", MakeUserInfo.instance.cardPack_attack_P},
            {"cardPack_minion_P", MakeUserInfo.instance.cardPack_minion_P},
            {"cardPack_buff_P", MakeUserInfo.instance.cardPack_buff_P},
            {"cardPack_random_P", MakeUserInfo.instance.cardPack_random_P},
        };

        Debug.Log("유저 카드팩 정보 전송");
        await collection.InsertOneAsync(User_CardPack_Info);
    }
    public async void New_DataInsert_User_WarriorCard_Info()
    {
        collection = database.GetCollection<BsonDocument>("User_Warrior_Card_Info");
        MakeUserInfo.instance.User_Warrior_Card_Info_DataSet();
        var User_Warrior_Card_Info = new BsonDocument()
        {
            {"user_id", MakeUserInfo.instance.id},
            {"ChainAttack1", MakeUserInfo.instance.ChainAttack[0]},
            {"ChainAttack2", MakeUserInfo.instance.ChainAttack[1]},
            {"ChainAttack3", MakeUserInfo.instance.ChainAttack[2]},
            {"ChainAttack4", MakeUserInfo.instance.ChainAttack[3]},
            {"ChainAttack5", MakeUserInfo.instance.ChainAttack[4]},
            {"Smash1", MakeUserInfo.instance.Smash[0]},
            {"Smash2", MakeUserInfo.instance.Smash[1]},
            {"Smash3", MakeUserInfo.instance.Smash[2]},
            {"Smash4", MakeUserInfo.instance.Smash[3]},
            {"Smash5", MakeUserInfo.instance.Smash[4]},
            {"Whirlwind1", MakeUserInfo.instance.Whirlwind[0]},
            {"Whirlwind2", MakeUserInfo.instance.Whirlwind[1]},
            {"Whirlwind3", MakeUserInfo.instance.Whirlwind[2]},
            {"Whirlwind4", MakeUserInfo.instance.Whirlwind[3]},
            {"Whirlwind5", MakeUserInfo.instance.Whirlwind[4]},
            {"SpritSword1", MakeUserInfo.instance.SpritSword[0]},
            {"SpritSword2", MakeUserInfo.instance.SpritSword[1]},
            {"SpritSword3", MakeUserInfo.instance.SpritSword[2]},
            {"SpritSword4", MakeUserInfo.instance.SpritSword[3]},
            {"SpritSword5", MakeUserInfo.instance.SpritSword[4]},
            {"Leap1", MakeUserInfo.instance.Leap[0]},
            {"Leap2", MakeUserInfo.instance.Leap[1]},
            {"Leap3", MakeUserInfo.instance.Leap[2]},
            {"Leap4", MakeUserInfo.instance.Leap[3]},
            {"Leap5", MakeUserInfo.instance.Leap[4]},
        };
        Debug.Log("유저 전사 카드 정보 전송");
        await collection.InsertOneAsync(User_Warrior_Card_Info);
    }
    public async void New_DataInsert_User_WizardCard_Info()
    {
        collection = database.GetCollection<BsonDocument>("User_Wizard_Card_Info");
        MakeUserInfo.instance.User_Wizard_Card_Info_DataSet();
        var User_Wizard_Card_Info = new BsonDocument()
        {
            {"user_id", MakeUserInfo.instance.id},
            {"Blaze1", MakeUserInfo.instance.Blaze[0]},
            {"Blaze2", MakeUserInfo.instance.Blaze[1]},
            {"Blaze3", MakeUserInfo.instance.Blaze[2]},
            {"Blaze4", MakeUserInfo.instance.Blaze[3]},
            {"Blaze5", MakeUserInfo.instance.Blaze[4]},
            {"Lightning1", MakeUserInfo.instance.Lightning[0]},
            {"Lightning2", MakeUserInfo.instance.Lightning[1]},
            {"Lightning3", MakeUserInfo.instance.Lightning[2]},
            {"Lightning4", MakeUserInfo.instance.Lightning[3]},
            {"Lightning5", MakeUserInfo.instance.Lightning[4]},
            {"Blink1", MakeUserInfo.instance.Blink[0]},
            {"Blink2", MakeUserInfo.instance.Blink[1]},
            {"Blink3", MakeUserInfo.instance.Blink[2]},
            {"Blink4", MakeUserInfo.instance.Blink[3]},
            {"Blink5", MakeUserInfo.instance.Blink[4]},
            {"IceArrow1", MakeUserInfo.instance.IceArrow[0]},
            {"IceArrow2", MakeUserInfo.instance.IceArrow[1]},
            {"IceArrow3", MakeUserInfo.instance.IceArrow[2]},
            {"IceArrow4", MakeUserInfo.instance.IceArrow[3]},
            {"IceArrow5", MakeUserInfo.instance.IceArrow[4]},
            {"EnergyBolt1", MakeUserInfo.instance.EnergyBolt[0]},
            {"EnergyBolt2", MakeUserInfo.instance.EnergyBolt[1]},
            {"EnergyBolt3", MakeUserInfo.instance.EnergyBolt[2]},
            {"EnergyBolt4", MakeUserInfo.instance.EnergyBolt[3]},
            {"EnergyBolt5", MakeUserInfo.instance.EnergyBolt[4]},
        };
        Debug.Log("유저 마법사 카드 정보 전송");
        await collection.InsertOneAsync(User_Wizard_Card_Info);
    }
    public async void New_DataInsert_User_InherenceCard_Info()
    {
        collection = database.GetCollection<BsonDocument>("User_Inherence_Card_Info");
        MakeUserInfo.instance.User_Inherence_Card_Info_DataSet();
        var User_InherenceCard_Card_Info = new BsonDocument()
        {
            {"user_id", MakeUserInfo.instance.id},
            {"increasedMinionProduction1", MakeUserInfo.instance.increasedMinionProduction[0]},
            {"increasedMinionProduction2", MakeUserInfo.instance.increasedMinionProduction[1]},
            {"increasedMinionProduction3", MakeUserInfo.instance.increasedMinionProduction[2]},
            {"increasedMinionProduction4", MakeUserInfo.instance.increasedMinionProduction[3]},
            {"increasedMinionProduction5", MakeUserInfo.instance.increasedMinionProduction[4]},
            {"increasedMinionAttackPower1", MakeUserInfo.instance.increasedMinionAttackPower[0]},
            {"increasedMinionAttackPower2", MakeUserInfo.instance.increasedMinionAttackPower[1]},
            {"increasedMinionAttackPower3", MakeUserInfo.instance.increasedMinionAttackPower[2]},
            {"increasedMinionAttackPower4", MakeUserInfo.instance.increasedMinionAttackPower[3]},
            {"increasedMinionAttackPower5", MakeUserInfo.instance.increasedMinionAttackPower[4]},
            {"dragonSummon1", MakeUserInfo.instance.dragonSummon[0]},
            {"dragonSummon2", MakeUserInfo.instance.dragonSummon[1]},
            {"dragonSummon3", MakeUserInfo.instance.dragonSummon[2]},
            {"dragonSummon4", MakeUserInfo.instance.dragonSummon[3]},
            {"dragonSummon5", MakeUserInfo.instance.dragonSummon[4]},
        };
        Debug.Log("유저 공통 카드 정보 전송");
        await collection.InsertOneAsync(User_InherenceCard_Card_Info);
    }

    public async void New_DataInsert_User_TowerCard_Info()
    {
        collection = database.GetCollection<BsonDocument>("User_Tower_Card_Info");
        MakeUserInfo.instance.User_Tower_Card_Info_DataSet();
        var User_TowerCard_Card_Info = new BsonDocument()
        {
            {"user_id", MakeUserInfo.instance.id},
            {"Tower_Attack_Guard1", MakeUserInfo.instance.towerAttackGuard[0]},
            {"Tower_Attack_Guard2", MakeUserInfo.instance.towerAttackGuard[1]},
            {"Tower_Attack_Guard3", MakeUserInfo.instance.towerAttackGuard[2]},
            {"Tower_Attack_Guard4", MakeUserInfo.instance.towerAttackGuard[3]},
            {"Tower_Attack_Guard5", MakeUserInfo.instance.towerAttackGuard[4]},
            {"Tower_Attack_Cannon1", MakeUserInfo.instance.towerAttackCannon[0]},
            {"Tower_Attack_Cannon2", MakeUserInfo.instance.towerAttackCannon[1]},
            {"Tower_Attack_Cannon3", MakeUserInfo.instance.towerAttackCannon[2]},
            {"Tower_Attack_Cannon4", MakeUserInfo.instance.towerAttackCannon[3]},
            {"Tower_Attack_Cannon5", MakeUserInfo.instance.towerAttackCannon[4]},
            {"Tower_Attack_Flame1", MakeUserInfo.instance.towerAttackFlame[0]},
            {"Tower_Attack_Flame2", MakeUserInfo.instance.towerAttackFlame[1]},
            {"Tower_Attack_Flame3", MakeUserInfo.instance.towerAttackFlame[2]},
            {"Tower_Attack_Flame4", MakeUserInfo.instance.towerAttackFlame[3]},
            {"Tower_Attack_Flame5", MakeUserInfo.instance.towerAttackFlame[4]},
            {"Tower_Attack_FrozenOrb1", MakeUserInfo.instance.towerAttackFrozenOrb[0]},
            {"Tower_Attack_FrozenOrb2", MakeUserInfo.instance.towerAttackFrozenOrb[1]},
            {"Tower_Attack_FrozenOrb3", MakeUserInfo.instance.towerAttackFrozenOrb[2]},
            {"Tower_Attack_FrozenOrb4", MakeUserInfo.instance.towerAttackFrozenOrb[3]},
            {"Tower_Attack_FrozenOrb5", MakeUserInfo.instance.towerAttackFrozenOrb[4]},
            {"Tower_Attack_Lightning1", MakeUserInfo.instance.towerAttackLightning[0]},
            {"Tower_Attack_Lightning2", MakeUserInfo.instance.towerAttackLightning[1]},
            {"Tower_Attack_Lightning3", MakeUserInfo.instance.towerAttackLightning[2]},
            {"Tower_Attack_Lightning4", MakeUserInfo.instance.towerAttackLightning[3]},
            {"Tower_Attack_Lightning5", MakeUserInfo.instance.towerAttackLightning[4]},
            {"Tower_Attack_BlackHall1", MakeUserInfo.instance.towerAttackBlackHall[0]},
            {"Tower_Attack_BlackHall2", MakeUserInfo.instance.towerAttackBlackHall[1]},
            {"Tower_Attack_BlackHall3", MakeUserInfo.instance.towerAttackBlackHall[2]},
            {"Tower_Attack_BlackHall4", MakeUserInfo.instance.towerAttackBlackHall[3]},
            {"Tower_Attack_BlackHall5", MakeUserInfo.instance.towerAttackBlackHall[4]},
            {"Tower_Attack_7_1", MakeUserInfo.instance.towerAttack_7[0]},
            {"Tower_Attack_7_2", MakeUserInfo.instance.towerAttack_7[1]},
            {"Tower_Attack_7_3", MakeUserInfo.instance.towerAttack_7[2]},
            {"Tower_Attack_7_4", MakeUserInfo.instance.towerAttack_7[3]},
            {"Tower_Attack_7_5", MakeUserInfo.instance.towerAttack_7[4]},
            {"Tower_Attack_8_1", MakeUserInfo.instance.towerAttack_8[0]},
            {"Tower_Attack_8_2", MakeUserInfo.instance.towerAttack_8[1]},
            {"Tower_Attack_8_3", MakeUserInfo.instance.towerAttack_8[2]},
            {"Tower_Attack_8_4", MakeUserInfo.instance.towerAttack_8[3]},
            {"Tower_Attack_8_5", MakeUserInfo.instance.towerAttack_8[4]},
            {"Tower_Attack_9_1", MakeUserInfo.instance.towerAttack_9[0]},
            {"Tower_Attack_9_2", MakeUserInfo.instance.towerAttack_9[1]},
            {"Tower_Attack_9_3", MakeUserInfo.instance.towerAttack_9[2]},
            {"Tower_Attack_9_4", MakeUserInfo.instance.towerAttack_9[3]},
            {"Tower_Attack_9_5", MakeUserInfo.instance.towerAttack_9[4]},
            {"Tower_Attack_10_1", MakeUserInfo.instance.towerAttack_10[0]},
            {"Tower_Attack_10_2", MakeUserInfo.instance.towerAttack_10[1]},
            {"Tower_Attack_10_3", MakeUserInfo.instance.towerAttack_10[2]},
            {"Tower_Attack_10_4", MakeUserInfo.instance.towerAttack_10[3]},
            {"Tower_Attack_10_5", MakeUserInfo.instance.towerAttack_10[4]},
            {"Tower_Buff_Damage_Increase1", MakeUserInfo.instance.towerBuffDamageIncrease[0]},
            {"Tower_Buff_Damage_Increase2", MakeUserInfo.instance.towerBuffDamageIncrease[1]},
            {"Tower_Buff_Damage_Increase3", MakeUserInfo.instance.towerBuffDamageIncrease[2]},
            {"Tower_Buff_Damage_Increase4", MakeUserInfo.instance.towerBuffDamageIncrease[3]},
            {"Tower_Buff_Damage_Increase5", MakeUserInfo.instance.towerBuffDamageIncrease[4]},
            {"Tower_Buff_Hp_Regen_Increase1", MakeUserInfo.instance.towerBuffHp_RegenIncrease[0]},
            {"Tower_Buff_Hp_Regen_Increase2", MakeUserInfo.instance.towerBuffHp_RegenIncrease[1]},
            {"Tower_Buff_Hp_Regen_Increase3", MakeUserInfo.instance.towerBuffHp_RegenIncrease[2]},
            {"Tower_Buff_Hp_Regen_Increase4", MakeUserInfo.instance.towerBuffHp_RegenIncrease[3]},
            {"Tower_Buff_Hp_Regen_Increase5", MakeUserInfo.instance.towerBuffHp_RegenIncrease[4]},
            {"Tower_Buff_Speed_Increase1", MakeUserInfo.instance.towerBuffSpeedIncrease[0]},
            {"Tower_Buff_Speed_Increase2", MakeUserInfo.instance.towerBuffSpeedIncrease[1]},
            {"Tower_Buff_Speed_Increase3", MakeUserInfo.instance.towerBuffSpeedIncrease[2]},
            {"Tower_Buff_Speed_Increase4", MakeUserInfo.instance.towerBuffSpeedIncrease[3]},
            {"Tower_Buff_Speed_Increase5", MakeUserInfo.instance.towerBuffSpeedIncrease[4]},
            {"Tower_Buff_Attack_Speed_Increase1", MakeUserInfo.instance.towerBuffAttackSpeedIncrease[0]},
            {"Tower_Buff_Attack_Speed_Increase2", MakeUserInfo.instance.towerBuffAttackSpeedIncrease[1]},
            {"Tower_Buff_Attack_Speed_Increase3", MakeUserInfo.instance.towerBuffAttackSpeedIncrease[2]},
            {"Tower_Buff_Attack_Speed_Increase4", MakeUserInfo.instance.towerBuffAttackSpeedIncrease[3]},
            {"Tower_Buff_Attack_Speed_Increase5", MakeUserInfo.instance.towerBuffAttackSpeedIncrease[4]},
            {"Tower_DeBuff_Damage_Decrease1", MakeUserInfo.instance.towerDeBuffDamageDecrease[0]},
            {"Tower_DeBuff_Damage_Decrease2", MakeUserInfo.instance.towerDeBuffDamageDecrease[1]},
            {"Tower_DeBuff_Damage_Decrease3", MakeUserInfo.instance.towerDeBuffDamageDecrease[2]},
            {"Tower_DeBuff_Damage_Decrease4", MakeUserInfo.instance.towerDeBuffDamageDecrease[3]},
            {"Tower_DeBuff_Damage_Decrease5", MakeUserInfo.instance.towerDeBuffDamageDecrease[4]},
            {"Tower_DeBuff_HpRegen_Decrease1", MakeUserInfo.instance.towerDeBuffHpRegenDecrease[0]},
            {"Tower_DeBuff_HpRegen_Decrease2", MakeUserInfo.instance.towerDeBuffHpRegenDecrease[1]},
            {"Tower_DeBuff_HpRegen_Decrease3", MakeUserInfo.instance.towerDeBuffHpRegenDecrease[2]},
            {"Tower_DeBuff_HpRegen_Decrease4", MakeUserInfo.instance.towerDeBuffHpRegenDecrease[3]},
            {"Tower_DeBuff_HpRegen_Decrease5", MakeUserInfo.instance.towerDeBuffHpRegenDecrease[4]},
            {"Tower_DeBuff_Move_Decrease1", MakeUserInfo.instance.towerDeBuffMoveDecrease[0]},
            {"Tower_DeBuff_Move_Decrease2", MakeUserInfo.instance.towerDeBuffMoveDecrease[1]},
            {"Tower_DeBuff_Move_Decrease3", MakeUserInfo.instance.towerDeBuffMoveDecrease[2]},
            {"Tower_DeBuff_Move_Decrease4", MakeUserInfo.instance.towerDeBuffMoveDecrease[3]},
            {"Tower_DeBuff_Move_Decrease5", MakeUserInfo.instance.towerDeBuffMoveDecrease[4]},
            {"Tower_DeBuff_Attack_Speed_Decrease1", MakeUserInfo.instance.towerDeBuffAttackSpeedDecrease[0]},
            {"Tower_DeBuff_Attack_Speed_Decrease2", MakeUserInfo.instance.towerDeBuffAttackSpeedDecrease[1]},
            {"Tower_DeBuff_Attack_Speed_Decrease3", MakeUserInfo.instance.towerDeBuffAttackSpeedDecrease[2]},
            {"Tower_DeBuff_Attack_Speed_Decrease4", MakeUserInfo.instance.towerDeBuffAttackSpeedDecrease[3]},
            {"Tower_DeBuff_Attack_Speed_Decrease5", MakeUserInfo.instance.towerDeBuffAttackSpeedDecrease[4]},
            {"Tower_Minion_Wolf1", MakeUserInfo.instance.towerMinionWolf[0]},
            {"Tower_Minion_Wolf2", MakeUserInfo.instance.towerMinionWolf[1]},
            {"Tower_Minion_Wolf3", MakeUserInfo.instance.towerMinionWolf[2]},
            {"Tower_Minion_Wolf4", MakeUserInfo.instance.towerMinionWolf[3]},
            {"Tower_Minion_Wolf5", MakeUserInfo.instance.towerMinionWolf[4]},
            {"Tower_Minion_Golem1", MakeUserInfo.instance.towerMinionGolem[0]},
            {"Tower_Minion_Golem2", MakeUserInfo.instance.towerMinionGolem[1]},
            {"Tower_Minion_Golem3", MakeUserInfo.instance.towerMinionGolem[2]},
            {"Tower_Minion_Golem4", MakeUserInfo.instance.towerMinionGolem[3]},
            {"Tower_Minion_Golem5", MakeUserInfo.instance.towerMinionGolem[4]},
            {"Tower_Minion_TreantGuard1", MakeUserInfo.instance.towerMinionTreantGuard[0]},
            {"Tower_Minion_TreantGuard2", MakeUserInfo.instance.towerMinionTreantGuard[1]},
            {"Tower_Minion_TreantGuard3", MakeUserInfo.instance.towerMinionTreantGuard[2]},
            {"Tower_Minion_TreantGuard4", MakeUserInfo.instance.towerMinionTreantGuard[3]},
            {"Tower_Minion_TreantGuard5", MakeUserInfo.instance.towerMinionTreantGuard[4]},
            {"Tower_Minion_Goblin1", MakeUserInfo.instance.towerMinionGoblin[0]},
            {"Tower_Minion_Goblin2", MakeUserInfo.instance.towerMinionGoblin[1]},
            {"Tower_Minion_Goblin3", MakeUserInfo.instance.towerMinionGoblin[2]},
            {"Tower_Minion_Goblin4", MakeUserInfo.instance.towerMinionGoblin[3]},
            {"Tower_Minion_Goblin5", MakeUserInfo.instance.towerMinionGoblin[4]},
            {"Tower_Minion_Monster_Bee1", MakeUserInfo.instance.towerMinionMonster_Bee[0]},
            {"Tower_Minion_Monster_Bee2", MakeUserInfo.instance.towerMinionMonster_Bee[1]},
            {"Tower_Minion_Monster_Bee3", MakeUserInfo.instance.towerMinionMonster_Bee[2]},
            {"Tower_Minion_Monster_Bee4", MakeUserInfo.instance.towerMinionMonster_Bee[3]},
            {"Tower_Minion_Monster_Bee5", MakeUserInfo.instance.towerMinionMonster_Bee[4]},
            {"Tower_Minion_Mushroom1", MakeUserInfo.instance.towerMinionMushroom[0]},
            {"Tower_Minion_Mushroom2", MakeUserInfo.instance.towerMinionMushroom[1]},
            {"Tower_Minion_Mushroom3", MakeUserInfo.instance.towerMinionMushroom[2]},
            {"Tower_Minion_Mushroom4", MakeUserInfo.instance.towerMinionMushroom[3]},
            {"Tower_Minion_Mushroom5", MakeUserInfo.instance.towerMinionMushroom[4]},
         };

        Debug.Log("유저 타워 카드 정보 전송");
        await collection.InsertOneAsync(User_TowerCard_Card_Info);
    }
}