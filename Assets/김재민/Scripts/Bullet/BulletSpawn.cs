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
        if (PhotonNetwork.IsMasterClient)
        {


            if (Minion._target != null)
            {
                GameObject bullet = PhotonNetwork.Instantiate(Prefab.name, _gunPivot.position, _gunPivot.rotation);
                bullet.GetComponent<BulletMove>().tg = Minion._target.gameObject;
                bullet.GetComponent<BulletMove>().Damage = Minion.Damage;
                if (gameObject.CompareTag("Blue"))
                {
                    bullet.GetComponent<BulletMove>().EnemyTag = "Red";
                }
                else
                {
                    bullet.GetComponent<BulletMove>().EnemyTag = "Blue";
                }

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

}

