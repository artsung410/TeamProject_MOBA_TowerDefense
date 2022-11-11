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

        // target정보 받아오고 그 타겟을 따라 가게 한다
        // target정보는 에너지볼을 생성한 플레이어에게서 가져온다 -> _behaviour.targetedEnemy.transform.position
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
    //        Debug.Log($"타겟에 맞음 : {other.gameObject.name}");
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
