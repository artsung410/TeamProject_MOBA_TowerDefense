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
        GameManager.Instance.CurrentTowers.Add(currentBuildedTower);

        if (photonView.IsMine)
        {
            currentBuildedTower.tag = "Player";
            currentBuildedTower.GetComponentInParent<Turret>().enemyTag = "Enemy";
        }

        else
        {
            currentBuildedTower.tag = "Enemy";
            currentBuildedTower.GetComponentInParent<Turret>().enemyTag = "Player";
        }
    }
}
