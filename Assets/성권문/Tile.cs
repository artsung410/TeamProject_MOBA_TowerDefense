using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
    public int id;
    public bool isBuild;
    public GameObject tower;
    public void BuildTower()
    {
        isBuild = true;
    }
}
