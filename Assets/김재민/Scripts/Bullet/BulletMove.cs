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
    [HideInInspector]
    public float Damage;
    public string EnemyTag;
    // Update is called once per frame
    private void Awake()
    {
        rigidbody = GetComponent<Rigidbody>();
    }

    private void Start()
    {
        Destroy(gameObject, 3f);
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
        if(other.CompareTag(EnemyTag))
        {
            if(other.gameObject.layer == 6) // Ÿ��
            {
                other.gameObject.GetComponent<Turret>().Damage(Damage);
                Destroy(gameObject);
            }
            if (other.gameObject.layer == 7) // �÷��̾�
            {
                other.gameObject.GetComponent<Health>().OnDamage(Damage);
                Destroy(gameObject);
            }
            if (other.gameObject.layer == 8) // �̴Ͼ�
            {
                other.gameObject.GetComponent<Enemybase>().TakeDamage(Damage);
                Destroy(gameObject);
            }
            if (other.gameObject.layer == 12) // �ؼ���
            {
                other.gameObject.GetComponent<NexusHp>().TakeOnDagmage(Damage);
                Destroy(gameObject);
            }
            if (other.gameObject.layer == 13) // Ư���̴Ͼ�
            {
                other.gameObject.GetComponent<Enemybase>().TakeDamage(Damage);
                Destroy(gameObject);
            }
        }

    }

}
