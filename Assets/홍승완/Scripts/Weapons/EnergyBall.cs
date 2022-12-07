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

    public GameObject Target;   // 플레이어에게서 정보 받아옴
    float _damage;              // 파싱데이터 담을부분
    public float EnergyBallSpeed; 
    public bool StopEnergyBall;

    GameObject _missileVFX;
    GameObject _hitVFX;

    PlayerBehaviour _behaviour;
    Stats _stats;

    private void Awake()
    {
        _missileVFX = transform.GetChild(0).gameObject; // 미사일 효과
        _hitVFX = transform.GetChild(1).gameObject; // 피격 효과

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
            // 총알의 목표는 Target이다 => 목적지가 정해져 있으므로 MoveToward사용할것
            transform.position = Vector3.MoveTowards(transform.position, Target.transform.position, EnergyBallSpeed * Time.deltaTime);

            // 목표지점 도달 여부 판단부분
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
            // 적태그는 기본전제조건
            if (_behaviour.EnemyTag == target.tag)
            {
                // 플레이어
                if (target.layer == 7 && _behaviour.EnemyTag == target.tag)
                {
                    target.GetComponent<Health>().OnDamage(_damage);
                }
                // 미니언 || 특수미니언
                else if (target.layer == 8 || target.layer == 13)
                {
                    target.GetComponent<Enemybase>().TakeDamage(_damage);
                }
                // 터렛
                else if (target.layer == 6)
                {
                    target.GetComponent<Turret>().Damage(_damage);
                }
                // 넥서스
                else if (target.layer == 12)
                {
                    target.GetComponent<NexusHp>().TakeOnDagmage(_damage);
                }
            }
            // 중립몬스터는 태그없음
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
