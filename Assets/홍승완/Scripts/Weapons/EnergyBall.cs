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

    PlayerBehaviour _behaviour;

    public float BulletSpeed;

    public GameObject Check;

    void Start()
    {
        Debug.Log("EnergyBall Start");
    }

    void Update()
    {
        Debug.Log("EnergyBall Update");
        if (_behaviour == null)
        {
            Debug.Log("EnergyBall _behaviour is null");
            return;
        }

        // target���� �޾ƿ��� �� Ÿ���� ���� ���� �Ѵ�
        // target������ ���������� ������ �÷��̾�Լ� �����´� -> _behaviour.targetedEnemy.transform.position
        //this.transform.Translate(Time.deltaTime * BulletSpeed * _behaviour.targetedEnemy.transform.position);

        //transform.LookAt(_behaviour.targetedEnemy.transform);
    }

    //public void HitTarget()
    //{
    //    float dist = Vector3.Distance(transform.position, _behaviour.targetedEnemy.transform.position);
    //    if (dist <= )
    //    {

    //    }
    //}

    //private void OnTriggerEnter(Collider other)
    //{
    //    if (other.gameObject == _behaviour.targetedEnemy)
    //    {
    //        Debug.Log($"Ÿ�ٿ� ���� : {other.gameObject.name}");
    //        if (photonView.IsMine)
    //        {
    //            PhotonNetwork.Destroy(this.gameObject);
    //        }
    //    }
    //}


    public void GetTargetObject(PlayerBehaviour behaviour)
    {
        _behaviour = behaviour;
    }
}
