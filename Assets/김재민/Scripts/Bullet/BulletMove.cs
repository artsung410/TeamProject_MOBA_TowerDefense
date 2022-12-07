//#define BULLET_VER_01
#define BULLET_VER_02
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class BulletMove : MonoBehaviourPun

{
    // ###############################################
    //             NAME : KimJaeMin                      
    //             MAIL : woals1566@gmail.com         
    // ###############################################

    public GameObject tg;

    new Rigidbody rigidbody;
    public float turn;
    public float ballVelocity;
    public float Damage;
    public string EnemyTag;

#if BULLET_VER_01
    // Update is called once per frame
    private void Awake()
    {
        rigidbody = GetComponent<Rigidbody>();
        transform.GetChild(0).gameObject.SetActive(false);
    }

    float elapsedTime;

    private void FixedUpdate()
    {
        elapsedTime += Time.deltaTime;

        // 3초지나면 자동으로 파괴
        if (elapsedTime > 3f)
        {
            if (photonView.IsMine)
            {
                PhotonNetwork.Destroy(gameObject);
            }
        }

        // 유도탄
        if (tg == null) //타켓이 없을때;
        {
            if (photonView.IsMine)
            {
                PhotonNetwork.Destroy(gameObject);
            }
        }
        else
        {
            rigidbody.velocity = transform.forward * ballVelocity;
            var ballTargetRotation = Quaternion.LookRotation(tg.transform.position + new Vector3(0, 1.5f) - transform.position);
            rigidbody.MoveRotation(Quaternion.RotateTowards(transform.rotation, ballTargetRotation, turn));
            transform.LookAt(tg.transform.position + new Vector3(0, 2f, 0));

        }
        //if(tg.tag == EnemyTag && tg.layer == 14)
        //{
        //    return;
        //}

    }

    private void OnTriggerEnter(Collider other)
    {
        // 미니언일 때 처리
        if (photonView.IsMine)
        {

            if (other.CompareTag(EnemyTag) && other.gameObject.layer == 8)
            {

                photonView.RPC(nameof(RPC_hitEffect), RpcTarget.All, true);
                other.gameObject.GetComponent<Enemybase>().TakeDamage(Damage);
                PhotonNetwork.Destroy(gameObject);
            }
            // 타워일때 처리
            else if (other.CompareTag(EnemyTag) && other.gameObject.layer == 6)
            {

                photonView.RPC(nameof(RPC_hitEffect), RpcTarget.All, true);
                other.gameObject.GetComponent<Turret>().Damage(Damage);
                PhotonNetwork.Destroy(gameObject);
            }
            // 플레이어일때 
            else if (other.CompareTag(EnemyTag) && other.gameObject.layer == 7)
            {

                photonView.RPC(nameof(RPC_hitEffect), RpcTarget.All, true);
                other.gameObject.GetComponent<Health>().OnDamage(Damage);
                PhotonNetwork.Destroy(gameObject);
            }
            // 넥서스 일때
            else if (other.CompareTag(EnemyTag) && other.gameObject.layer == 12)
            {

                photonView.RPC(nameof(RPC_hitEffect), RpcTarget.All, true);
                other.gameObject.GetComponent<NexusHp>().TakeOnDagmage(Damage);
                PhotonNetwork.Destroy(gameObject);
            }
            else if (other.CompareTag(EnemyTag) && other.gameObject.layer == 13)
            {


                photonView.RPC(nameof(RPC_hitEffect), RpcTarget.All, true);
                other.gameObject.GetComponent<Enemybase>().TakeDamage(Damage);

                PhotonNetwork.Destroy(gameObject);
            }
            else if (other.gameObject.layer == 17)
            {

                photonView.RPC(nameof(RPC_hitEffect), RpcTarget.All, true);
                other.gameObject.GetComponent<Enemybase>().TakeDamage(Damage);
                if (EnemyTag == "Blue")
                {

                    other.gameObject.GetComponent<Enemybase>().tagThrow("Red");
                }
                else
                {

                    other.gameObject.GetComponent<Enemybase>().tagThrow("Blue");

                }
                PhotonNetwork.Destroy(gameObject);
            }
            else if (other.gameObject.layer == 14)
            {

                photonView.RPC(nameof(RPC_hitEffect), RpcTarget.All, true);
                PhotonNetwork.Destroy(gameObject);
            }


        }

    }
    [PunRPC]
    private void RPC_hitEffect(bool value)
    {
        transform.GetChild(0).gameObject.SetActive(value);
        GameObject Effect = transform.GetChild(0).gameObject;
        transform.DetachChildren();
        Destroy(Effect, 0.5f);
    }
#endif
#if BULLET_VER_02
    public GameObject hitVfx;
    public GameObject bulletVfx;

    IEnumerator destroy;
    private void Awake()
    {
        rigidbody = GetComponent<Rigidbody>();
        bulletVfx = transform.GetChild(1).gameObject;
        hitVfx = transform.GetChild(0).gameObject;
        hitVfx.SetActive(false);
    }

    private void Start()
    {
        destroy = DestroyBullet();
    }

    float elapsedTime;
    bool isArrival;
    private void Update()
    {
        if (photonView.IsMine)
        {
            elapsedTime += Time.deltaTime;
            if (tg == null || tg.tag == EnemyTag && tg.layer == 14 || elapsedTime > 3f)
            {
                PhotonNetwork.Destroy(gameObject);
            }
            else
            {
                transform.position = Vector3.MoveTowards(transform.position, tg.transform.position, ballVelocity * Time.deltaTime);

                if (isArrival == false)
                {
                    float dist = Vector3.Distance(transform.position, tg.transform.position);
                    if (dist < 0.5f)
                    {
                        isArrival = true;
                        DamageTotheEnemy(tg);
                        photonView.RPC(nameof(RPC_hitEffect), RpcTarget.All);
                        StartCoroutine(destroy);
                    }
                }
            }
        }
    }

    public void DamageTotheEnemy(GameObject target)
    {
        if (photonView.IsMine)
        {
            // 적태그는 기본전제조건
            if (EnemyTag == target.tag)
            {
                // 플레이어
                if (target.layer == 7 && EnemyTag == target.tag)
                {
                    target.GetComponent<Health>().OnDamage(Damage);
                }
                // 미니언 || 특수미니언
                else if (target.layer == 8 || target.layer == 13)
                {
                    target.GetComponent<Enemybase>().TakeDamage(Damage);
                }
                // 터렛
                else if (target.layer == 6)
                {
                    target.GetComponent<Turret>().Damage(Damage);
                }
                // 넥서스
                else if (target.layer == 12)
                {
                    target.GetComponent<NexusHp>().TakeOnDagmage(Damage);
                }
            }
            // 중립몬스터는 태그없음
            else if (target.layer == 17)
            {
                target.GetComponent<Enemybase>().TakeDamage(Damage);
            }
        }


    }
  

    [PunRPC]
    private void RPC_hitEffect()
    {
        hitVfx.SetActive(true);
        bulletVfx.SetActive(false);
    }

    IEnumerator DestroyBullet()
    {
        yield return new WaitForSeconds(1f);
        PhotonNetwork.Destroy(this.gameObject);
    }
#endif




}
