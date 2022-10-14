using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletSpawn : MonoBehaviour
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

    ShotEnemy ShotSatatus;

    
    private void Awake()
    {
       ShotSatatus = GetComponent<ShotEnemy>();

    }    

    

    public void Spawn()
    {if(ShotSatatus._target == null)
        {
            ShotSatatus._target = ShotSatatus._PrevTarget;
        }

        GameObject bullet = Instantiate(Prefab, _gunPivot.position, transform.rotation);

        bullet.GetComponent<BulletMove>().tg = ShotSatatus._target;
    }

    //public void InsertQueue(GameObject P_object)
    //{
    //    spawnQueue.Enqueue(P_object);
    //    P_object.SetActive(false);
    //}

    //public GameObject GetQueue()
    //{
    //    GameObject t_object = spawnQueue.Dequeue();
    //    t_object.SetActive(true);
    //    return t_object;
    //}












}
