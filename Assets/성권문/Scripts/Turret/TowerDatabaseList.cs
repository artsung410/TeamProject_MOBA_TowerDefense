using UnityEngine;
using System.Collections;
using System.Collections.Generic;

// ###############################################
//             NAME : ARTSUNG                      
//             MAIL : artsung410@gmail.com         
// ###############################################


[System.Serializable]
[CreateAssetMenu(fileName = "TowerDatabaseListName", menuName = "TowerDatabaseList/Create New TowerDatabaseList")]
public class TowerDatabaseList : ScriptableObject
{       
    [SerializeField]
    public List<TowerBlueprint> itemList = new List<TowerBlueprint>(); 
}
