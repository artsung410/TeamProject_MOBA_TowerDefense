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
        if (photonView.IsMine)
        {
            GameObject bullet = PhotonNetwork.Instantiate(Prefab.name, _gunPivot.position, transform.rotation);
            bullet.GetComponent<BulletMove>().tg = Minion._target;
            bullet.GetComponent<BulletMove>().Damage = Minion.Damage;

            if (bullet.GetComponent<BulletMove>().tg == Minion._PrevTarget)
            {
                PhotonNetwork.Destroy(bullet);
            }

            if (gameObject.CompareTag("Blue"))
            {
                bullet.GetComponent<BulletMove>().CompareTag("Blue");
                bullet.GetComponent<BulletMove>().EnemyTag = "Red";

            }
            else
            {
                bullet.GetComponent<BulletMove>().CompareTag("Red");
                bullet.GetComponent<BulletMove>().EnemyTag = "Blue";
            }



        }
    }
}
