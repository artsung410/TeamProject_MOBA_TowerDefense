using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class Tile : MonoBehaviourPun
{
    public int id;
    public bool isBuild;
    public GameObject currentBuildedTower;

    public void BuildTower(GameObject towerPrefab)
    {
        isBuild = true;
        GameObject currentBuildedTower = PhotonNetwork.Instantiate(towerPrefab.name, transform.position, Quaternion.identity);

        photonView.RPC("SetLayer", RpcTarget.All, currentBuildedTower);
    }

    //[PunRPC]
    //private void SetLayer(GameObject tower)
    //{
    //    if (id < 4)
    //    {
    //        tower.layer = 10;
    //    }
    //    else
    //    {
    //        tower.layer = 11;
    //    }
    //}
}
