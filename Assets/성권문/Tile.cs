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
        currentBuildedTower = PhotonNetwork.Instantiate(towerPrefab.name, transform.position, Quaternion.identity);

        photonView.RPC("SetTag", RpcTarget.All);
    }

    [PunRPC]
    private void SetTag()
    {
        if (id < 4)
        {
            currentBuildedTower.tag = "Blue";
            currentBuildedTower.GetComponent<Turret>().enemyTag = "Red";
        }
        else
        {
            currentBuildedTower.tag = "Red";
            currentBuildedTower.GetComponent<Turret>().enemyTag = "Blue";
        }
    }
}
