using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class Enemybase : MonoBehaviourPun
{
    // ###############################################
    //             NAME : KimJaeMin                      
    //             MAIL : woals1566@gmail.com         
    // ###############################################

    // �̵��ӵ�
   protected float moveSpeed;
    // ���ݻ�Ÿ�
    protected float attackRange;
    // ���ݷ�
    protected float Damage;
    // ü��
    protected float HP = 100f;
    //���� ��Ÿ��
    protected float AttackTime;

    public string EnemyTag;

    

    void Start()
    {
  
    }

    // Update is called once per frame
    void Update()
    {


    }
    
    
}
