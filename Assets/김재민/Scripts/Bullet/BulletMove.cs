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

        // ����ź
        if(tg.position != null) //Ÿ���� ������
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
        // �̴Ͼ��� �� ó��
        if(other.CompareTag(EnemyTag) && other.gameObject.layer == 8)
        {
            other.gameObject.GetComponent<Enemybase>().TakeDamage(Damage);
        }

        // Ÿ���϶� ó��
        if(other.CompareTag(EnemyTag) && other.gameObject.layer == 6)
        {
            other.gameObject.GetComponent<Turret>().TakeDamage(Damage);
        }
        // �÷��̾��϶� ó��
    }

}
