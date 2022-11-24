using System.Collections;
using System.Collections.Generic;
using UnityEngine;


    // ###############################################
    //             NAME : ARTSUNG                      
    //             MAIL : artsung410@gmail.com         
    // ###############################################

[System.Serializable]
[CreateAssetMenu(fileName = "BuffDatabaseListName", menuName = "BuffDatabaseList/Create New BuffDatabaseList")]
public class BuffDatabaseList : ScriptableObject
{
    [SerializeField]
    public List<BuffBlueprint> itemList = new List<BuffBlueprint>();
}

