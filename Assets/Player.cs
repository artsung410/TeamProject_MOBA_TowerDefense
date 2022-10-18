using Photon.Pun;
using UnityEngine;


// ###############################################
//             NAME : ARTSUNG                      
//             MAIL : artsung410@gmail.com         
// ###############################################

public class Player : MonoBehaviourPun
{
    //private Rigidbody playerRigidbody;
    public GameObject swordMaterial;
    private MeshRenderer swordMeshRenderer;
    //private PlayerBehaviour playerBehaviour;

    //public float speed = 3f;

    void Start()
    {
        swordMeshRenderer = swordMaterial.GetComponent<MeshRenderer>();
        //playerRigidbody = GetComponent<Rigidbody>();
        //playerBehaviour = GetComponent<PlayerBehaviour>();

        if (photonView.IsMine)
        {
            swordMeshRenderer.material.color = Color.blue;
        }
        else
        {
            swordMeshRenderer.material.color = Color.red;
        }
    }

    //private void Update()
    //{
    //    if (!photonView.IsMine)
    //    {
    //        return;
    //    }

    //    playerBehaviour.MoveTo();
    //}
}