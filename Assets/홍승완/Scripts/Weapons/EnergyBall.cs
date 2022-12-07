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

    public GameObject Target;   // �÷��̾�Լ� ���� �޾ƿ�
    float _damage;              // �Ľ̵����� �����κ�
    public float EnergyBallSpeed; 
    public bool StopEnergyBall;

    GameObject _missileVFX;
    GameObject _hitVFX;

    PlayerBehaviour _behaviour;
    Stats _stats;

    private void Awake()
    {
        _missileVFX = transform.GetChild(0).gameObject; // �̻��� ȿ��
        _hitVFX = transform.GetChild(1).gameObject; // �ǰ� ȿ��

        _hitVFX.SetActive(false);
    }

    void Start()
    {
        if (_behaviour == null || !photonView.IsMine)
        {
            return;
        }

        Target = _behaviour.targetedEnemy;
        _damage = _stats.attackDmg;
        EnergyBallSpeed = 30f;

        myCoroutine = DeleteEnergyBall();
    }

    float elapsedTime;
    void Update()
    {
        if (!photonView.IsMine)
        {
            return;
        }

        if (Target == null)
        {
            PhotonNetwork.Destroy(gameObject);
        }
        else
        {
            // �Ѿ��� ��ǥ�� Target�̴� => �������� ������ �����Ƿ� MoveToward����Ұ�
            transform.position = Vector3.MoveTowards(transform.position, Target.transform.position, EnergyBallSpeed * Time.deltaTime);

            // ��ǥ���� ���� ���� �Ǵܺκ�
            if (StopEnergyBall == false)
            {
                float dist = Vector3.Distance(transform.position, Target.transform.position);

                if (dist < 0.5f)
                {
                    StopEnergyBall = true;
                    DamageTotheEnemy(Target);
                    photonView.RPC(nameof(RPC_EnergyBallVfx), RpcTarget.All);
                    StartCoroutine(myCoroutine);
                }

            }
        }
    }

    [PunRPC]
    public void RPC_EnergyBallVfx()
    {
        _hitVFX.SetActive(true);
        _missileVFX.SetActive(false);
    }

    IEnumerator myCoroutine;
    IEnumerator DeleteEnergyBall()
    {
        yield return new WaitForSeconds(1f);
        PhotonNetwork.Destroy(gameObject);
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
            // �߸����ʹ� �±׾���
            else if (target.layer == 17)
            {
                target.GetComponent<Enemybase>().TakeDamage(_damage);
            }
        }


    }

    private void OnDisable()
    {
        StopAllCoroutines();
    }
}
