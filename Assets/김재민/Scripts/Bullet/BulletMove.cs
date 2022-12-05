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
    // Update is called once per frame
    private void Awake()
    {
        rigidbody = GetComponent<Rigidbody>();
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
            transform.LookAt(tg.transform.position + new Vector3(0,2f,0));

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
                other.gameObject.GetComponent<Enemybase>().TakeDamage(Damage);
                PhotonNetwork.Destroy(gameObject);
            }
            // 타워일때 처리
            else if (other.CompareTag(EnemyTag) && other.gameObject.layer == 6)
            {
                other.gameObject.GetComponent<Turret>().Damage(Damage);
                PhotonNetwork.Destroy(gameObject);
            }
            // 플레이어일때 
            else if (other.CompareTag(EnemyTag) && other.gameObject.layer == 7)
            {
                other.gameObject.GetComponent<Health>().OnDamage(Damage);
                PhotonNetwork.Destroy(gameObject);
            }
            // 넥서스 일때
            else if (other.CompareTag(EnemyTag) && other.gameObject.layer == 12)
            {
                other.gameObject.GetComponent<NexusHp>().TakeOnDagmage(Damage);
                PhotonNetwork.Destroy(gameObject);
            }
            else if (other.CompareTag(EnemyTag) && other.gameObject.layer == 13)
            {
                other.gameObject.GetComponent<Enemybase>().TakeDamage(Damage);

                PhotonNetwork.Destroy(gameObject);
            }
            else if (other.gameObject.layer == 17)
            {
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
            }else if (other.gameObject.layer == 14)
            {
                PhotonNetwork.Destroy(gameObject);
            }


        }

    }
}
