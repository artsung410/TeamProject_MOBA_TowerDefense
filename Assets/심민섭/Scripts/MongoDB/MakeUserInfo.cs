using UnityEngine;
using MongoDB.Bson;

// DB ������ �����.
public class MakeUserInfo : MonoBehaviour
{
    // ###############################################
    //             NAME : Simstealer                      
    //             MAIL : minsub4400@gmail.com         
    // ###############################################

    // user_info

    // ���� ���� id
    private static string id;
    // ���� ���� �г���
    private static string nickName;
    public BsonDocument User_Info = new BsonDocument()
    {
        {"user_id",  id},
        {"user_nickName", nickName}
    };

    // ���� ���� ī�� ��
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
        {"�����ֵθ���1", swing[0]},
        {"�����ֵθ���2", swing[1]},
        {"�����ֵθ���3", swing[2]},
        {"�����ֵθ���4", swing[3]},
        {"�����ֵθ���5", swing[4]},
        {"�������1", takeDown[0]},
        {"�������2", takeDown[1]},
        {"�������3", takeDown[2]},
        {"�������4", takeDown[3]},
        {"�������5", takeDown[4]},
        {"������1", wheelWind[0]},
        {"������2", wheelWind[1]},
        {"������3", wheelWind[2]},
        {"������4", wheelWind[3]},
        {"������5", wheelWind[4]},
        {"�˱�1", Geometry[0]},
        {"�˱�2", Geometry[1]},
        {"�˱�3", Geometry[2]},
        {"�˱�4", Geometry[3]},
        {"�˱�5", Geometry[4]},
        {"����1", jump[0]},
        {"����2", jump[1]},
        {"����3", jump[2]},
        {"����4", jump[3]},
        {"����5", jump[4]},
    };

    private static int[] Blaze;
    private static int[] lightning;
    private static int[] blink;
    private static int[] iceArrow;
    private static int[] juenergyBoltmp;
    public BsonDocument User_wizardSkillCardInfo = new BsonDocument()
    {
        {"user_id", id},
        {"������1", Blaze[0]},
        {"������2", Blaze[1]},
        {"������3", Blaze[2]},
        {"������4", Blaze[3]},
        {"������5", Blaze[4]},
        {"����Ʈ��1", lightning[0]},
        {"����Ʈ��2", lightning[1]},
        {"����Ʈ��3", lightning[2]},
        {"����Ʈ��4", lightning[3]},
        {"����Ʈ��5", lightning[4]},
        {"��ũ1", blink[0]},
        {"��ũ2", blink[1]},
        {"��ũ3", blink[2]},
        {"��ũ4", blink[3]},
        {"��ũ5", blink[4]},
        {"���̽��ַο�1", iceArrow[0]},
        {"���̽��ַο�2", iceArrow[1]},
        {"���̽��ַο�3", iceArrow[2]},
        {"���̽��ַο�4", iceArrow[3]},
        {"���̽��ַο�5", iceArrow[4]},
        {"��������Ʈ1", juenergyBoltmp[0]},
        {"��������Ʈ2", juenergyBoltmp[1]},
        {"��������Ʈ3", juenergyBoltmp[2]},
        {"��������Ʈ4", juenergyBoltmp[3]},
        {"��������Ʈ5", juenergyBoltmp[4]},
    };

    private static int[] increasedMinionProduction;
    private static int[] increasedMinionAttackPower;
    private static int[] dragonSummon;
    public BsonDocument User_inherenceSkillCardInfo = new BsonDocument()
    {
        {"user_id", id},
        {"�̴Ͼ� ���귮 ����1", increasedMinionProduction[0]},
        {"�̴Ͼ� ���귮 ����2", increasedMinionProduction[1]},
        {"�̴Ͼ� ���귮 ����3", increasedMinionProduction[2]},
        {"�̴Ͼ� ���귮 ����4", increasedMinionProduction[3]},
        {"�̴Ͼ� ���귮 ����5", increasedMinionProduction[4]},
        {"�̴Ͼ� ���ݷ� ����1", increasedMinionAttackPower[0]},
        {"�̴Ͼ� ���ݷ� ����2", increasedMinionAttackPower[1]},
        {"�̴Ͼ� ���ݷ� ����3", increasedMinionAttackPower[2]},
        {"�̴Ͼ� ���ݷ� ����4", increasedMinionAttackPower[3]},
        {"�̴Ͼ� ���ݷ� ����5", increasedMinionAttackPower[4]},
        {"�巡�� ��ȯ1", dragonSummon[0]},
        {"�巡�� ��ȯ2", dragonSummon[1]},
        {"�巡�� ��ȯ3", dragonSummon[2]},
        {"�巡�� ��ȯ4", dragonSummon[3]},
        {"�巡�� ��ȯ5", dragonSummon[4]},
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

