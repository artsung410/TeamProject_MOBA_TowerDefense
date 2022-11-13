using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class EnergyBall : MonoBehaviourPun
{
    // ###############################################
    //             NAME : HongSW                      
    //             MAIL : gkenfktm@gmail.com         
    // ###############################################

    public GameObject Target; // �÷��̾�Լ� ���� �޾ƿ�
    float _damage; // �Ľ̵����� �����κ�
    public float EnergyBallSpeed; 
    public bool StopEnergyBall;
    //public string OwnerTag;

    PlayerBehaviour _behaviour;
    Stats _stats;

    void Start()
    {
        if (_behaviour == null || !photonView.IsMine)
        {
            return;
        }

        Target = _behaviour.targetedEnemy;
        _damage = _stats.attackDmg;
        EnergyBallSpeed = 30f;
        //OwnerTag = _behaviour.gameObject.tag;
        //Debug.Log($"Ÿ���� �̸� : {Target.name}");
    }

    void Update()
    {
        if (!photonView.IsMine)
        {
            //Debug.Log($"photonView�� ������ �ƴ�");
            return;
        }

        if (Target == null)
        {
            //Debug.Log($"Target�� null �Ѿ� ����");
            PhotonNetwork.Destroy(gameObject);
        }
        else
        {
            // �Ѿ��� ��ǥ�� Target�̴� => �������� ������ �����Ƿ� MoveToward����Ұ�
            transform.position = Vector3.MoveTowards(transform.position, Target.transform.position, EnergyBallSpeed * Time.deltaTime);
            //Debug.Log($"��������  �ӵ� : {EnergyBallSpeed * Time.deltaTime}");

            // ��ǥ���� ���� ���� �Ǵܺκ�
            if (StopEnergyBall == false)
            {
                float dist = Vector3.Distance(transform.position, Target.transform.position);
                //Debug.Log($"�ǽð� ��ǥ��ġ�� �Ѿ��� �Ÿ� : {dist}");
                if (dist < 0.5f)
                {
                    //Debug.Log($"Ÿ��({Target})�� �浹");
                    DamageTotheEnemy(Target);
                    StopEnergyBall = true;
                    PhotonNetwork.Destroy(gameObject);
                }

            }
        }

    }

    public void GetTargetObject(PlayerBehaviour behaviour)
    {
        _behaviour = behaviour;
    }

    public void GetStatData(Stats stats)
    {
        _stats = stats;
    }

    public void DamageTotheEnemy(GameObject target)
    {
        if (photonView.IsMine)
        {
            // ���±״� �⺻��������
            if (_behaviour.EnemyTag == target.tag)
            {
                // �÷��̾�
                if (target.layer == 7 && _behaviour.EnemyTag == target.tag)
                {
                    target.GetComponent<Health>().OnDamage(_damage);
                }
                // �̴Ͼ� || Ư���̴Ͼ�
                else if (target.layer == 8 || target.layer == 13)
                {
                    target.GetComponent<Enemybase>().TakeDamage(_damage);
                }
                // �ͷ�
                else if (target.layer == 6)
                {
                    target.GetComponent<Turret>().Damage(_damage);
                }
                // �ؼ���
                else if (target.layer == 12)
                {
                    target.GetComponent<NexusHp>().TakeOnDagmage(_damage);
                }

            }
        }
    }
}
