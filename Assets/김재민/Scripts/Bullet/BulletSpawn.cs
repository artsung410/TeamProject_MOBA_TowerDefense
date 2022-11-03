using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class BulletSpawn : MonoBehaviourPun
{
    // ###############################################
    //             NAME : KimJaeMin                      
    //             MAIL : woals1566@gmail.com         
    // ###############################################
    //public static BulletSpawn instance;

    //public Queue<GameObject> spawnQueue = new Queue<GameObject>();
    [SerializeField]
    GameObject Prefab;
    float elaspedTime;
    [SerializeField]
    Transform _gunPivot;

    EnemySatatus Minion;

    
    private void Awake()
    {
      
        Minion = GetComponent<EnemySatatus>();
    }    

    public void Spawn()
    {
       if(photonView.IsMine)
        {
        GameObject bullet = PhotonNetwork.Instantiate(Prefab.name, _gunPivot.position, transform.rotation);

        bullet.GetComponent<BulletMove>().tg = Minion._target;
        bullet.GetComponent<BulletMove>().Damage = Minion.Damage;
        }

    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, 14f);
    }
}
