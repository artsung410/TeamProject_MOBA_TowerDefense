using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// ###############################################
//             NAME : ARTSUNG                      
//             MAIL : artsung410@gmail.com         
// ###############################################

[System.Serializable]
[CreateAssetMenu(fileName = "MinionDatabaseListName", menuName = "MinionDatabaseList/Create New MinionDatabaseList")]
public class MinionDatabaseList : ScriptableObject
{
    [SerializeField]
    public List<MinionBlueprint> itemList = new List<MinionBlueprint>();
}
