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
   
    
    public Transform tg { get;  set; }

    new Rigidbody rigidbody;
    public float turn;
    public float ballVelocity;
    public float Damage;
    public string EnemyTag;
    // Update is called once per frame
    private void Awake()
    {
        rigidbody = GetComponent<Rigidbody>();
        Damage = 10;
    }

    private void FixedUpdate()
    {
        if (tg == null)
        {
            return;
        }

        // 유도탄
        if(tg.position != null) //타켓이 있을때
        {
            rigidbody.velocity = transform.forward * ballVelocity;
            var ballTargetRotation = Quaternion.LookRotation(tg.position + new Vector3(0, 0.8f) - transform.position);
            rigidbody.MoveRotation(Quaternion.RotateTowards(transform.rotation, ballTargetRotation, turn));
        }

        else
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        // 미니언일 때 처리
        if(other.CompareTag(EnemyTag) && other.gameObject.layer == 8)
        {
            other.gameObject.GetComponent<Enemybase>().TakeDamage(Damage);
        }

        // 타워일때 처리
        if(other.CompareTag(EnemyTag) && other.gameObject.layer == 6)
        {
            other.gameObject.GetComponent<Turret>().TakeDamage(Damage);
        }
        // 플레이어일때 처리
    }

}
