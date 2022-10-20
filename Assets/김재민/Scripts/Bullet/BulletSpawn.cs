using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

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

    EnemySatatus Minion;

    
    private void Awake()
    {
        Minion = GetComponent<EnemySatatus>();

    }    

    public void Spawn()
    {
        GameObject bullet = PhotonNetwork.Instantiate(Prefab.name, _gunPivot.position, transform.rotation);

        bullet.GetComponent<BulletMove>().tg = Minion._target;
        StartCoroutine(DeactivationBullet(bullet));
    }

    private IEnumerator DeactivationBullet(GameObject bullet)
    {
        yield return new WaitForSeconds(3f);
        PhotonNetwork.Destroy(bullet);
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
