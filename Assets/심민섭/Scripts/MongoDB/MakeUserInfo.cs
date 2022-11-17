using UnityEngine;
using MongoDB.Bson;

// DB 정보를 만든다.
public class MakeUserInfo : MonoBehaviour
{
    // ###############################################
    //             NAME : Simstealer                      
    //             MAIL : minsub4400@gmail.com         
    // ###############################################

    // user_info

    // 유저 고유 id
    private static string id;
    // 유저 고유 닉네임
    private static string nickName;
    public BsonDocument User_Info = new BsonDocument()
    {
        {"user_id",  id},
        {"user_nickName", nickName}
    };

    // 유저 보유 카드 수
    private static string haveCardCnt;
    private static int warriorCardCnt;
    private static int wizardCardCnt;
    private static int inherenceCardCnt;
    private static int attackCardCnt;
    private static int minionCardCnt;
    private static int buffCardCnt;
    public BsonDocument User_CardInfo = new BsonDocument()
    {
        {"user_id", id},
        {"total_cnt", haveCardCnt},
        {"warrior", warriorCardCnt},
        {"wizard", wizardCardCnt},
        {"inherence", inherenceCardCnt},
        {"attack", attackCardCnt},
        {"minion", minionCardCnt},
        {"buff", buffCardCnt}
    };

    private static int[] swing;
    private static int[] takeDown;
    private static int[] wheelWind;
    private static int[] Geometry;
    private static int[] jump;
    public BsonDocument User_warriorSkillCardInfo = new BsonDocument()
    {
        {"user_id", id},
        {"마구휘두르기1", swing[0]},
        {"마구휘두르기2", swing[1]},
        {"마구휘두르기3", swing[2]},
        {"마구휘두르기4", swing[3]},
        {"마구휘두르기5", swing[4]},
        {"내려찍기1", takeDown[0]},
        {"내려찍기2", takeDown[1]},
        {"내려찍기3", takeDown[2]},
        {"내려찍기4", takeDown[3]},
        {"내려찍기5", takeDown[4]},
        {"휠윈드1", wheelWind[0]},
        {"휠윈드2", wheelWind[1]},
        {"휠윈드3", wheelWind[2]},
        {"휠윈드4", wheelWind[3]},
        {"휠윈드5", wheelWind[4]},
        {"검기1", Geometry[0]},
        {"검기2", Geometry[1]},
        {"검기3", Geometry[2]},
        {"검기4", Geometry[3]},
        {"검기5", Geometry[4]},
        {"도약1", jump[0]},
        {"도약2", jump[1]},
        {"도약3", jump[2]},
        {"도약4", jump[3]},
        {"도약5", jump[4]},
    };

    private static int[] Blaze;
    private static int[] lightning;
    private static int[] blink;
    private static int[] iceArrow;
    private static int[] juenergyBoltmp;
    public BsonDocument User_wizardSkillCardInfo = new BsonDocument()
    {
        {"user_id", id},
        {"블레이즈1", Blaze[0]},
        {"블레이즈2", Blaze[1]},
        {"블레이즈3", Blaze[2]},
        {"블레이즈4", Blaze[3]},
        {"블레이즈5", Blaze[4]},
        {"라이트닝1", lightning[0]},
        {"라이트닝2", lightning[1]},
        {"라이트닝3", lightning[2]},
        {"라이트닝4", lightning[3]},
        {"라이트닝5", lightning[4]},
        {"블링크1", blink[0]},
        {"블링크2", blink[1]},
        {"블링크3", blink[2]},
        {"블링크4", blink[3]},
        {"블링크5", blink[4]},
        {"아이스애로우1", iceArrow[0]},
        {"아이스애로우2", iceArrow[1]},
        {"아이스애로우3", iceArrow[2]},
        {"아이스애로우4", iceArrow[3]},
        {"아이스애로우5", iceArrow[4]},
        {"에너지볼트1", juenergyBoltmp[0]},
        {"에너지볼트2", juenergyBoltmp[1]},
        {"에너지볼트3", juenergyBoltmp[2]},
        {"에너지볼트4", juenergyBoltmp[3]},
        {"에너지볼트5", juenergyBoltmp[4]},
    };

    private static int[] increasedMinionProduction;
    private static int[] increasedMinionAttackPower;
    private static int[] dragonSummon;
    public BsonDocument User_inherenceSkillCardInfo = new BsonDocument()
    {
        {"user_id", id},
        {"미니언 생산량 증가1", increasedMinionProduction[0]},
        {"미니언 생산량 증가2", increasedMinionProduction[1]},
        {"미니언 생산량 증가3", increasedMinionProduction[2]},
        {"미니언 생산량 증가4", increasedMinionProduction[3]},
        {"미니언 생산량 증가5", increasedMinionProduction[4]},
        {"미니언 공격력 증가1", increasedMinionAttackPower[0]},
        {"미니언 공격력 증가2", increasedMinionAttackPower[1]},
        {"미니언 공격력 증가3", increasedMinionAttackPower[2]},
        {"미니언 공격력 증가4", increasedMinionAttackPower[3]},
        {"미니언 공격력 증가5", increasedMinionAttackPower[4]},
        {"드래곤 소환1", dragonSummon[0]},
        {"드래곤 소환2", dragonSummon[1]},
        {"드래곤 소환3", dragonSummon[2]},
        {"드래곤 소환4", dragonSummon[3]},
        {"드래곤 소환5", dragonSummon[4]},
    };


    private static int[] towerAttackGuard;
    private static int[] towerAttackCannon;
    private static int[] towerAttackFlame;
    private static int[] towerAttackFrozenOrb;
    private static int[] towerAttackLightning;
    private static int[] towerAttackBlackHall;
    private static int[] towerAttack_7;
    private static int[] towerAttack_8;
    private static int[] towerAttack_9;
    private static int[] towerAttack_10;
    public BsonDocument User_attackTowerCardInfo = new BsonDocument()
    {
        {"user_id", id},
        {"Tower_Attack_Guard1", towerAttackGuard[0]},
        {"Tower_Attack_Guard2", towerAttackGuard[1]},
        {"Tower_Attack_Guard3", towerAttackGuard[2]},
        {"Tower_Attack_Guard4", towerAttackGuard[3]},
        {"Tower_Attack_Guard5", towerAttackGuard[4]},
        {"Tower_Attack_Cannon1", towerAttackCannon[0]},
        {"Tower_Attack_Cannon2", towerAttackCannon[1]},
        {"Tower_Attack_Cannon3", towerAttackCannon[2]},
        {"Tower_Attack_Cannon4", towerAttackCannon[3]},
        {"Tower_Attack_Cannon5", towerAttackCannon[4]},
        {"Tower_Attack_Flame1", towerAttackFlame[0]},
        {"Tower_Attack_Flame2", towerAttackFlame[1]},
        {"Tower_Attack_Flame3", towerAttackFlame[2]},
        {"Tower_Attack_Flame4", towerAttackFlame[3]},
        {"Tower_Attack_Flame5", towerAttackFlame[4]},
        {"Tower_Attack_FrozenOrb1", towerAttackFrozenOrb[0]},
        {"Tower_Attack_FrozenOrb2", towerAttackFrozenOrb[1]},
        {"Tower_Attack_FrozenOrb3", towerAttackFrozenOrb[2]},
        {"Tower_Attack_FrozenOrb4", towerAttackFrozenOrb[3]},
        {"Tower_Attack_FrozenOrb5", towerAttackFrozenOrb[4]},
        {"Tower_Attack_Lightning1", towerAttackLightning[0]},
        {"Tower_Attack_Lightning2", towerAttackLightning[1]},
        {"Tower_Attack_Lightning3", towerAttackLightning[2]},
        {"Tower_Attack_Lightning4", towerAttackLightning[3]},
        {"Tower_Attack_Lightning5", towerAttackLightning[4]},
        {"Tower_Attack_BlackHall1", towerAttackBlackHall[0]},
        {"Tower_Attack_BlackHall2", towerAttackBlackHall[1]},
        {"Tower_Attack_BlackHall3", towerAttackBlackHall[2]},
        {"Tower_Attack_BlackHall4", towerAttackBlackHall[3]},
        {"Tower_Attack_BlackHall5", towerAttackBlackHall[4]},
        {"Tower_Attack_7_1", towerAttack_7[0]},
        {"Tower_Attack_7_2", towerAttack_7[1]},
        {"Tower_Attack_7_3", towerAttack_7[2]},
        {"Tower_Attack_7_4", towerAttack_7[3]},
        {"Tower_Attack_7_5", towerAttack_7[4]},
        {"Tower_Attack_8_1", towerAttack_8[0]},
        {"Tower_Attack_8_2", towerAttack_8[1]},
        {"Tower_Attack_8_3", towerAttack_8[2]},
        {"Tower_Attack_8_4", towerAttack_8[3]},
        {"Tower_Attack_8_5", towerAttack_8[4]},
        {"Tower_Attack_9_1", towerAttack_9[0]},
        {"Tower_Attack_9_2", towerAttack_9[1]},
        {"Tower_Attack_9_3", towerAttack_9[2]},
        {"Tower_Attack_9_4", towerAttack_9[3]},
        {"Tower_Attack_9_5", towerAttack_9[4]},
        {"Tower_Attack_10_1", towerAttack_10[0]},
        {"Tower_Attack_10_2", towerAttack_10[1]},
        {"Tower_Attack_10_3", towerAttack_10[2]},
        {"Tower_Attack_10_4", towerAttack_10[3]},
        {"Tower_Attack_10_5", towerAttack_10[4]}
    };

    public BsonDocument User_minionTowerCardInfo = new BsonDocument()
    {
        {"Tower_Minion_Wolf1", 1},
        {"Tower_Minion_Wolf2", 1},
        {"Tower_Minion_Wolf3", 1},
        {"Tower_Minion_Wolf4", 1},
        {"Tower_Minion_Wolf5", 1},
        {"Tower_Minion_Golem1", 1},
        {"Tower_Minion_Golem2", 1},
        {"Tower_Minion_Golem3", 1},
        {"Tower_Minion_Golem4", 1},
        {"Tower_Minion_Golem5", 1},
        {"Tower_Minion_TreantGuard1", 1},
        {"Tower_Minion_TreantGuard2", 1},
        {"Tower_Minion_TreantGuard3", 1},
        {"Tower_Minion_TreantGuard4", 1},
        {"Tower_Minion_TreantGuard5", 1},
        {"Tower_Minion_Goblin1", 1},
        {"Tower_Minion_Goblin2", 1},
        {"Tower_Minion_Goblin3", 1},
        {"Tower_Minion_Goblin4", 1},
        {"Tower_Minion_Goblin5", 1},
        {"Tower_Minion_Monster_Bee1", 1},
        {"Tower_Minion_Monster_Bee2", 1},
        {"Tower_Minion_Monster_Bee3", 1},
        {"Tower_Minion_Monster_Bee4", 1},
        {"Tower_Minion_Monster_Bee5", 1},
        {"Tower_Minion_Mushroom1", 1},
        {"Tower_Minion_Mushroom2", 1},
        {"Tower_Minion_Mushroom3", 1},
        {"Tower_Minion_Mushroom4", 1},
        {"Tower_Minion_Mushroom5", 1}
    };

    public BsonDocument User_buffTowerCardInfo = new BsonDocument()
    {
        {"Tower_Buff_Damage_Increase1", 1},
        {"Tower_Buff_Damage_Increase2", 1},
        {"Tower_Buff_Damage_Increase3", 1},
        {"Tower_Buff_Damage_Increase4", 1},
        {"Tower_Buff_Damage_Increase5", 1},
        {"Tower_Buff_Hp_Regen_Increase1", 1},
        {"Tower_Buff_Hp_Regen_Increase2", 1},
        {"Tower_Buff_Hp_Regen_Increase3", 1},
        {"Tower_Buff_Hp_Regen_Increase4", 1},
        {"Tower_Buff_Hp_Regen_Increase5", 1},
        {"Tower_Buff_Speed_Increase1", 1},
        {"Tower_Buff_Speed_Increase2", 1},
        {"Tower_Buff_Speed_Increase3", 1},
        {"Tower_Buff_Speed_Increase4", 1},
        {"Tower_Buff_Speed_Increase5", 1},
        {"Tower_DeBuff_Damage_Decrease1", 1},
        {"Tower_DeBuff_Damage_Decrease2", 1},
        {"Tower_DeBuff_Damage_Decrease3", 1},
        {"Tower_DeBuff_Damage_Decrease4", 1},
        {"Tower_DeBuff_Damage_Decrease5", 1},
        {"Tower_DeBuff_HpRegen_Decrease1", 1},
        {"Tower_DeBuff_HpRegen_Decrease2", 1},
        {"Tower_DeBuff_HpRegen_Decrease3", 1},
        {"Tower_DeBuff_HpRegen_Decrease4", 1},
        {"Tower_DeBuff_HpRegen_Decrease5", 1},
        {"Tower_DeBuff_Move_Decrease1", 1},
        {"Tower_DeBuff_Move_Decrease2", 1},
        {"Tower_DeBuff_Move_Decrease3", 1},
        {"Tower_DeBuff_Move_Decrease4", 1},
        {"Tower_DeBuff_Move_Decrease5", 1},
        {"Tower_DeBuff_Attack_Speed_Decrease1", 1},
        {"Tower_DeBuff_Attack_Speed_Decrease2", 1},
        {"Tower_DeBuff_Attack_Speed_Decrease3", 1},
        {"Tower_DeBuff_Attack_Speed_Decrease4", 1},
        {"Tower_DeBuff_Attack_Speed_Decrease5", 1},
    };
}

