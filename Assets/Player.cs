using Photon.Pun;
using UnityEngine;


// ###############################################
//             NAME : ARTSUNG                      
//             MAIL : artsung410@gmail.com         
// ###############################################

public class Player : MonoBehaviourPun
{
    private void OnEnable()
    {
        GameManager.Instance.CurrentPlayers.Add(gameObject);
    }
}